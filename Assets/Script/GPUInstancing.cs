using UnityEngine;
using System.Collections.Generic;

public class GPUInstancing : MonoBehaviour
{
    public Mesh mesh;
    public Material material;
    public string keyword = "ObjectName"; // Le mot clé recherché dans les noms des GameObjects

    private Matrix4x4[] matrices;
    private MaterialPropertyBlock propertyBlock;

    void Start()
    {
        List<Matrix4x4> matrixList = new List<Matrix4x4>();

        // Récupérer tous les GameObjects enfants de l'objet auquel ce script est attaché
        CollectGameObjects(transform, matrixList);
        Debug.Log(matrixList.Count);

        matrices = matrixList.ToArray();
        propertyBlock = new MaterialPropertyBlock();
    }

    void Update()
    {
        if (matrices != null)
        {
            Graphics.DrawMeshInstanced(mesh, 0, material, matrices, matrices.Length, propertyBlock);
        }
    }

    void CollectGameObjects(Transform parent, List<Matrix4x4> matrixList)
    {
        foreach (Transform child in parent)
        {
            // Vérifier si le nom du GameObject contient le mot clé spécifié
            if (child.gameObject.name.StartsWith(keyword))
            {
                matrixList.Add(Matrix4x4.TRS(child.position, child.rotation, child.localScale));
            }

            // Appeler récursivement cette méthode pour les enfants de ce GameObject
            CollectGameObjects(child, matrixList);
        }
    }
}


