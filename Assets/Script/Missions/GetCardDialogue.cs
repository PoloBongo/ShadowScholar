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

    private KinematicStartOffice kinematicStartOffice;
    private JsonFile jsonFile;
    private Pause pause;

    void Start()
    {
        GameObject pauseGamobject = GameObject.Find("Pause");
        if (pauseGamobject != null)
            pause = pauseGamobject.GetComponent<Pause>();

        GameObject jsonFileObject = GameObject.Find("Save");
        if (jsonFileObject != null)
            jsonFile = jsonFileObject.GetComponent<JsonFile>();

        if (!jsonFile.shadowScholar.kinematicOffice.isFinish)
        {
            if (interactionScript != null)
            {
                interactionScript.OnInteract += StartDialogue;
            }

            dialogueCanvas.gameObject.SetActive(false);
            kinematicStartOffice = GetComponent<KinematicStartOffice>();
        }
        else
        {
            InteractionScript interactionScript = GetComponent<InteractionScript>();
            interactionScript.enabled = false;
        }
        /*player = GameObject.FindWithTag("Player");*/
    }

    private void CantRelanceDialogue()
    {
        InteractionScript interactionScript = GetComponent<InteractionScript>();
        interactionScript.enabled = false;
    }

    public void InitGetCardDialogue()
    {
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
            CantRelanceDialogue();
        }
    }

    void EndDialogue()
    {
        isInDialogue = false;
        dialogueCanvas.gameObject.SetActive(false);
        player.GetComponent<vItemManager>().AddItem(new ItemReference(28));
        player.GetComponent<vShooterMeleeInput>().SetLockAllInput(false);
        enabled = false;

        if (kinematicStartOffice != null)
        {
            pause.enabled = false;
            kinematicStartOffice.StartKinematicOffice();
        }
    }

}
