using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsPlaceholderInput : MonoBehaviour
{
    [SerializeField] private GameObject mainCharacter;
    [SerializeField] private GameObject textJump;

    private InputManagerEntry inputManagerEntry;
    private vThirdPersonInput vThirdPersonInput;
    // Start is called before the first frame update
    void Start()
    {
        if (mainCharacter)
        {
            vThirdPersonInput = mainCharacter.GetComponent<vThirdPersonInput>();
            Debug.Log(vThirdPersonInput.horizontalInput.GetType());
        }

        if (textJump)
        {
            Debug.Log(Input.GetKey("a"));
            TMP_InputField inputJump = textJump.GetComponent<TMP_InputField>();
            inputJump.text = "Bongo na Bongo";
        }
    }
}
