using UnityEngine;

public class Activation : MonoBehaviour
{
    private GameObject loadInput;
    // Start is called before the first frame update
    void Start()
    {
        loadInput = GameObject.Find("LoadInput");
        loadInput.GetComponent<LoadInput>().OnThirdPersonInputActivated();
    }
}
