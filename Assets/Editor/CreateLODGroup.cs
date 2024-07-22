using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

        // R�cup�rer tous les renderers enfants
        Renderer[] renderers = selectedObject.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            Debug.LogWarning("L'objet s�lectionn� ne contient pas de renderers.");
            return;
        }

        // Calculer la taille de chaque renderer et les assigner � un LOD en fonction de leur taille
        float[] rendererSizes = new float[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            rendererSizes[i] = renderers[i].bounds.size.magnitude;
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

        // Assigner les renderers aux LODs en fonction de leur taille et de leur tag/nom
        for (int i = 0; i < renderers.Length; i++)
        {
            float normalizedSize = (rendererSizes[i] - minSize) / (maxSize - minSize);
            GameObject rendererObject = renderers[i].gameObject;
            Debug.Log(selectedObject.name);
            Debug.Log(normalizedSize);

            // V�rifier si l'objet est dans un dossier "Exterieur"
            bool isExteriorObj = IsInExteriorFolder(rendererObject.transform);
            bool isInteriorObj = IsInInteriorFolder(rendererObject.transform);

            if (isExteriorObj)
            {
                lodRenderers[0].Add(renderers[i]); 
                lodRenderers[1].Add(renderers[i]); 
                lodRenderers[2].Add(renderers[i]); 
            }
            else if (isInteriorObj)
            {
                lodRenderers[0].Add(renderers[i]);
            }
            else
            {
                if (normalizedSize < 0.1f)
                {
                    lodRenderers[0].Add(renderers[i]); 
                }
                else if (normalizedSize < 0.3f)
                {
                    lodRenderers[0].Add(renderers[i]);
                    lodRenderers[1].Add(renderers[i]); 
                }
                else
                {
                    lodRenderers[0].Add(renderers[i]);
                    lodRenderers[1].Add(renderers[i]);
                    lodRenderers[2].Add(renderers[i]); 
                }
            }
        }

        // Cr�er les niveaux de LOD
        float[] lodPercentages = { 0.6f, 0.3f, 0.1f }; // Ajuster les pourcentages pour LOD 0, LOD 1 et LOD 2
        for (int i = 0; i < lodCount; i++)
        {
            lods[i] = new LOD(lodPercentages[i], lodRenderers[i].ToArray());
        }

        // Appliquer les niveaux de LOD au LOD Group
        lodGroup.SetLODs(lods);
        lodGroup.RecalculateBounds();

        Debug.Log("LOD Group cr�� avec succ�s pour l'objet s�lectionn�.");
    }
}