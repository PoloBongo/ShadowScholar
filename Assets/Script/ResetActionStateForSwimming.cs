using UnityEngine;
using Invector.vCharacterController;

public class ResetActionStateForSwimming : MonoBehaviour
{
    public GameObject playerObject;
    protected vThirdPersonInput tpInput;

    private void Start()
    {
        // Récupérer le vThirdPersonInput associé au playerObject
        if (playerObject != null)
        {
            tpInput = playerObject.GetComponent<vThirdPersonInput>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerObject.GetComponent<Animator>().SetInteger("ActionState", 0);
        }
    }
}
