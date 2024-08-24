using UnityEngine;
using System.Collections;
using static JsonInput;

namespace Invector
{
    public class vPickupItem : MonoBehaviour
    {
        AudioSource _audioSource;
        public AudioClip _audioClip;
        public GameObject _particle;
        private GameObject Door_Prefab_Melee;
        private GameObject Lock_Prefab_Melee;

        private GameObject Door_Prefab_Shooter;
        private GameObject Lock_Prefab_Shooter;
        private objectCollectableCount objectCollectableCount;

        [System.Serializable]
        public class CheckMission
        {
            public bool isForMission1;
            public bool isForMission2;
        }
        public CheckMission checkMission;

        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            objectCollectableCount = GetComponentInParent<objectCollectableCount>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !_audioSource.isPlaying)
            {
                Renderer[] renderers = GetComponentsInChildren<Renderer>();
                foreach (Renderer r in renderers)
                    r.enabled = false;

                _audioSource.PlayOneShot(_audioClip);
                Destroy(gameObject, _audioClip.length);
                if(checkMission.isForMission1)
                {
                    objectCollectableCount.IncrementeMission1();
                    if (objectCollectableCount.count == 11 || objectCollectableCount.count >= 11)
                        openDoorMelee();
                    if (objectCollectableCount.count == 17 || objectCollectableCount.count >= 17)
                        openDoorShooter();
                }
                else if (checkMission.isForMission2)
                {
                    objectCollectableCount.IncrementeMission2();
                }
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
