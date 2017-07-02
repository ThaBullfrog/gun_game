using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[System.Serializable]
public struct SerializableQuaternion
{
    public float w;
    public float x;
    public float y;
    public float z;

    public SerializableQuaternion(Quaternion quaternion)
    {
        x = quaternion.x;
        y = quaternion.y;
        z = quaternion.z;
        w = quaternion.w;
    }

    public Quaternion regular { get { return new Quaternion(x, y, z, w); } }
}