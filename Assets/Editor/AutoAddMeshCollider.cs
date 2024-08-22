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
            if (child.name.Contains("(POLY FEW)"))
            {
                if (child.GetComponent<Renderer>() != null)
                {
                    Collider[] colliders = child.GetComponents<Collider>();
                    bool no_collider = true;
                    foreach (Collider collider in colliders)
                    {
                        if (!collider.isTrigger)
                        {
                            if (no_collider)
                            {
                                no_collider = false;
                            }
                            else
                            {
                                DestroyImmediate(collider);
                            }
                        }
                    }

                    if (colliders.Length == 0 || no_collider)
                    {
                        child.gameObject.AddComponent<MeshCollider>();
                    }
                }
                AutoAddMeshColliderFunction(child);
            }
        }
    }
}
