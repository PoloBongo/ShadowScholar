using UnityEngine;

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

        // Combiner les meshes
        CombineInstance[] combine = new CombineInstance[filters.Length];
        for (int i = 0; i < filters.Length; i++)
        {
            combine[i].mesh = filters[i].sharedMesh;
            combine[i].transform = filters[i].transform.localToWorldMatrix;
        }

        MeshFilter mf = gameObject.AddComponent<MeshFilter>();
        MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();

        Mesh combinedMesh = new Mesh();
        mf.mesh = combinedMesh;
        combinedMesh.CombineMeshes(combine, generateTriangleStrips);

        // Optionnel : supprimer les meshes originaux
        if (destroyOnDisable)
        {
            foreach (MeshFilter filter in filters)
            {
                Destroy(filter.gameObject);
            }
        }
    }
}
