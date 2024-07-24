using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CopyAndReplaceChildren : EditorWindow
{
    private GameObject sourceParent;  // Le parent source dont les enfants seront copiés
    private GameObject targetParent;  // Le parent cible où les nouveaux objets seront créés
    private GameObject newPrefab;     // Le prefab à utiliser pour les nouveaux objets

    [MenuItem("Tools/Copy and Replace Children")]
    public static void ShowWindow()
    {
        GetWindow<CopyAndReplaceChildren>("Copy and Replace Children");
    }

    private void OnGUI()
    {
        GUILayout.Label("Copy and Replace Children", EditorStyles.boldLabel);

        sourceParent = (GameObject)EditorGUILayout.ObjectField("Source Parent", sourceParent, typeof(GameObject), true);
        targetParent = (GameObject)EditorGUILayout.ObjectField("Target Parent", targetParent, typeof(GameObject), true);
        newPrefab = (GameObject)EditorGUILayout.ObjectField("New Prefab", newPrefab, typeof(GameObject), false);

        if (GUILayout.Button("Copy and Replace Children"))
        {
            CopyAndReplace();
        }
    }

    private void CopyAndReplace()
    {
        if (sourceParent == null || targetParent == null || newPrefab == null)
        {
            Debug.LogError("Source parent, target parent, and new prefab must be assigned.");
            return;
        }

        List<GameObject> newObjects = new List<GameObject>();

        // Instancier les nouveaux objets et stocker leurs références
        foreach (Transform child in sourceParent.transform)
        {
            // Créez une instance du nouveau prefab
            GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(newPrefab);

            // Parent le nouveau prefab au parent cible
            newObject.transform.SetParent(targetParent.transform);

            // Ajouter à la liste des nouveaux objets
            newObjects.Add(newObject);
        }

        // Appliquer les transformations après la boucle foreach
        for (int i = 0; i < sourceParent.transform.childCount; i++)
        {
            Transform child = sourceParent.transform.GetChild(i);
            GameObject newObject = newObjects[i];

            // Appliquez les informations de transformation
            newObject.transform.position = child.position;
            newObject.transform.rotation = child.rotation;
            newObject.transform.localScale = child.localScale;
        }

        // Supprimez l'ancien parent et tous ses enfants
        Undo.DestroyObjectImmediate(sourceParent);

        Debug.Log("Children copied and source parent deleted successfully.");
    }
}
