using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class MeshExtractor : EditorWindow
{
    private string meshName = "";

    private int nbRoadMesh = 0;

    [MenuItem("Tools/Mesh Extractor")]
    public static void ShowWindow()
    {
        GetWindow<MeshExtractor>("Mesh Extractor");
    }

    void OnGUI()
    {
        GUILayout.Label("Extract and Recreate Mesh", EditorStyles.boldLabel);
        meshName = EditorGUILayout.TextField("Mesh Name", meshName);

        if (GUILayout.Button("Extract and Recreate Mesh"))
        {
            ExtractAndRecreateMesh(meshName);
        }
    }

    void ExtractAndRecreateMesh(string meshName)
    {
        nbRoadMesh = CountFilesInDirectory("Assets/Assets/Props/Road");

        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject == null)
        {
            Debug.LogError("No GameObject selected.");
            return;
        }

        MeshFilter[] meshFilters = selectedObject.GetComponentsInChildren<MeshFilter>();
        List<MeshFilter> matchingMeshFilters = new List<MeshFilter>();

        foreach (MeshFilter meshFilter in meshFilters)
        {
            if (meshFilter.sharedMesh != null && meshFilter.sharedMesh.name == meshName)
            {
                Debug.Log($"Mesh found in GameObject: {meshFilter.gameObject.name}");
                matchingMeshFilters.Add(meshFilter);
            }
        }

        if (matchingMeshFilters.Count == 0)
        {
            Debug.LogError("No meshes found with the specified name ");
            return;
        }

        foreach (MeshFilter meshFilter in matchingMeshFilters)
        {
            Mesh newMesh = RecreateMesh(meshFilter.sharedMesh);
            SaveMesh(newMesh, $"Assets/Assets/Props/Road/Road_Mesh_{nbRoadMesh}.asset");

            
            meshFilter.mesh = newMesh;

            MeshCollider meshCollider = meshFilter.gameObject.GetComponent<MeshCollider>();
            if (meshCollider != null)
            {
                meshCollider.sharedMesh = newMesh;
            }

            nbRoadMesh += 1;
        }
    }

    Mesh RecreateMesh(Mesh originalMesh)
    {
        Mesh newMesh = new Mesh();
        newMesh.vertices = originalMesh.vertices;
        newMesh.normals = originalMesh.normals;
        newMesh.uv = originalMesh.uv;
        newMesh.triangles = originalMesh.triangles;
        newMesh.name = originalMesh.name + "_Copy";
        return newMesh;
    }

    void SaveMesh(Mesh mesh, string path)
    {
        AssetDatabase.CreateAsset(mesh, path);
        AssetDatabase.SaveAssets();
        Debug.Log("Mesh saved to " + path);
    }

    static int CountFilesInDirectory(string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            string[] files = Directory.GetFiles(folderPath);
            return files.Length;
        }
        else
        {
            Debug.LogError($"Le dossier '{folderPath}' n'existe pas.");
            return 0;
        }
    }
}
