using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

[Serializable]
public class SceneData
{
    public CharacterData player;
    public List<CharacterData> characters;
    public List<CheckpointData> checkpoints;
}

[Serializable]
public class CharacterData
{
    public SerializableVector2 location;
    public SerializableQuaternion rotation;
    public bool dead;
    public string prefabName;
    public string bodyPrefabName;
    public bool facingRight;
}

[Serializable]
public class CheckpointData
{
    public SerializableVector2 location;
    public bool active;
}