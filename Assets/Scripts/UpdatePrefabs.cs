using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class UpdatePrefabs : MonoBehaviour
{
    public static UpdatePrefabs updatePrefabs;

    public DictionaryStringGameObject entityPrefabs;

    public void UpdateAllPrefabs()
    {
        entityPrefabs = new DictionaryStringGameObject();
        updatePrefabs = this;
        if (!EditorApplication.isPlayingOrWillChangePlaymode)
        {
            // Get the EntityBuilder component of every prefab that has one
            List<string> prefabPaths = new List<string>(Directory.GetFiles(Application.dataPath, "*.prefab", SearchOption.AllDirectories)); // 1) Get system paths for all prefabs in project
            prefabPaths = prefabPaths.ConvertAll<string>(x => "Assets" + x.Replace(Application.dataPath, "").Replace('\\', '/'));           // 2) Convert system paths to asset paths
            List<GameObject> prefabs = prefabPaths.ConvertAll<GameObject>(x => AssetDatabase.LoadAssetAtPath<GameObject>(x));               // 3) Load paths as Game Objects
            prefabs.RemoveAll(x => x.GetComponent<EntityBuilder>() == null);                                                                // 4) Narrow down to just entities
            List<EntityBuilder> entities = prefabs.ConvertAll<EntityBuilder>(x => x.GetComponent<EntityBuilder>());                         // 5) Get EntityBuilder component
            entities.ForEach(x => x.SetupPrefab());
        }
    }
}