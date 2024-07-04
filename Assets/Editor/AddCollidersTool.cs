using UnityEngine;
using UnityEditor;

public class AddCollidersTool : EditorWindow
{
    [MenuItem("Tools/Add Colliders to Finished Fence")]
    public static void ShowWindow()
    {
        GetWindow<AddCollidersTool>("Add Colliders");
    }

    void OnGUI()
    {
        GUILayout.Label("Add Colliders to Finished Fence", EditorStyles.boldLabel);

        if (GUILayout.Button("Add Colliders to Selected Object"))
        {
            CheckIfSelected();
        }
    }


    private void CheckIfSelected()
    {
        if (Selection.activeGameObject != null)
        {
            AddCollidersToSelectedObject(Selection.activeGameObject);
        }
        else
        {
            Debug.LogError("No object selected.");
        }
    }


    private void AddCollidersToSelectedObject(GameObject obj)
    {
        if (obj.name.Contains("[Finished Fence]"))
        {
            foreach (Transform child in obj.transform)
            {
                foreach (Transform child2 in child.transform)
                {
                    foreach (Transform child3 in child2.transform)
                    {
                        if (child3.gameObject.name.Contains("Ornate") || child3.gameObject.name.Contains("ArcWoodSlats") || child3.gameObject.name.Contains("WoodenArc"))
                        {
                            BoxCollider box_collider = child3.gameObject.GetComponent<BoxCollider>();
                            if (box_collider != null)
                            {
                                DestroyImmediate(box_collider);
                            }
                            MeshCollider mesh_collider = child3.gameObject.GetComponent<MeshCollider>();
                            if (mesh_collider != null)
                            {
                                DestroyImmediate(mesh_collider);
                            }
                            child3.gameObject.AddComponent<MeshCollider>();
                        }
                        else if (child3.gameObject.GetComponent<Collider>() == null)
                        {
                            child3.gameObject.AddComponent<BoxCollider>();
                        }
                    }
                }
            }
            Debug.Log("Colliders added to all child objects of [Finished Fence] that did not have one.");
        }
        else
        {
            foreach (Transform child in obj.transform)
            {
                AddCollidersToSelectedObject(child.gameObject);
            }
        }
    }
}
