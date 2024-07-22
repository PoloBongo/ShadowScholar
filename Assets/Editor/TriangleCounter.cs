using UnityEditor;
using UnityEngine;

public class TriangleCounter : EditorWindow
{
    private GameObject selectedObject;

    [MenuItem("Tools/Triangle Counter")]
    public static void ShowWindow()
    {
        GetWindow<TriangleCounter>("Triangle Counter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Select a GameObject to count triangles", EditorStyles.boldLabel);

        selectedObject = EditorGUILayout.ObjectField("GameObject", selectedObject, typeof(GameObject), true) as GameObject;

        if (selectedObject != null)
        {
            if (GUILayout.Button("Count Triangles"))
            {
                int totalTriangles = CountTriangles(selectedObject);
                EditorUtility.DisplayDialog("Triangle Count", "Total Triangles: " + totalTriangles, "OK");
            }
        }
    }

    private int CountTriangles(GameObject obj)
    {
        int triangleCount = 0;

        MeshFilter[] meshFilters = obj.GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter meshFilter in meshFilters)
        {
            if (meshFilter.sharedMesh != null)
            {
                triangleCount += meshFilter.sharedMesh.triangles.Length / 3;
            }
        }

        SkinnedMeshRenderer[] skinnedMeshRenderers = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
        {
            if (skinnedMeshRenderer.sharedMesh != null)
            {
                triangleCount += skinnedMeshRenderer.sharedMesh.triangles.Length / 3;
            }
        }

        return triangleCount;
    }
}
