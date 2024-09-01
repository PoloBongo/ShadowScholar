using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Binoculars : MonoBehaviour
{

    private vShooterMeleeInput vShooterMeleeInput;
    private Camera playerCamera;
    private float fieldOfViewAngle;
    public float detectionRange = 400f;
    public LayerMask enemyLayer;

    private List<GameObject> enemyList = new List<GameObject>();

    public ObjectifMission objectifMission;

   // Start is called before the first frame update
    void Start()
    {
        GameObject IA = GameObject.Find("IA");
        foreach (Transform obj in IA.transform)
        {
            foreach (Transform obj2 in obj.transform)
            {
                if (obj2.name.Contains("InsurgentM4") || obj2.name.Contains("BossM4"))
                {
                    enemyList.Add(obj2.gameObject);
                }
            }
        }

        Debug.Log(enemyList.Count);
    }

    public void InitBinoculars()
    {
        vShooterMeleeInput = GameObject.FindGameObjectWithTag("Player").GetComponent<vShooterMeleeInput>();
        playerCamera = vShooterMeleeInput.cameraMain;
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Binoculars") > 0)
        {
            vShooterMeleeInput.ChangeCameraStateWithLerp("Binoculars");
            DetectVisibleEnemies();
        }
        else
        {
            vShooterMeleeInput.ResetCameraState();
        }
    }

    void DetectVisibleEnemies()
    {
        fieldOfViewAngle = playerCamera.fieldOfView;

        if(enemyList.Count == 0)
        {
            return;
        }

        List<GameObject> enemyListTemp = new List<GameObject>(enemyList);

        foreach (GameObject enemy in enemyListTemp)
        {
            Vector3 directionToEnemy = (enemy.transform.position - playerCamera.transform.position).normalized;

            float angleToEnemy = Vector3.Angle(playerCamera.transform.forward, directionToEnemy);

            if (angleToEnemy < fieldOfViewAngle / 2f)
            {
                if (IsEnemyVisible(enemy))
                {
                    enemyList.Remove(enemy);
                    if (enemy.name.Contains("InsurgentM4"))
                    {
                        objectifMission.UpdateObjectif(0, 1);
                    }
                    else if(enemy.name.Contains("BossM4"))
                    {
                        objectifMission.UpdateObjectif(1, 1);
                    }
                }
            }
        }
    }

    bool IsEnemyVisible(GameObject enemy)
    {
        Vector3[] pointsToCheck = new Vector3[]
        {
            enemy.transform.position + Vector3.up * 1.8f,  // Tête
            enemy.transform.position + Vector3.up * 1.0f,  // Torse
            enemy.transform.position + Vector3.up * 0.5f,  // Taille
            enemy.transform.position + Vector3.left * 0.5f,  // Côté gauche
            enemy.transform.position + Vector3.right * 0.5f  // Côté droit
        };

        foreach (var point in pointsToCheck)
        {
            Vector3 directionToPoint = (point - playerCamera.transform.position).normalized;
            Ray ray = new Ray(playerCamera.transform.position, directionToPoint);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, detectionRange))
            {
                // Si le Raycast frappe l'ennemi, il est visible
                if (hit.collider.gameObject == enemy)
                {
                    return true;
                }
            }
        }

        return true;
    }
}
