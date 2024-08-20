using Invector.vCharacterController;
using System;
using TMPro;
using UnityEngine;
using Invector.vItemManager;

public class GetCardDialogue : MonoBehaviour
{
    public InteractionScript interactionScript;

    [Serializable]
    public class DialogueLine
    {
        public string talker; // Le nom ou l'identifiant du personnage qui parle
        public string text;   // Le texte de la ligne de dialogue
    }

    private GameObject player;

    public Canvas dialogueCanvas; // Référence au Canvas qui affiche le dialogue
    public GameObject talkerText; // Référence au Text UI qui affichera le texte du dialogue
    public GameObject dialogueText; // Référence au Text UI qui affichera le texte du dialogue
    public DialogueLine[] dialogueLines; // Tableau de lignes de dialogue
    private int currentLineIndex = 0;
    private bool isInDialogue = false;

    void Start()
    {
        if (interactionScript != null)
        {
            interactionScript.OnInteract += StartDialogue; 
        }

        dialogueCanvas.gameObject.SetActive(false);
        player = GameObject.FindWithTag("Player");
    }

    void OnDestroy()
    {
        if (interactionScript != null)
        {
            interactionScript.OnInteract -= StartDialogue;
        }
    }

    public void StartDialogue()
    {
        isInDialogue = true;
        currentLineIndex = 0;
        dialogueCanvas.gameObject.SetActive(true);
        interactionScript.enabled = false;
        player.GetComponent<vShooterMeleeInput>().SetLockAllInput(true);
        player.GetComponent<vThirdPersonController>().StopCharacter();
        ShowNextLine();
    }


    void Update()
    {
        if (isInDialogue && Input.GetKeyDown(KeyCode.Space)) // Appuyer sur Espace pour avancer dans le dialogue
        {
            ShowNextLine();
        }
    }

    void ShowNextLine()
    {
        if (currentLineIndex < dialogueLines.Length)
        {
            talkerText.GetComponent<TextMeshProUGUI>().text = dialogueLines[currentLineIndex].talker;  // Affiche le nom du talker
            dialogueText.GetComponent<TextMeshProUGUI>().text = dialogueLines[currentLineIndex].text;  // Affiche le texte du dialogue
            currentLineIndex++;
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        isInDialogue = false;
        dialogueCanvas.gameObject.SetActive(false);
        player.GetComponent<vItemManager>().AddItem(new ItemReference(28));
        player.GetComponent<vShooterMeleeInput>().SetLockAllInput(false);
        enabled = false;
    }

}
