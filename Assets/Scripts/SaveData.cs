using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

[Serializable]
public class SceneData
{
    public CharacterData player;
    public List<CharacterData> characters;
}

[Serializable]
public class CharacterData
{
    public float locationX;
    public float locationY;
    public float rotationX;
    public float rotationY;
    public float rotationZ;
    public float rotationW;
    public bool dead;
    public string prefabName;
    public string bodyPrefabName;
    public bool facingRight;
}