using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[DisallowMultipleComponent]
public class EntityBuilder : MonoBehaviour
{
    public static GameObject GetPrefab(System.Type type)
    {
        if(!(type.IsSubclassOf(typeof(EntityBuilder)) || type == typeof(EntityBuilder)))
        {
            Debug.Log("Cannot get prefab for a type that does not extend EntityBuilder");
            return null;
        }
        Dictionary<string, GameObject> entityPrefabs = UpdatePrefabs.updatePrefabs.entityPrefabs;
        if (entityPrefabs == null || (!entityPrefabs.ContainsKey(type.Name)) || entityPrefabs[type.Name] == null)
        {
            Debug.Log("Could not get prefab for type '" + type.Name + "'");
            return null;
        }
        return entityPrefabs[type.Name];
    }

    public static GameObject GetPrefab<T>() where T : EntityBuilder
    {
        return GetPrefab(typeof(T));
    }

    public virtual void Build()
    {

    }

    public void SetupPrefab()
    {
#if UNITY_EDITOR
        if (this != null && PrefabUtility.GetPrefabType(this) == PrefabType.Prefab)
        {
            Dictionary<string, GameObject> entityPrefabs = UpdatePrefabs.updatePrefabs.entityPrefabs;
            GameObject obj = new GameObject();
            ((EntityBuilder)obj.AddComponent(GetType())).Build();
            GameObject newPrefabObj = PrefabUtility.ReplacePrefab(obj, this, ReplacePrefabOptions.ReplaceNameBased);
            if (!entityPrefabs.ContainsKey(GetType().Name))
            {
                entityPrefabs.Add(GetType().Name, newPrefabObj);
            }
            else
            {
                entityPrefabs[GetType().Name] = newPrefabObj;
            }
            EditorApplication.delayCall += () => DestroyImmediate(obj);
        }
#endif
    }

    protected T GetAsset<T>(string path) where T : Object
    {
#if UNITY_EDITOR
        if (!EditorApplication.isPlayingOrWillChangePlaymode)
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null)
            {
                return asset;
            }
        }
        else
        {
            Debug.Log("Can't load asset in play mode. Tried to load '" + path + "'");
            return null;
        }
#endif
        if(Application.isPlaying)
        {
            Debug.Log("Can't load asset in play mode. Tried to load '" + path + "'");
            return null;
        }
        Debug.Log("Asset at path '" + path + "' not found.");
        return null;
    }

    public static EntityBuilder CreateEntity(System.Type type)
    {
        if(!(type.IsSubclassOf(typeof(EntityBuilder)) || type == typeof(EntityBuilder)))
        {
            Debug.Log("You called function 'CreateEntity' with type parameter '" + type.Name + "' which is not a subclass of type 'EntityBuilder'");
            return null;
        }
#if UNITY_EDITOR
        if(!EditorApplication.isPlayingOrWillChangePlaymode)
        {
            GameObject obj = new GameObject(type.Name);
            EntityBuilder builder = (EntityBuilder)obj.AddComponent(type);
            builder.Build();
            return builder;
        }
#endif
        GameObject prefab = GetPrefab(type);
        if(prefab == null)
        {
            Debug.Log("No prefab exists for entity type '" + type.Name + "'");
            return null;
        }
        return (EntityBuilder)Instantiate<GameObject>(prefab).GetComponent(type);
    }

    public static T CreateEntity<T>() where T : EntityBuilder
    {
        return (T)CreateEntity(typeof(T));
    }
}