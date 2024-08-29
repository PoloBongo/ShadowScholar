using Invector.vCharacterController.AI;
using UnityEngine;

public class DestroyOnDead : MonoBehaviour
{
    private vSimpleMeleeAI_Controller controller;

    void Start()
    {
        controller = GetComponent<vSimpleMeleeAI_Controller>();
    }

    void Update()
    {
        if (controller != null)
        {
            if (controller.currentHealth <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
