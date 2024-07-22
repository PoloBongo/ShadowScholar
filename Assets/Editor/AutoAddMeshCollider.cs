using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class AutoAddMeshCollider : MonoBehaviour
{
    [MenuItem("GameObject/Add Mesh Colliders to Children", false, 10)]
    static void Start()
    {
        // Récupère le GameObject sélectionné dans la hiérarchie
        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject == null)
        {
            Debug.LogWarning("Please select a GameObject in the hierarchy.");
            return;
        }

        // Ajoute les MeshColliders aux enfants
        AutoAddMeshColliderFunction(selectedObject.transform);
        Debug.Log("Mesh Colliders added to children of " + selectedObject.name);
    }

    static void AutoAddMeshColliderFunction(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.GetComponent<MeshRenderer>() != null)
            {
                if (child.GetComponent<MeshRenderer>() != null && child.GetComponent<MeshCollider>() == null)
                {
                    child.gameObject.AddComponent<MeshCollider>();
                }
            }
            AutoAddMeshColliderFunction(child);
        }
    }
}
