using UnityEngine;
using System.Collections.Generic;

public class CombineChildrenPlus : MonoBehaviour
{
    public bool generateTriangleStrips = true;
    public bool destroyOnDisable = false;
    public bool combineAtStart = true;

    private void Start()
    {
        if (combineAtStart)
        {
            Combine();
        }
    }

    public void Combine()
    {
        // Trouver tous les MeshFilters dans les enfants
        MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();
        if (filters.Length == 0)
            return;

        // Dictionnaire pour stocker les meshes par matériau
        Dictionary<Material, List<CombineInstance>> materialToCombineInstances = new Dictionary<Material, List<CombineInstance>>();

        // Collecter les CombineInstances
        foreach (MeshFilter filter in filters)
        {
            MeshRenderer renderer = filter.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                Material[] materials = renderer.sharedMaterials;
                for (int i = 0; i < materials.Length; i++)
                {
                    Material material = materials[i];
                    if (!materialToCombineInstances.ContainsKey(material))
                    {
                        materialToCombineInstances[material] = new List<CombineInstance>();
                    }

                    Mesh mesh = filter.sharedMesh;

                    // Vérifier la lisibilité du mesh
                    if (mesh != null && mesh.isReadable)
                    {
                        CombineInstance combineInstance = new CombineInstance
                        {
                            mesh = mesh,
                            transform = filter.transform.localToWorldMatrix
                        };
                        materialToCombineInstances[material].Add(combineInstance);
                    }
                }
            }
        }

        // Créer un GameObject pour chaque matériau
        foreach (var kvp in materialToCombineInstances)
        {
            Material material = kvp.Key;
            List<CombineInstance> combineInstances = kvp.Value;

            GameObject combinedObject = new GameObject("Combined Mesh - " + material.name);
            combinedObject.transform.parent = transform;
            combinedObject.transform.localPosition = Vector3.zero;
            combinedObject.transform.localRotation = Quaternion.identity;
            combinedObject.transform.localScale = Vector3.one;

            MeshFilter combinedMeshFilter = combinedObject.AddComponent<MeshFilter>();
            MeshRenderer combinedMeshRenderer = combinedObject.AddComponent<MeshRenderer>();

            Mesh combinedMesh = new Mesh();
            combinedMeshFilter.mesh = combinedMesh;
            combinedMesh.CombineMeshes(combineInstances.ToArray(), generateTriangleStrips);

            combinedMeshRenderer.material = material;
        }

        // Optionnel : supprimer les objets d'origine
        if (destroyOnDisable)
        {
            foreach (MeshFilter filter in filters)
            {
                Destroy(filter.gameObject);
            }
        }
    }
}
