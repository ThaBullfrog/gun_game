using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public GameObject thePlayer;

    private static Game obj;
    public static Transform clones { get; private set; }
    public static GameObject player { get { return obj.thePlayer; } set { obj.thePlayer = value; } }
    
    private Vector3 playerSpawnPosition;

    private void Awake()
    {
        if(obj != null)
        {
            Destroy(gameObject);
        }
        else
        {
            obj = this;
        }
    }

    private void Start()
    {
        clones = new GameObject("clones").transform;
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(player);
        playerSpawnPosition = player.transform.position;
    }

    private void OnLevelWasLoaded(int level)
    {
        if (player != null)
        {
            Camera.main.GetComponent<CameraFollow>().objectToFollow = player.transform;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
            if (player != null)
            {
                player.transform.position = playerSpawnPosition;
            }
        }
    }

    public static void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}