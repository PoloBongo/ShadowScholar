using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Binoculars : MonoBehaviour
{

    private vShooterMeleeInput vShooterMeleeInput;

    // Start is called before the first frame update
    void Start()
    {
        vShooterMeleeInput = GetComponent<vShooterMeleeInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Binoculars") > 0)
        {
            Debug.Log("Binoculars");
            vShooterMeleeInput.ChangeCameraStateWithLerp("Binoculars");
        }
        else
        {
            vShooterMeleeInput.ResetCameraState();
        }
    }
}
