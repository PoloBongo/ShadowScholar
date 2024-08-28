using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GeNa.Core.GeNaOneChildOfDecorator;

public class checkVagueIA : MonoBehaviour
{
    [SerializeField] List<GameObject> iaVagues = new List<GameObject>();
    private string actualVague;
    void Start()
    {
        actualVague = "IAVague1";
    }

    private int SearchIAInVague()
    {
        int count = 0;
        for (int i = 0; i < iaVagues.Count; i++)
        {
            if (iaVagues[i].name == actualVague)
            {
                foreach(Transform child in iaVagues[i].transform)
                {
                    count++;
                }
            }
        }
        Debug.Log(count);
        return count;
    }

    private void CheckAndSwitchVague()
    {
        int count = SearchIAInVague();
        if (count <= 0)
        {
            if (actualVague == "IAVague1")
            {
                actualVague = "IAVague2";
            }
        }
    }

    void Update()
    {
        CheckAndSwitchVague();
    }
}
