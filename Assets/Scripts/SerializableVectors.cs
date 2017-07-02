using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[System.Serializable]
public struct SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3 regular { get { return new Vector3(x, y, z); } }
}

[System.Serializable]
public struct SerializableVector2
{
    public float x;
    public float y;

    public SerializableVector2(Vector2 vector)
    {
        x = vector.x;
        y = vector.y;
    }

    public Vector2 regular { get { return new Vector2(x, y); } }
}