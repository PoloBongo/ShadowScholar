using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityMeshSimplifier;

public class CreateLODGroup : MonoBehaviour
{
    [MenuItem("GameObject/Create LOD Group", false, 10)]
    static void CreateLODGroupForSelectedObject()
    {
        // Vérifier s'il y a un objet sélectionné
        if (Selection.activeGameObject == null)
        {
            Debug.LogWarning("Aucun objet sélectionné. Veuillez sélectionner un objet pour créer un LOD Group.");
            return;
        }

        GameObject selectedObject = Selection.activeGameObject;

        // Vérifier si l'objet a déjà un LOD Group
        if (selectedObject.GetComponent<LODGroup>() != null)
        {
            Debug.LogWarning("L'objet sélectionné a déjà un LOD Group.");
            return;
        }

        // Ajouter le composant LOD Group à l'objet sélectionné
        LODGroup lodGroup = selectedObject.AddComponent<LODGroup>();

        // Récupérer tous les MeshRenderers et SkinnedMeshRenderers enfants
        List<Renderer> renderersList = new List<Renderer>();
        renderersList.AddRange(selectedObject.GetComponentsInChildren<MeshRenderer>());
        renderersList.AddRange(selectedObject.GetComponentsInChildren<SkinnedMeshRenderer>());

        if (renderersList.Count == 0)
        {
            Debug.LogWarning("L'objet sélectionné ne contient pas de renderers.");
            return;
        }

        Bounds totalBounds = new Bounds(selectedObject.transform.position, Vector3.zero);
        foreach (Renderer renderer in renderersList)
        {
            totalBounds.Encapsulate(renderer.bounds);
        }
        float maxObjectSize = totalBounds.size.magnitude;

        // Calculer la taille de chaque renderer et les assigner à un LOD en fonction de leur taille
        float[] rendererSizes = new float[renderersList.Count];
        for (int i = 0; i < renderersList.Count; i++)
        {
            rendererSizes[i] = renderersList[i].bounds.size.magnitude;
        }

        // Trouver les tailles minimale et maximale
        float minSize = Mathf.Min(rendererSizes);
        float maxSize = Mathf.Max(rendererSizes);

        // Définir les niveaux de LOD
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
            if (renderersList[i].gameObject.GetComponent<LODGroup>() == null && !renderersList[i].gameObject.name.Contains("LOD"))
            {

                float normalizedSize = (rendererSizes[i] - minSize) / (maxSize - minSize);
                GameObject rendererObject = renderersList[i].gameObject;

                // Vérifier si l'objet est dans un dossier "Exterieur" ou "Interior"
                bool isExteriorObj = IsInExteriorFolder(rendererObject.transform);
                bool isInteriorObj = IsInInteriorFolder(rendererObject.transform);

                if (isExteriorObj)
                {
                    lodRenderers[0].Add(renderersList[i]);
                    lodRenderers[1].Add(renderersList[i]);
                    lodRenderers[2].Add(renderersList[i]);
                }
                else if (isInteriorObj)
                {
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
                        lodRenderers[1].Add(renderersList[i]);
                    }
                    else
                    {
                        lodRenderers[0].Add(renderersList[i]);
                        lodRenderers[1].Add(renderersList[i]);
                        lodRenderers[2].Add(renderersList[i]);
                    }
                }
            }
        }

        float[] lodPercentages = { 0.45f, 0.2f, 0.02f }; // Ajuster les pourcentages pour LOD 0, LOD 1 et LOD 2

        for (int i = 0; i < lodCount; i++)
        {
            lods[i] = new LOD(lodPercentages[i], lodRenderers[i].ToArray());
        }

        // Appliquer les niveaux de LOD au LOD Group
        lodGroup.SetLODs(lods);
        lodGroup.RecalculateBounds();

        Debug.Log("LOD Group créé avec succès pour l'objet sélectionné.");

        // Fonction pour vérifier si un objet est dans un dossier nommé "Exterieur"
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

        // Fonction pour vérifier si un objet est dans un dossier nommé "Interior"
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

    }
}


