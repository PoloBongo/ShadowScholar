using UnityEngine;
using System.Collections.Generic;

public class AutomaticGPUInstancing : MonoBehaviour
{
    // Structure pour contenir les donn�es de chaque instance de Mesh avec un mat�riau sp�cifique
    private struct MeshInstanceData
    {
        public Mesh mesh;
        public Material material;
        public List<Matrix4x4> matrices;
    }

    private List<MeshInstanceData> meshInstances = new List<MeshInstanceData>(); // Liste pour stocker les instances de Mesh avec diff�rents mat�riaux
    private MaterialPropertyBlock propertyBlock;

    void Start()
    {
        propertyBlock = new MaterialPropertyBlock();

        // R�cup�rer tous les MeshFilters sous le GameObject actuel et ses enfants
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>(true);

        foreach (MeshFilter meshFilter in meshFilters)
        {
            Mesh mesh = meshFilter.sharedMesh;
            Renderer renderer = meshFilter.GetComponent<Renderer>();
            Material material = renderer.sharedMaterial;

            // D�sactiver le Renderer de l'objet original pour �viter le rendu en double
            renderer.enabled = false;

            // V�rifier si cette combinaison Mesh-Mat�riau existe d�j� dans la liste
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

            // Si la combinaison Mesh-Mat�riau n'a pas �t� trouv�e, l'ajouter � la liste
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
        // Dessiner les instances pour chaque Mesh-Mat�riau d�tect�
        foreach (MeshInstanceData data in meshInstances)
        {
            // Assurez-vous que le mat�riau a l'option GPU Instancing activ�e
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
