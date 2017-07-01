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

    private void Awake()
    {
        if(obj == null)
        {
            obj = this;
        }
        else if(obj != this)
        {
            Destroy(gameObject);
        }
        if (obj == null || obj == this)
        {
            StartCoroutine(WaitSecondsThenExecute(0.5f, LoadIfSaveAvailable));
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
        if (Input.GetKeyDown(KeyCode.G))
        {
            Save();
        }
    }

    public static void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator WaitSecondsThenExecute(float seconds, System.Action func)
    {
        yield return new WaitForSeconds(seconds);
        if(func != null)
        {
            func();
        }
    }

    public void LoadIfSaveAvailable()
    {
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            CharacterTracker.DestroyAllCharacters();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            SceneData data = (SceneData)bf.Deserialize(file);
            file.Close();
            foreach (CharacterData characterData in data.characters)
            {
                Vector3 location = new Vector3(characterData.locationX, characterData.locationY, 0f);
                Quaternion rotation = new Quaternion(characterData.rotationX, characterData.rotationY, characterData.rotationZ, characterData.rotationW);
                if (!characterData.dead)
                {
                    GameObject prefab = Resources.Load<GameObject>(characterData.prefabName);
                    GameObject obj = Instantiate(prefab, location, rotation);
                    if(data.player == characterData)
                    {
                        Game.player = obj;
                        CameraFollow mainCamFollow = Camera.main.GetComponent<CameraFollow>();
                        if(mainCamFollow != null)
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
        }
    }

    public void Save()
    {
        SceneData data = new SceneData();
        List<CharacterData> allCharacterData = new List<CharacterData>();
        foreach(GameObject obj in CharacterTracker.characters)
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
                characterData.locationX = dead.body.transform.position.x;
                characterData.locationY = dead.body.transform.position.y;
                characterData.rotationW = dead.body.transform.rotation.w;
                characterData.rotationX = dead.body.transform.rotation.x;
                characterData.rotationY = dead.body.transform.rotation.y;
                characterData.rotationZ = dead.body.transform.rotation.z;
            }
            else
            {
                characterData.dead = false;
                characterData.locationX = obj.transform.position.x;
                characterData.locationY = obj.transform.position.y;
                characterData.rotationW = obj.transform.rotation.w;
                characterData.rotationX = obj.transform.rotation.x;
                characterData.rotationY = obj.transform.rotation.y;
                characterData.rotationZ = obj.transform.rotation.z;
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
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");
        bf.Serialize(file, data);
        file.Close();
    }
}