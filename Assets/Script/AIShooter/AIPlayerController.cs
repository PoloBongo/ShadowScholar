using Invector;
using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public enum WeaponType
{
    Pistol,
    Rifle,
    Shotgun
}

[System.Serializable]
public class WeaponProperties
{
    public GameObject prefabWeapon;
    public GameObject prefabBullet;
    public GameObject BulletPosition;
    public string clipTypeWalkName;
    public string clipIdleName;
    public string clipFireName;
    public string clipReloadName;
    public string clipAimingName;
    public float clipReloadTime;
    public int clipCapacity;
    public int clipMaxCapacity;
    public float frequencyFire;
    public float stopDistanceToFirePlayer;
    public List<ParticleSystem> particlesFire;
    public Light lightFire;
    public AudioSource audioSource;
}

public enum AIType
{
    Facile,
    Moyen,
    Difficile
}

public class AIPlayerController : MonoBehaviour
{
    [SerializeField] GameObject mainCharacter;
    private MissionManager missionManager;
    [SerializeField] bool aiAutoChooseWeapon;
    [System.Serializable]
    public class MultipleTargets
    {
        public List<Transform> targets = new List<Transform>();
    }
    public MultipleTargets multipleTargets;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private vThirdPersonController vThirdPersonController;
    private vHealthController vHealthController;
    private bool isWalking = true;
    private bool isIdle = false;
    private bool canFire = false;
    private bool isFiring = false;
    private bool isReloading = false;
    private bool isDead = false;
    private int randomChooseWeapon;

    [System.Serializable]
    public class SelectWeapon
    {
        public WeaponType selectedWeapon;
        public WeaponProperties pistolProperties = new WeaponProperties
        {
            clipFireName = "FirePistol",
            clipReloadName = "ReloadPistol",
            clipAimingName = "isAimingPistol",
            clipReloadTime = 2.25f,
            clipCapacity = 6,
            clipMaxCapacity = 6,
            particlesFire = new List<ParticleSystem>(),
            lightFire = new Light(),
            audioSource = new AudioSource(),
        };

        public WeaponProperties rifleProperties = new WeaponProperties
        {
            clipFireName = "FireRifle",
            clipReloadName = "ReloadRifle",
            clipAimingName = "isAimingRifle",
            clipReloadTime = 2.25f,
            clipCapacity = 15,
            clipMaxCapacity = 15,
            particlesFire = new List<ParticleSystem>(),
            lightFire = new Light(),
            audioSource = new AudioSource(),
        };

        public WeaponProperties shotgunProperties = new WeaponProperties
        {
            clipFireName = "FireShotgun",
            clipReloadName = "ReloadShotgun",
            clipAimingName = "isAimingShotgun",
            clipReloadTime = 2.25f,
            clipCapacity = 4,
            clipMaxCapacity = 4,
            particlesFire = new List<ParticleSystem>(),
            lightFire = new Light(),
            audioSource = new AudioSource(),
        };
    }
    [SerializeField] private SelectWeapon selectWeapon;
    [System.Serializable]
    public class IASelectedType
    {
        public AIType iaType;
        public float targetDistancePlayer;
    }
    [SerializeField] private IASelectedType selectedIAType;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] bool isStatic;
    [SerializeField] bool canPtrol;

    [SerializeField] private List<Transform> patrolPoints;
    private int currentPatrolIndex = 0;
    private bool inPatrol = false;

    private string setActiveWalkType;
    private string setActiveIdle;
    private Camera iaCamera;
    private bool inChasse;
    private string iaChoose;
    private float setTargetDistancePlayer;
    private float setStopDistanceToFirePlayer;
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        iaCamera = GetComponentInChildren<Camera>();
        vHealthController = GetComponent<vHealthController>();
        GameObject gameController = GameObject.Find("GameController");
        missionManager = gameController.GetComponent<MissionManager>();
        if (!aiAutoChooseWeapon)
        {
            switch (selectWeapon.selectedWeapon)
            {
                case WeaponType.Pistol:
                    setStopDistanceToFirePlayer = selectWeapon.pistolProperties.stopDistanceToFirePlayer;
                    setActiveWalkType = selectWeapon.pistolProperties.clipTypeWalkName;
                    setActiveIdle = selectWeapon.pistolProperties.clipIdleName;
                    selectWeapon.pistolProperties.prefabWeapon.SetActive(true);
                    break;
                case WeaponType.Rifle:
                    setStopDistanceToFirePlayer = selectWeapon.rifleProperties.stopDistanceToFirePlayer;
                    setActiveWalkType = selectWeapon.rifleProperties.clipTypeWalkName;
                    setActiveIdle = selectWeapon.rifleProperties.clipIdleName;
                    selectWeapon.rifleProperties.prefabWeapon.SetActive(true);
                    break;
                case WeaponType.Shotgun:
                    setStopDistanceToFirePlayer = selectWeapon.shotgunProperties.stopDistanceToFirePlayer;
                    setActiveWalkType = selectWeapon.shotgunProperties.clipTypeWalkName;
                    setActiveIdle = selectWeapon.shotgunProperties.clipIdleName;
                    selectWeapon.shotgunProperties.prefabWeapon.SetActive(true);
                    break;
            }
        }
        else
        {
            System.Random random = new System.Random();
            randomChooseWeapon = random.Next(0, 3);
            switch (randomChooseWeapon)
            {
                case 0:
                    setStopDistanceToFirePlayer = selectWeapon.pistolProperties.stopDistanceToFirePlayer;
                    setActiveWalkType = selectWeapon.pistolProperties.clipTypeWalkName;
                    setActiveIdle = selectWeapon.pistolProperties.clipIdleName;
                    selectWeapon.pistolProperties.prefabWeapon.SetActive(true);
                    break;
                case 1:
                    setStopDistanceToFirePlayer = selectWeapon.rifleProperties.stopDistanceToFirePlayer;
                    setActiveWalkType = selectWeapon.rifleProperties.clipTypeWalkName;
                    setActiveIdle = selectWeapon.rifleProperties.clipIdleName;
                    selectWeapon.rifleProperties.prefabWeapon.SetActive(true);
                    break;
                case 2:
                    setStopDistanceToFirePlayer = selectWeapon.shotgunProperties.stopDistanceToFirePlayer;
                    setActiveWalkType = selectWeapon.shotgunProperties.clipTypeWalkName;
                    setActiveIdle = selectWeapon.shotgunProperties.clipIdleName;
                    selectWeapon.shotgunProperties.prefabWeapon.SetActive(true);
                    break;
            }
        }

        switch (selectedIAType.iaType)
        {
            case AIType.Facile:
                iaChoose = "Facile";
                setTargetDistancePlayer = selectedIAType.targetDistancePlayer;
                break;
            case AIType.Moyen:
                iaChoose = "Moyen";
                setTargetDistancePlayer = selectedIAType.targetDistancePlayer;
                break;
            case AIType.Difficile:
                iaChoose = "Difficile";
                setTargetDistancePlayer = selectedIAType.targetDistancePlayer;
                break;
        }

        navMeshAgent.stoppingDistance = setStopDistanceToFirePlayer;
        animator.SetTrigger(setActiveIdle);
        inChasse = false;
        isStatic = false;
    }

    #region AssignTransform
    public void AssignPlayerTransforms(GameObject player)
    {
        mainCharacter = player;
        vThirdPersonController = player.GetComponent<vThirdPersonController>();

        if (player.transform != null)
        {
            AssignPlayerTransforms2(player.transform);
        }
        this.gameObject.SetActive(true);
    }

    void AssignPlayerTransforms2(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.name.Contains("CC_Base_Hip"))
            {
                multipleTargets.targets.Add(child);
            }

            if (child.name.Contains("CC_BaseThigh.L"))
            {
                multipleTargets.targets.Add(child);
            }

            if (child.name.Contains("CC_BaseThigh.R"))
            {
                multipleTargets.targets.Add(child);
            }

            if (child.name.Contains("CC_BaseCalf.L"))
            {
                multipleTargets.targets.Add(child);
            }

            if (child.name.Contains("CC_BaseCalf.R"))
            {
                multipleTargets.targets.Add(child);
            }

            if (child.name.Contains("CCC_Base_Spine01"))
            {
                multipleTargets.targets.Add(child);
            }

            if (child.name.Contains("CC_Base_Head"))
            {
                multipleTargets.targets.Add(child);
            }

            if (child.name.Contains("CC_BaseUpperarm.L"))
            {
                multipleTargets.targets.Add(child);
            }

            if (child.name.Contains("CC_BaseForearm.L"))
            {
                multipleTargets.targets.Add(child);
            }

            if (child.name.Contains("CC_BaseUpperarm.R"))
            {
                multipleTargets.targets.Add(child);
            }

            if (child.name.Contains("CC_BaseForearm.R"))
            {
                multipleTargets.targets.Add(child);
            }

            AssignPlayerTransforms2(child);
        }
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (iaChoose == "Difficile")
        {
            if (other.CompareTag("Ignore Ragdoll"))
            {
                inChasse = true;
            }
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Count == 0) return;
        if (!CanViewPlayer())
        {
            if (!inPatrol)
            {
                navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
                inPatrol = true;
                navMeshAgent.stoppingDistance = 1;
                animator.SetTrigger(setActiveWalkType);
            }

            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
                navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
            }
        }
    }

    private bool CanViewPlayer()
    {
        if (iaChoose == "Facile" || iaChoose == "Moyen")
        {
            if (iaCamera != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                if (player != null)
                {
                    float distanceToPlayer = Vector3.Distance(iaCamera.transform.position, player.transform.position);

                    if (distanceToPlayer <= setTargetDistancePlayer)
                    {
                        Vector3 directionToPlayer = (player.transform.position - iaCamera.transform.position).normalized;
                        float angleToPlayer = Vector3.Angle(iaCamera.transform.forward, directionToPlayer);

                        if (angleToPlayer < iaCamera.fieldOfView / 2)
                        {
                            Vector3 screenPoint = iaCamera.WorldToViewportPoint(player.transform.position);
                            bool isInView = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

                            if (isInView)
                            {
                                Ray ray = iaCamera.ScreenPointToRay(iaCamera.WorldToScreenPoint(player.transform.position));
                                RaycastHit hit;

                                if (Physics.Raycast(ray, out hit, Mathf.Infinity, obstacleMask))
                                {
                                    if (hit.transform.name == "HeadTrackSensor")
                                    {
                                        animator.SetTrigger(setActiveWalkType);
                                        navMeshAgent.stoppingDistance = setStopDistanceToFirePlayer;
                                        inChasse = true;
                                        return inChasse;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("player est null");
                }
            }
            else
            {
                Debug.Log("camera null");
            }

            inChasse = false;
            return inChasse;
        }

        inChasse = false;
        return inChasse;
    }

    void Update()
    {
        if (vHealthController.currentHealth < vHealthController.maxHealth && !inChasse)
        {
            animator.SetTrigger(setActiveWalkType);
            inChasse = true;
        }

        if (inChasse)
        {
            if (!isDead)
            {
                if (vThirdPersonController.currentHealth > 0)
                {
                    if (!isReloading)
                    {
                        if (!isStatic)
                        {
                            if (Vector3.Distance(this.gameObject.transform.position, mainCharacter.transform.position) < setStopDistanceToFirePlayer)
                            {
                                Vector3 direction = mainCharacter.transform.position - this.gameObject.transform.position;
                                direction.y = 0;
                                if (direction != Vector3.zero)
                                {
                                    Quaternion rotation = Quaternion.LookRotation(direction);
                                    this.gameObject.transform.rotation = rotation;
                                }

                                if (navMeshAgent.remainingDistance <= setStopDistanceToFirePlayer)
                                {
                                    if (!isIdle)
                                    {
                                        isIdle = true;
                                        isWalking = false;
                                        canFire = true;
                                    }
                                    if (canFire && !isFiring)
                                    {
                                        canFire = false;
                                        isFiring = true;
                                        if (!aiAutoChooseWeapon)
                                        {
                                            switch (selectWeapon.selectedWeapon)
                                            {
                                                case WeaponType.Pistol:
                                                    StartCoroutine(WaitForFire(selectWeapon.pistolProperties));
                                                    break;
                                                case WeaponType.Rifle:
                                                    StartCoroutine(WaitForFire(selectWeapon.rifleProperties));
                                                    break;
                                                case WeaponType.Shotgun:
                                                    StartCoroutine(WaitForFire(selectWeapon.shotgunProperties));
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            switch (randomChooseWeapon)
                                            {
                                                case 0:
                                                    StartCoroutine(WaitForFire(selectWeapon.pistolProperties));
                                                    break;
                                                case 1:
                                                    StartCoroutine(WaitForFire(selectWeapon.rifleProperties));
                                                    break;
                                                case 2:
                                                    StartCoroutine(WaitForFire(selectWeapon.shotgunProperties));
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!isWalking)
                                {
                                    animator.SetTrigger(setActiveWalkType);
                                    isWalking = true;
                                    isIdle = false;
                                }
                                navMeshAgent.SetDestination(mainCharacter.transform.position);
                            }
                        }
                        else
                        {
                            if (canFire && !isFiring)
                            {
                                canFire = false;
                                isFiring = true;
                                if (!aiAutoChooseWeapon)
                                {
                                    switch (selectWeapon.selectedWeapon)
                                    {
                                        case WeaponType.Pistol:
                                            StartCoroutine(WaitForFire(selectWeapon.pistolProperties));
                                            break;
                                        case WeaponType.Rifle:
                                            StartCoroutine(WaitForFire(selectWeapon.rifleProperties));
                                            break;
                                        case WeaponType.Shotgun:
                                            StartCoroutine(WaitForFire(selectWeapon.shotgunProperties));
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (randomChooseWeapon)
                                    {
                                        case 0:
                                            StartCoroutine(WaitForFire(selectWeapon.pistolProperties));
                                            break;
                                        case 1:
                                            StartCoroutine(WaitForFire(selectWeapon.rifleProperties));
                                            break;
                                        case 2:
                                            StartCoroutine(WaitForFire(selectWeapon.shotgunProperties));
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    missionManager.MissionStatus("Failed", 2);
                }
            }
        }
        else
        {
            if (canPtrol)
            {
                Patrol();
            }
            else
            {
                CanViewPlayer();
            }
        }
    }

    IEnumerator WaitForFire(WeaponProperties weaponProperties)
    {
        System.Random random = new System.Random();
        int randomTargetHit = random.Next(0, multipleTargets.targets.Count);
        if (weaponProperties.clipCapacity == 0)
        {
            isReloading = true;
            StartCoroutine(WaitForReload(weaponProperties));
            yield break;
        }
        else
        {
            if (weaponProperties.clipReloadName != "ReloadRifle")
            {
                animator.SetTrigger(weaponProperties.clipAimingName);
            }
            yield return new WaitForSeconds(weaponProperties.frequencyFire);

            if (Vector3.Distance(this.gameObject.transform.position, mainCharacter.transform.position) > setStopDistanceToFirePlayer)
            {
                animator.SetTrigger(setActiveWalkType);
                isWalking = true;
                isIdle = false;
                canFire = true;
                isFiring = false;
                yield break;
            }
            animator.SetTrigger(weaponProperties.clipFireName);
            for (int i = 0; i < weaponProperties.particlesFire.Count; i++)
            {
                weaponProperties.particlesFire[i].Play();
            }

            GameObject bullet = Instantiate(weaponProperties.prefabBullet, weaponProperties.BulletPosition.transform.position, Quaternion.identity);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            for (int i = 0; i < multipleTargets.targets.Count; i++)
            {
                if (randomTargetHit == i)
                {
                    bulletRb.velocity = (multipleTargets.targets[i].position - weaponProperties.BulletPosition.transform.position).normalized * 50f;
                }
            }

            weaponProperties.audioSource.Play();
            weaponProperties.lightFire.enabled = true;
            weaponProperties.clipCapacity--;
        }
        yield return new WaitForSeconds(.06f);
        canFire = true;
        isFiring = false;
        isReloading = false;
        weaponProperties.lightFire.enabled = false;
    }

    IEnumerator WaitForReload(WeaponProperties weaponProperties)
    {
        if (weaponProperties.clipCapacity == 0)
        {
            animator.SetTrigger(weaponProperties.clipReloadName);
            yield return new WaitForSeconds(weaponProperties.clipReloadTime);
            weaponProperties.clipCapacity = weaponProperties.clipMaxCapacity;
        }
        yield return new WaitForSeconds(.06f);
        canFire = true;
        isFiring = false;
        isReloading = false;
    }

    public void Dead()
    {
        isDead = true;
        StartCoroutine(OnDeadStart());
    }

    IEnumerator OnDeadStart()
    {
        yield return new WaitForSeconds(.3f);
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(3.5f);
        Destroy(this.gameObject);
    }
}
