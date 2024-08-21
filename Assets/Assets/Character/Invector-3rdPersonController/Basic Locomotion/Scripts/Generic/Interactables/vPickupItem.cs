using UnityEngine;
using System.Collections;

namespace Invector
{
    public class vPickupItem : MonoBehaviour
    {
        AudioSource _audioSource;
        public AudioClip _audioClip;
        public GameObject _particle;
        private int countItemPickup;
        private GameObject Door_Prefab_Melee;
        private GameObject Lock_Prefab_Melee;

        private GameObject Door_Prefab_Shooter;
        private GameObject Lock_Prefab_Shooter;

        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            countItemPickup = 0;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !_audioSource.isPlaying)
            {
                Renderer[] renderers = GetComponentsInChildren<Renderer>();
                foreach (Renderer r in renderers)
                    r.enabled = false;

                _audioSource.PlayOneShot(_audioClip);
                countItemPickup++;
                if (countItemPickup == 1 || countItemPickup >= 1)
                    openDoorMelee();
                if (countItemPickup == 0 || countItemPickup >= 0)
                    openDoorShooter();
                Destroy(gameObject, _audioClip.length);
            }
        }

        private void openDoorMelee()
        {
            Door_Prefab_Melee = GameObject.Find("Door_Prefab_Melee");
            Lock_Prefab_Melee = GameObject.Find("lock_melee");
            if (Door_Prefab_Melee != null)
            {
                Destroy(Lock_Prefab_Melee);
                Door_Prefab_Melee.GetComponent<vSimpleDoor>().autoOpen = true;
            }
        }

        private void openDoorShooter()
        {
            Door_Prefab_Shooter = GameObject.Find("Door_Prefab_Shooter");
            Lock_Prefab_Shooter = GameObject.Find("lock_shooter");
            if (Door_Prefab_Melee != null)
            {
                Destroy(Lock_Prefab_Shooter);
                Door_Prefab_Shooter.GetComponent<vSimpleDoor>().autoOpen = true;
            }
        }
    }
}
