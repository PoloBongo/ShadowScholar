using Invector.vCamera;
using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    [SerializeField] private GameObject sleepText;
    [SerializeField] private Image Fade;
    [SerializeField] private Transform finalPosition;
    [SerializeField] private Transform finalPositionReveil;
    private bool inZone;
    private vThirdPersonController playerController;
    private Transform resetRotationPlayer;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;
    [SerializeField] private Horloge horloge;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = true;
            sleepText.GetComponentInChildren<Text>().text = "Appuyez sur [E] pour dormir.";
            sleepText.SetActive(true);
            GetPlayerComponent(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = false;
            sleepText.SetActive(false);
        }
    }

    private void GetPlayerComponent(GameObject player)
    {
        playerController = player.GetComponent<vThirdPersonController>();
        resetRotationPlayer = player.GetComponent<Transform>();
        playerRigidbody = player.GetComponent<Rigidbody>();
        playerAnimator = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inZone && playerController != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (horloge.percentageComplete > 0f && horloge.percentageComplete < 0.37f || horloge.percentageComplete > 0.826f && horloge.percentageComplete < 1f)
                {
                    sleepText.GetComponentInChildren<Text>().text = "Impossible de ce coucher si tôt !";
                }
                else
                {
                    playerController.transform.LookAt(finalPosition);
                    StartCoroutine(MovePlayerToPosition());
                }
            }
        }
    }

    private IEnumerator MovePlayerToPosition()
    {
        while (Vector3.Distance(playerController.transform.position, finalPosition.position) > 0.5f)
        {
            playerController.MoveToPosition(finalPosition.position);
            playerController.RotateToDirection(finalPosition.position - playerController.transform.position);
            yield return null;
        }

        playerRigidbody.constraints = RigidbodyConstraints.FreezePosition;
        resetRotationPlayer.rotation = Quaternion.Euler(0, 0, 0);
        resetRotationPlayer.position = finalPosition.position;

        yield return new WaitForSeconds(0.5f);

        playerAnimator.Play("Sleep", 0, 0f);
        Fade.GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeIn(Fade, 2f));
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(FadeOut(Fade, 2f));

        playerAnimator.Play("GetUpFromBelly", 0, 0f);
        playerAnimator.Play("Pick_Mid", 0, 0f);
        yield return new WaitForSeconds(2f);
        playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        while (Vector3.Distance(playerController.transform.position, finalPositionReveil.position) > 0.5f)
        {
            playerController.MoveToPosition(finalPositionReveil.position);
            playerController.RotateToDirection(finalPositionReveil.position - playerController.transform.position);
            yield return null;
        }

        resetRotationPlayer.position = finalPositionReveil.position;
        horloge.SetTimeManually(0.825f);
    }

    private IEnumerator FadeIn(Image image, float duration)
    {
        Color startColor = image.color;
        float startAlpha = startColor.a;
        float endAlpha = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            image.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
            yield return null;
        }

        // Assurez-vous que l'alpha est complètement à 1 à la fin du fade-in
        image.color = new Color(startColor.r, startColor.g, startColor.b, endAlpha);
    }

    private IEnumerator FadeOut(Image image, float duration)
    {
        Color startColor = image.color;
        float startAlpha = startColor.a;
        float endAlpha = 0f;
        float elapsedTime = 1f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            image.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
            yield return null;
        }

        // Assurez-vous que l'alpha est complètement à 1 à la fin du fade-in
        image.color = new Color(startColor.r, startColor.g, startColor.b, endAlpha);
    }
}
