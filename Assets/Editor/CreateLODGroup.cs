/*using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityMeshSimplifier;

public class CreateLODGroup : MonoBehaviour
{
    [MenuItem("GameObject/Create LOD Group", false, 10)]
    static void CreateLODGroupForSelectedObject()
    {
        // V�rifier s'il y a un objet s�lectionn�
        if (Selection.activeGameObject == null)
        {
            Debug.LogWarning("Aucun objet s�lectionn�. Veuillez s�lectionner un objet pour cr�er un LOD Group.");
            return;
        }

        GameObject selectedObject = Selection.activeGameObject;

        // V�rifier si l'objet a d�j� un LOD Group
        if (selectedObject.GetComponent<LODGroup>() != null)
        {
            Debug.LogWarning("L'objet s�lectionn� a d�j� un LOD Group.");
            return;
        }

        // Ajouter le composant LOD Group � l'objet s�lectionn�
        LODGroup lodGroup = selectedObject.AddComponent<LODGroup>();

        // R�cup�rer tous les MeshRenderers et SkinnedMeshRenderers enfants
        List<Renderer> renderersList = new List<Renderer>();
        renderersList.AddRange(selectedObject.GetComponentsInChildren<MeshRenderer>());
        renderersList.AddRange(selectedObject.GetComponentsInChildren<SkinnedMeshRenderer>());

        if (renderersList.Count == 0)
        {
            Debug.LogWarning("L'objet s�lectionn� ne contient pas de renderers.");
            return;
        }

        Bounds totalBounds = new Bounds(selectedObject.transform.position, Vector3.zero);
        foreach (Renderer renderer in renderersList)
        {
            totalBounds.Encapsulate(renderer.bounds);
        }
        float maxObjectSize = totalBounds.size.magnitude;

        // Calculer la taille de chaque renderer et les assigner � un LOD en fonction de leur taille
        float[] rendererSizes = new float[renderersList.Count];
        for (int i = 0; i < renderersList.Count; i++)
        {
            rendererSizes[i] = renderersList[i].bounds.size.magnitude;
        }

        // Trouver les tailles minimale et maximale
        float minSize = Mathf.Min(rendererSizes);
        float maxSize = Mathf.Max(rendererSizes);

        // D�finir les niveaux de LOD
        int lodCount = 3; // Nombre de niveaux de LOD
        LOD[] lods = new LOD[lodCount];

        // Initialiser les listes de renderers pour chaque niveau de LOD
        List<Renderer>[] lodRenderers = new List<Renderer>[lodCount];
        for (int i = 0; i < lodCount; i++)
        {
            lodRenderers[i] = new List<Renderer>();
        }

        // Assigner les renderers aux LODs en fonction de leur taille et de leur tag/nom
        for (int i = 0; i < renderersList.Count; i++)
        {
            float normalizedSize = (rendererSizes[i] - minSize) / (maxSize - minSize);
            GameObject rendererObject = renderersList[i].gameObject;

            // V�rifier si l'objet est dans un dossier "Exterieur" ou "Interior"
            bool isExteriorObj = IsInExteriorFolder(rendererObject.transform);
            bool isInteriorObj = IsInInteriorFolder(rendererObject.transform);

            if (isExteriorObj)
            {
                lodRenderers[0].Add(renderersList[i]);
                LODSimplifyMeshIteratively(1, 0.5f, renderersList[i], rendererObject, lodRenderers);
                LODSimplifyMeshIteratively(2, 0.2f, renderersList[i], rendererObject, lodRenderers);
            }
            else if (isInteriorObj)
            {
                // Ajouter uniquement au premier niveau de LOD
                lodRenderers[0].Add(renderersList[i]);
            }
            else
            {
                if (normalizedSize < 0.1f)
                {
                    lodRenderers[0].Add(renderersList[i]);
                }
                else if (normalizedSize < 0.3f)
                {
                    lodRenderers[0].Add(renderersList[i]);
                    LODSimplifyMeshIteratively(1, 0.5f, renderersList[i], rendererObject, lodRenderers);
                }
                else
                {
                    lodRenderers[0].Add(renderersList[i]);
                    LODSimplifyMeshIteratively(1, 0.5f, renderersList[i], rendererObject, lodRenderers);
                    LODSimplifyMeshIteratively(2, 0.2f, renderersList[i], rendererObject, lodRenderers);
                }
            }
        }

        // Cr�er les niveaux de LOD avec des pourcentages ajust�s dynamiquement
        float[] lodPercentages = { 0.5f, 0.25f, 0.1f }; // Ajuster les pourcentages pour LOD 0, LOD 1 et LOD 2
        float[] lodScreenRelativeHeights = new float[lodCount];
        for (int i = 0; i < lodCount; i++)
        {
            lodScreenRelativeHeights[i] = lodPercentages[i] / maxObjectSize;
        }

        for (int i = 0; i < lodCount; i++)
        {
            lods[i] = new LOD(lodScreenRelativeHeights[i], lodRenderers[i].ToArray());
        }

        // Appliquer les niveaux de LOD au LOD Group
        lodGroup.SetLODs(lods);
        lodGroup.RecalculateBounds();

        Debug.Log("LOD Group cr�� avec succ�s pour l'objet s�lectionn�.");

        // Fonction pour v�rifier si un objet est dans un dossier nomm� "Exterieur"
        bool IsInExteriorFolder(Transform transform)
        {
            while (transform != null)
            {
                if (transform.name.Contains("Exterior"))
                {
                    return true;
                }
                transform = transform.parent;
            }
            return false;
        }

        // Fonction pour v�rifier si un objet est dans un dossier nomm� "Interior"
        bool IsInInteriorFolder(Transform transform)
        {
            while (transform != null)
            {
                if (transform.name.Contains("Interior"))
                {
                    return true;
                }
                transform = transform.parent;
            }
            return false;
        }

        void LODSimplifyMeshIteratively(int lod, float quality, Renderer renderer, GameObject rendererObject, List<Renderer>[] lodRenderers)
        {
            MeshFilter meshFilter = rendererObject.GetComponent<MeshFilter>();

            if (meshFilter != null)
            {
                Mesh originalMesh = meshFilter.sharedMesh;
                Debug.Log("Original Mesh Triangle Count: " + originalMesh.triangles.Length / 3);

                // Assurez-vous que la qualit� n'est pas trop basse
                quality = Mathf.Clamp(quality, 0.1f, 1.0f);

                Mesh simplifiedMesh = SimplifyMeshIteratively(originalMesh, quality);
                Debug.Log("Simplified Mesh Triangle Count: " + simplifiedMesh.triangles.Length / 3);

                GameObject lodObject = new GameObject(rendererObject.name + "_LOD" + lod);
                lodObject.transform.SetParent(rendererObject.transform);

                lodObject.transform.localPosition = Vector3.zero;
                lodObject.transform.localRotation = Quaternion.identity;
                lodObject.transform.localScale = Vector3.one;

                MeshFilter lodMeshFilter = lodObject.AddComponent<MeshFilter>();
                MeshRenderer lodMeshRenderer = lodObject.AddComponent<MeshRenderer>();
                lodMeshRenderer.sharedMaterials = renderer.sharedMaterials;
                lodMeshFilter.sharedMesh = simplifiedMesh;

                lodRenderers[lod].Add(lodMeshRenderer);
            }
            else
            {
                lodRenderers[lod].Add(renderer);
            }
        }

        Mesh SimplifyMeshIteratively(Mesh originalMesh, float targetQuality)
        {
            MeshSimplifier meshSimplifier = new MeshSimplifier();
            meshSimplifier.Initialize(originalMesh);

            // Configurer les options de simplification
            SimplificationOptions options = new SimplificationOptions
            {
                PreserveBorderEdges = true,
                PreserveUVSeamEdges = false,
                PreserveUVFoldoverEdges = false,
                PreserveSurfaceCurvature = false,
                EnableSmartLink = true,
                VertexLinkDistance = 0.001f,
                MaxIterationCount = 50,  // R�duit le nombre d'it�rations
                Agressiveness = 2.0,     // R�duit l'agressivit�
                ManualUVComponentCount = false
            };

            meshSimplifier.SimplificationOptions = options;

            // Simplifier par �tapes
            float currentQuality = 1.0f;
            while (currentQuality > targetQuality)
            {
                float nextQuality = Mathf.Max(currentQuality - 0.05f, targetQuality);
                try
                {
                    meshSimplifier.SimplifyMesh(nextQuality);
                }
                catch (System.IndexOutOfRangeException ex)
                {
                    Debug.LogWarning("IndexOutOfRangeException caught during mesh simplification. Adjusting the quality step.");
                    break;
                }
                currentQuality = nextQuality;
            }

            Mesh simplifiedMesh = meshSimplifier.ToMesh();

            // Recalculer les donn�es du maillage
            simplifiedMesh.RecalculateBounds();
            simplifiedMesh.RecalculateNormals();
            simplifiedMesh.RecalculateTangents();

            return simplifiedMesh;
        }

    }
}
*/

