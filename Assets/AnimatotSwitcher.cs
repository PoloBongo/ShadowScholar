using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Invector.vCharacterController.vActions
{
    public class ColliderAction : vActionListener
    {
        public BoxCollider fixAnimatorCollider;
        public List<GameObject> targetObjects = new List<GameObject>();
        public List<string> newTags = new List<string>();
        public List<int> newLayers = new List<int>();

        public override void OnActionExit(Collider other)
        {
            if (fixAnimatorCollider != null)
            {
                fixAnimatorCollider.enabled = true;
            }
        }
    }

    [vClassHeader("Animator Switcher")]
    public class AnimatorSwitcher : MonoBehaviour
    {
        public GameObject playerObject;
        private ColliderAction actionListener = new ColliderAction();

        public virtual void EnterBottomLader(Collider other)
        {
            // Vérifiez si les listes sont de même longueur
            if (actionListener.targetObjects.Count != actionListener.newTags.Count || actionListener.targetObjects.Count != actionListener.newLayers.Count)
            {
                Debug.LogWarning("Les listes des GameObjects, tags et layers doivent avoir la même longueur.");
                return;
            }

            for (int i = 0; i < actionListener.targetObjects.Count; i++)
            {
                if (actionListener.targetObjects[i] != null)
                {
                    actionListener.targetObjects[i].tag = actionListener.newTags[i];
                    actionListener.targetObjects[i].layer = actionListener.newLayers[i];

                    if (actionListener.targetObjects[i].GetComponent<BoxCollider>() != null)
                    {
                        actionListener.targetObjects[i].GetComponent<BoxCollider>().enabled = false;
                    }
                }
                else
                {
                    Debug.LogWarning($"Le GameObject à l'index {i} est null.");
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Le joueur a quitté le trigger");
            }
            if (actionListener.fixAnimatorCollider != null && other.CompareTag("Player"))
            {
                actionListener.fixAnimatorCollider.enabled = false;

                Debug.Log("sortie");

                for (int i = 0; i < actionListener.targetObjects.Count; i++)
                {
                    if (actionListener.targetObjects[i] != null)
                    {
                        actionListener.targetObjects[i].tag = "Water";
                        actionListener.targetObjects[i].layer = 11;

                        if (actionListener.targetObjects[i].GetComponent<BoxCollider>() != null)
                        {
                            actionListener.targetObjects[i].GetComponent<BoxCollider>().enabled = false;
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Le GameObject à l'index {i} est null.");
                    }
                }
            }
        }
    }
}
