using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SetAsPlayer : MonoBehaviour
{
    private void Start()
    {
        if(Game.player != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Game.player = gameObject;
        }
    }
}