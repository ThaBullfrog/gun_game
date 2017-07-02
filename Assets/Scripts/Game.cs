using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Game : MonoBehaviour
{
    public GameObject thePlayer;

    private static Game obj;
    public static Transform clones { get; private set; }
    public static GameObject player { get { return obj.thePlayer; } set { obj.thePlayer = value; } }

    private bool firstOnGUI = true;

    private void Awake()
    {
        if(obj == null)
        {
            obj = this;
            //SceneManager.sceneLoaded += OnLevelLoaded;
        }
        else if(obj != this)
        {
            Destroy(gameObject);
        }
        if (obj == null || obj == this)
        {
            LoadingScreen.Show();
            StartCoroutine(WaitSecondsThenExecute(1f, LoadIfSaveAvailable));
        }
    }

    private void Start()
    {
        clones = new GameObject("clones").transform;
        DontDestroyOnLoad(gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        Awake();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    public static void RestartLevel()
    {
        if(!SaveAvailable())
        {
            Destroy(obj.gameObject);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator WaitSecondsThenExecute(float seconds, System.Action func)
    {
        yield return new WaitForSecondsRealtime(seconds);
        if(func != null)
        {
            func();
        }
    }

    public static bool SaveAvailable()
    {
        return File.Exists(Application.persistentDataPath + "/save.dat");
    }

    public static void LoadIfSaveAvailable()
    {
        if (SaveAvailable())
        {
            SavedObjectsTracker.DestroyAllSavedObjects();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            SceneData data = (SceneData)bf.Deserialize(file);
            file.Close();
            foreach (CharacterData characterData in data.characters)
            {
                Vector3 location = characterData.location.regular.Vector3();
                Quaternion rotation = characterData.rotation.regular;
                if (!characterData.dead)
                {
                    GameObject prefab = Resources.Load<GameObject>(characterData.prefabName);
                    GameObject obj = Instantiate(prefab, location, rotation);
                    if (data.player == characterData)
                    {
                        Game.player = obj;
                        CameraFollow mainCamFollow = Camera.main.GetComponent<CameraFollow>();
                        if (mainCamFollow != null)
                        {
                            mainCamFollow.objectToFollow = obj.transform;
                        }
                    }
                    CharacterSpriteController characterSpriteController = obj.GetComponent<CharacterSpriteController>();
                    if (characterSpriteController != null)
                    {
                        characterSpriteController.facingRight = characterData.facingRight;
                    }
                }
                else
                {
                    Transform prefab = Resources.Load<Transform>(characterData.bodyPrefabName);
                    Instantiate(prefab, location, rotation);
                }
            }
            foreach(CheckpointData checkpointData in data.checkpoints)
            {
                Checkpoint checkpoint = Instantiate(Resources.Load<Transform>("Checkpoint"), checkpointData.location.regular.Vector3(), Quaternion.identity).GetComponent<Checkpoint>();
                if(checkpointData.active)
                {
                    checkpoint.ActivateWithoutSave();
                }
            }
        }
        LoadingScreen.Hide();
    }

    public static void Save()
    {
        SceneData data = new SceneData();
        List<CharacterData> allCharacterData = new List<CharacterData>();
        foreach(GameObject obj in SavedObjectsTracker.characters)
        {
            CharacterData characterData = new CharacterData();
            IDead dead = obj.GetComponent<IDead>();
            PrefabNames prefabNames = obj.GetComponent<PrefabNames>();
            if(prefabNames != null)
            {
                characterData.prefabName = prefabNames.prefabName;
                characterData.bodyPrefabName = prefabNames.bodyPrefabName;
            }
            if(dead != null && dead.dead)
            {
                characterData.dead = true;
                characterData.location = dead.body.transform.position.Vector2().ToSerializable();
                characterData.rotation = dead.body.transform.rotation.ToSerializable();
            }
            else
            {
                characterData.dead = false;
                characterData.location = obj.transform.position.Vector2().ToSerializable();
                characterData.rotation = obj.transform.rotation.ToSerializable();
            }
            if(player == obj)
            {
                data.player = characterData;
            }
            CharacterSpriteController characterSpriteController = obj.GetComponent<CharacterSpriteController>();
            if(characterSpriteController != null)
            {
                characterData.facingRight = characterSpriteController.facingRight;
            }
            allCharacterData.Add(characterData);
        }
        data.characters = allCharacterData;
        List<CheckpointData> allCheckpointData = new List<CheckpointData>();
        foreach(GameObject obj in SavedObjectsTracker.checkpoints)
        {
            Checkpoint checkpoint = obj.GetComponent<Checkpoint>();
            if(checkpoint != null)
            {
                CheckpointData checkpointData = new CheckpointData();
                checkpointData.location = checkpoint.transform.position.Vector2().ToSerializable();
                checkpointData.active = checkpoint.active;
                allCheckpointData.Add(checkpointData);
            }
        }
        data.checkpoints = allCheckpointData;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");
        bf.Serialize(file, data);
        file.Close();
    }

    public static void DeleteSave()
    {
        File.Delete(Application.persistentDataPath + "/save.dat");
    }

    private void OnGUI()
    {
        if(firstOnGUI)
        {
            firstOnGUI = false;
            GUIUtils.SetupGUIStyles();
        }
    }
}