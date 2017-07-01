using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform objectToFollow;

    private void LateUpdate()
    {
        if(objectToFollow != null)
        {
            transform.position = new Vector3(objectToFollow.position.x, objectToFollow.position.y, transform.position.z);
        }
    }
}