using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadZone : MonoBehaviour
{
    private GameObject Other;
    private GameObject Zone_1;
    private GameObject Zone_2;
    private GameObject Zone_3;
    private GameObject Zone_4;
    private GameObject Zone_5;
    private GameObject Zone_6;
    private GameObject Zone_7;
    private GameObject Zone_8;
    private GameObject Zone_9;
    private GameObject Zone_10;
    private GameObject Zone_11;
    private GameObject Zone_12;
    private GameObject Zone_13;
    private GameObject Zone_14;
    private GameObject Zone_15;
    private GameObject Zone_16;
    private GameObject Zone_17;
    private GameObject Zone_18;
    private GameObject Zone_19;
    private GameObject Zone_20;
    private GameObject Zone_21;
    private GameObject Zone_22;
    private GameObject Zone_23;
    private GameObject Zone_24;
    private GameObject Zone_25;


    // Start is called before the first frame update
    void Start()
    {
        /*Other = Resources.Load<GameObject>("Prefabs Zone/Other");
        Zone_1 = Resources.Load<GameObject>("Prefabs Zone/Zone_1");
        Zone_2 = Resources.Load<GameObject>("Prefabs Zone/Zone_2");
        Zone_3 = Resources.Load<GameObject>("Prefabs Zone/Zone_3");
        Zone_4 = Resources.Load<GameObject>("Prefabs Zone/Zone_4");
        Zone_5 = Resources.Load<GameObject>("Prefabs Zone/Zone_5");
        Zone_6 = Resources.Load<GameObject>("Prefabs Zone/Zone_6");
        Zone_7 = Resources.Load<GameObject>("Prefabs Zone/Zone_7");
        Zone_8 = Resources.Load<GameObject>("Prefabs Zone/Zone_8");
        Zone_9 = Resources.Load<GameObject>("Prefabs Zone/Zone_9");
        Zone_10 = Resources.Load<GameObject>("Prefabs Zone/Zone_10");
        Zone_11 = Resources.Load<GameObject>("Prefabs Zone/Zone_11");
        Zone_12 = Resources.Load<GameObject>("Prefabs Zone/Zone_12");
        Zone_13 = Resources.Load<GameObject>("Prefabs Zone/Zone_13");
        Zone_14 = Resources.Load<GameObject>("Prefabs Zone/Zone_14");
        Zone_15 = Resources.Load<GameObject>("Prefabs Zone/Zone_15");
        Zone_16 = Resources.Load<GameObject>("Prefabs Zone/Zone_16");
        Zone_17 = Resources.Load<GameObject>("Prefabs Zone/Zone_17");
        Zone_18 = Resources.Load<GameObject>("Prefabs Zone/Zone_18");
        Zone_19 = Resources.Load<GameObject>("Prefabs Zone/Zone_19");
        Zone_20 = Resources.Load<GameObject>("Prefabs Zone/Zone_20");
        Zone_21 = Resources.Load<GameObject>("Prefabs Zone/Zone_21");
        Zone_22 = Resources.Load<GameObject>("Prefabs Zone/Zone_22");
        Zone_23 = Resources.Load<GameObject>("Prefabs Zone/Zone_23");
        Zone_24 = Resources.Load<GameObject>("Prefabs Zone/Zone_24");
        Zone_25 = Resources.Load<GameObject>("Prefabs Zone/Zone_25");*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) 
        {
            /* Addressables.LoadAssetAsync<GameObject>("Prefabs Zone/Zone_11/House_E_2").Completed += OnHouseLoaded;*/
            Instantiate(Zone_11);
        }
    }

    private void OnHouseLoaded(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            // Instancier le prefab chargé
            Instantiate(obj.Result);
            Debug.Log("Maison chargée et instanciée avec succès !");
        }
        else
        {
            Debug.LogWarning("Échec du chargement de la maison.");
        }
    }

    /* IEnumerator LoadAndInstantiateZoneAsync(string assetPath)
     {
         ResourceRequest loadRequest = Resources.LoadAsync<GameObject>(assetPath);

         // Pendant que le prefab se charge, tu peux continuer à jouer
         while (!loadRequest.isDone)
         {
             yield return null;
         }

         // Le prefab est maintenant chargé
         GameObject loadedPrefab = loadRequest.asset as GameObject;

         if (loadedPrefab != null)
         {
             Instantiate(loadedPrefab, Vector3.zero, Quaternion.identity);
         }
         else
         {
             Debug.LogError($"Failed to load asset at path: {assetPath}. Make sure the asset is in a Resources folder.");
         }

     }*/
}
