using UnityEngine;
using UnityEditor;

public class EnableReadWriteOnMeshes : EditorWindow
{
    [MenuItem("Tools/Enable Read/Write on All Meshes")]
    public static void EnableReadWrite()
    {
        string[] guids = AssetDatabase.FindAssets("t:Model");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ModelImporter importer = AssetImporter.GetAtPath(path) as ModelImporter;
            if (importer != null && !importer.isReadable)
            {
                importer.isReadable = true;
                AssetDatabase.ImportAsset(path);
                Debug.Log($"Read/Write Enabled on mesh: {path}");
            }
        }

        Debug.Log("Read/Write Enabled on all meshes.");
    }
}
