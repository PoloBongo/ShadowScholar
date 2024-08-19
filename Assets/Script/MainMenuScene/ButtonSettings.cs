using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSettings : MonoBehaviour
{
    public GameObject objectToDesabled;
    public GameObject objectToEnabled;
    // Start is called before the first frame update
    void Start()
    {
        Button setting = GetComponent<Button>();
        setting.onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        Debug.Log("btn click");
        ObjectToDesabled();
        ObjectToEnabled();
    }

    private void ObjectToDesabled()
    {
        objectToDesabled.active = false;
    }

    private void ObjectToEnabled()
    {
        objectToEnabled.active = true;
    }
}
