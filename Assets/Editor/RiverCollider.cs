using UnityEngine;
using UnityEditor;

public class RiverColliderTool : EditorWindow
{
    [MenuItem("Tools/Add BoxCollider to River")]
    public static void ShowWindow()
    {
        GetWindow<RiverColliderTool>("Add BoxCollider to River");
    }

    void OnGUI()
    {
        GUILayout.Label("Add BoxCollider to River", EditorStyles.boldLabel);

        if (GUILayout.Button("Add/Update BoxCollider to Selected Object"))
        {
            CheckIfSelected();
        }
    }

    private void CheckIfSelected()
    {
        if (Selection.activeGameObject != null)
        {
            AddOrUpdateBoxCollider(Selection.activeGameObject);
        }
        else
        {
            Debug.LogError("No object selected.");
        }
    }

    private void AddOrUpdateBoxCollider(GameObject obj)
    {
        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("No MeshFilter found on the selected object.");
            return;
        }

        Mesh mesh = meshFilter.sharedMesh;
        if (mesh == null)
        {
            Debug.LogError("No mesh found on the MeshFilter.");
            return;
        }

        Bounds bounds = mesh.bounds;

        BoxCollider boxCollider = obj.GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            boxCollider = obj.AddComponent<BoxCollider>();
        }

        boxCollider.isTrigger = true;
        boxCollider.center = bounds.center;
        boxCollider.size = bounds.size;

        // Adjust to world space if needed
        boxCollider.center = obj.transform.TransformPoint(bounds.center);
        boxCollider.size = Vector3.Scale(bounds.size, obj.transform.localScale);

        Debug.Log("BoxCollider has been added/updated.");
    }
}
