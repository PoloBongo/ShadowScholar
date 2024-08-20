using UnityEngine;
using UnityEditor;

public class LODCheckerEditor : EditorWindow
{
    public float proximityThreshold = 0.0001f; // Seuil de proximité (arrondi au dixième de millième)
    private GameObject selectedObject;

    [MenuItem("Tools/LOD Checker")]
    public static void ShowWindow()
    {
        GetWindow<LODCheckerEditor>("LOD Checker");
    }

    void OnGUI()
    {
        GUILayout.Label("LOD Checker", EditorStyles.boldLabel);

        selectedObject = (GameObject)EditorGUILayout.ObjectField("Objet Sélectionné", selectedObject, typeof(GameObject), true);
        proximityThreshold = EditorGUILayout.FloatField("Seuil de Proximité", proximityThreshold);

        if (GUILayout.Button("Vérifier les LODs"))
        {
            CheckLODs();
        }
    }

    void CheckLODs()
    {
        if (selectedObject == null)
        {
            Debug.LogError("Aucun objet sélectionné.");
            return;
        }

        LODGroup lodGroup = selectedObject.GetComponent<LODGroup>();
        if (lodGroup == null)
        {
            Debug.LogError("Aucun LODGroup trouvé sur l'objet sélectionné.");
            return;
        }

        LOD[] lods = lodGroup.GetLODs();
        for (int i = 0; i < lods.Length; i++)
        {
            LOD lod = lods[i];
            for (int j = 0; j < lod.renderers.Length; j++)
            {
                Renderer renderer = lod.renderers[j];
                GameObject rendererObject = renderer.gameObject;

                if (rendererObject.name.Contains("_static")) // Vérifier si le nom de l'objet contient "_static"
                {
                    MeshFilter meshFilter = renderer.GetComponent<MeshFilter>();
                    if (meshFilter != null)
                    {
                        Mesh mesh = meshFilter.sharedMesh;
                        if (mesh != null && (HasIsolatedTriangle(mesh) || IsSingleTriangle(mesh)))
                        {
                            Debug.Log("Un renderer affiche un triangle isolé ou unique, remplacement du mesh en cours.");

                            // Extraire le nom d'origine de l'objet
                            string originalObjectName = ExtractOriginalObjectName(rendererObject.name);
                            if (!string.IsNullOrEmpty(originalObjectName))
                            {
                                GameObject closestObject = FindClosestObjectInHierarchyByName(originalObjectName, selectedObject.transform);

                                if (closestObject != null)
                                {
                                    MeshFilter closestMeshFilter = closestObject.GetComponent<MeshFilter>();
                                    if (closestMeshFilter != null)
                                    {
                                        Mesh closestMesh = closestMeshFilter.sharedMesh;
                                        if (closestMesh != null)
                                        {
                                            // Remplacer le mesh du renderer
                                            meshFilter.sharedMesh = closestMesh;
                                            Debug.Log("Mesh remplacé avec succès.");
                                        }
                                        else
                                        {
                                            Debug.LogWarning($"L'objet d'origine '{originalObjectName}' n'a pas de Mesh.");
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogWarning($"L'objet d'origine '{originalObjectName}' n'a pas de MeshFilter.");
                                    }
                                }
                                else
                                {
                                    Debug.LogWarning($"Objet d'origine '{originalObjectName}' non trouvé parmi les enfants.");
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    bool HasIsolatedTriangle(Mesh mesh)
    {
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;

        if (triangles.Length <= 12) // 12 indices correspondent à 4 triangles
        {
            Vector3[] normals = new Vector3[triangles.Length / 3];
            for (int i = 0; i < triangles.Length; i += 3)
            {
                Vector3 v0 = vertices[triangles[i]];
                Vector3 v1 = vertices[triangles[i + 1]];
                Vector3 v2 = vertices[triangles[i + 2]];
                normals[i / 3] = Vector3.Cross(v1 - v0, v2 - v0).normalized;
            }

            for (int i = 0; i < normals.Length; i++)
            {
                bool isolated = true;
                for (int j = 0; j < normals.Length; j++)
                {
                    if (i != j && Vector3.Dot(normals[i], normals[j]) > 0.5f)
                    {
                        isolated = false;
                        break;
                    }
                }

                if (isolated)
                {
                    return true; // Il y a un triangle isolé
                }
            }
        }

        return false;
    }

    bool IsSingleTriangle(Mesh mesh)
    {
        return mesh.triangles.Length == 3;
    }

    string ExtractOriginalObjectName(string staticObjectName)
    {
        // On suppose que le format du nom est "(nombre)_static_(nom du game object d'origine)"
        string[] parts = staticObjectName.Split(new string[] { "_static_" }, System.StringSplitOptions.None);
        if (parts.Length > 1)
        {
            return parts[1]; // Le nom d'origine est la deuxième partie
        }
        return null;
    }

    GameObject FindClosestObjectInHierarchyByName(string objectName, Transform rootTransform)
    {
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        // Rechercher récursivement dans tous les enfants de l'objet racine
        foreach (Transform child in rootTransform.GetComponentsInChildren<Transform>(true))
        {
            if (child.gameObject.name == objectName)
            {
                // Utiliser la position mondiale pour comparer
                float distance = Vector3.Distance(child.position, rootTransform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = child.gameObject;
                }
            }
        }

        return closestObject;
    }
}
