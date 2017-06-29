using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject thePlayer;

    private static Game obj;
    public static Transform clones { get; private set; }
    public static GameObject player { get { return obj.thePlayer; } }

    private void Awake()
    {
        obj = this;
        clones = new GameObject("clones").transform;
    }
}