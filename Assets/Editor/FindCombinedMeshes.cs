using UnityEngine;
using UnityEditor;

public class FindCombinedMeshes : MonoBehaviour
{
    [MenuItem("Tools/Find Combined Meshes")]
    static void FindAllCombinedMeshes()
    {
        MeshFilter[] meshFilters = FindObjectsOfType<MeshFilter>();
        foreach (MeshFilter meshFilter in meshFilters)
        {
            if (meshFilter.sharedMesh != null && meshFilter.sharedMesh.name.Contains("Combined Mesh"))
            {
                Debug.Log($"Found Combined Mesh: {meshFilter.sharedMesh.name} in GameObject: {meshFilter.gameObject.name}");
            }
        }
    }
}
