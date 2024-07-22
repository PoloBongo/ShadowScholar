using UnityEngine;
using System.Collections.Generic;

public class AutomaticGPUInstancing : MonoBehaviour
{
    // Structure pour contenir les données de chaque instance de Mesh avec un matériau spécifique
    private struct MeshInstanceData
    {
        public Mesh mesh;
        public Material material;
        public List<Matrix4x4> matrices;
    }

    private List<MeshInstanceData> meshInstances = new List<MeshInstanceData>(); // Liste pour stocker les instances de Mesh avec différents matériaux
    private MaterialPropertyBlock propertyBlock;

    void Start()
    {
        propertyBlock = new MaterialPropertyBlock();

        // Récupérer tous les MeshFilters sous le GameObject actuel et ses enfants
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>(true);

        foreach (MeshFilter meshFilter in meshFilters)
        {
            Mesh mesh = meshFilter.sharedMesh;
            Renderer renderer = meshFilter.GetComponent<Renderer>();
            Material material = renderer.sharedMaterial;

            // Désactiver le Renderer de l'objet original pour éviter le rendu en double
            renderer.enabled = false;

            // Vérifier si cette combinaison Mesh-Matériau existe déjà dans la liste
            bool found = false;
            foreach (MeshInstanceData data in meshInstances)
            {
                if (data.mesh == mesh && data.material == material)
                {
                    data.matrices.Add(Matrix4x4.TRS(meshFilter.transform.position, meshFilter.transform.rotation, meshFilter.transform.localScale));
                    found = true;
                    break;
                }
            }

            // Si la combinaison Mesh-Matériau n'a pas été trouvée, l'ajouter à la liste
            if (!found)
            {
                MeshInstanceData newData = new MeshInstanceData
                {
                    mesh = mesh,
                    material = material,
                    matrices = new List<Matrix4x4>
                    {
                        Matrix4x4.TRS(meshFilter.transform.position, meshFilter.transform.rotation, meshFilter.transform.localScale)
                    }
                };
                meshInstances.Add(newData);
            }
        }
    }

    void Update()
    {
        // Dessiner les instances pour chaque Mesh-Matériau détecté
        foreach (MeshInstanceData data in meshInstances)
        {
            // Assurez-vous que le matériau a l'option GPU Instancing activée
            if (data.material.enableInstancing)
            {
                Graphics.DrawMeshInstanced(data.mesh, 0, data.material, data.matrices.ToArray(), data.matrices.Count, propertyBlock);
            }else
            {
                Debug.LogError($"{data.material.name} have not instancing ON !" );
            }
        }
    }
}
