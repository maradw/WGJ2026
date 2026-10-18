
using Game.Audio;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VisualNovelManager : MonoBehaviour
{
    [Header("Historia")]
    [SerializeField] private StoryData story;

    [Header("Interfaz")]
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image characterPortrait;

    [Header("Botones principales")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;

    [Header("Botones de decisiones")]
    [SerializeField] private List<Button> choiceButtons;

    // Recorrido que ya hizo el jugador.
    private List<DialogueNode> history = new List<DialogueNode>();
    private int currentIndex = 0;
    private int currentLineIndex = 0;
    // Registra la decisión tomada en cada nodo.
    private Dictionary<DialogueNode, DialogueChoice> savedChoices =
        new Dictionary<DialogueNode, DialogueChoice>();

    [SerializeField] private MiniGameManager miniGameManager;

    [SerializeField] GameObject DialogueSys;



    [SerializeField] MusicData Background;

    [Header("Interfaz visual")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image dialogueBoxImage;

    private void Start()
    {
       

        if (story == null || story.startNode == null)
        {
            Debug.LogWarning("No se ha asignado el inicio de la historia.");
            return;
        }

        history.Add(story.startNode);
        ShowNode();
    }
    private void OnEnable()
    {
        nextButton.onClick.AddListener(NextDialogue);
        backButton.onClick.AddListener(PreviousDialogue);
        miniGameManager.OnMinigameFinished += ContinueAfterMinigame;
    }
    private void OnDisable()
    {
        nextButton.onClick.RemoveListener(NextDialogue);
        backButton.onClick.RemoveListener(PreviousDialogue);
        miniGameManager.OnMinigameFinished -= ContinueAfterMinigame;
    }
    private void ContinueAfterMinigame()
    {
        DialogueNode currentNode = history[currentIndex];

        if (!savedChoices.TryGetValue(currentNode, out DialogueChoice choice))
        {
            Debug.LogWarning("No hay una decisión guardada para este nodo.");
            return;
        }

        if (choice.nextNode == null)
        {
            Debug.LogWarning("La decisión no tiene Next Node asignado.");
            return;
        }
        currentLineIndex = 0;
        history.Add(choice.nextNode);
        currentIndex++;

        ShowNode();
    }
    public void NextDialogue()
    {



        DialogueNode currentNode = history[currentIndex];



        if (currentLineIndex < currentNode.dialogueLines.Count - 1)
        {
            currentLineIndex++;
            ShowNode();
            return;
        }

        // Si ya avanzamos y retrocedimos, reproducir
        // el recorrido guardado, sin crear otra ruta.
        if (currentIndex < history.Count - 1)
        {
            currentIndex++;
            ShowNode();
            return;
        }

        DialogueNode targetNode = null;

        if (currentNode.choices.Count > 0)
        {
            // Solo podemos continuar por la opción guardada.
            if (!savedChoices.TryGetValue(currentNode, out DialogueChoice savedChoice))
                return;

            targetNode = savedChoice.nextNode;
        }
        else
        {
            targetNode = currentNode.nextNode;
        }

        if (targetNode == null)
            return;

        //////
        ///
        currentLineIndex = 0;

        history.Add(targetNode);
        currentIndex++;

        ShowNode();
    }

    public void PreviousDialogue()
    {
        if (currentLineIndex > 0)
        {
            currentLineIndex--;
            ShowNode();
            return;
        }

        if (currentIndex <= 0)
            return;

        currentIndex--;

        DialogueNode previousNode = history[currentIndex];

        currentLineIndex = previousNode.dialogueLines.Count > 0
            ? previousNode.dialogueLines.Count - 1
            : 0;

        ShowNode();
    }

    private void ChooseOption(int optionIndex)
    {
        DialogueNode currentNode = history[currentIndex];


       
        // Una decisión tomada no se puede reemplazar.
        if (savedChoices.ContainsKey(currentNode))
            return;

        if (optionIndex < 0 ||
            optionIndex >= currentNode.choices.Count)
            return;

        DialogueChoice choice = currentNode.choices[optionIndex];
        savedChoices[currentNode] = choice;
        if (choice.miniGameType == MiniGameType.Typing)
        {
            miniGameManager.OpenTypingGame();
            return;
        }

        if (choice.miniGameType == MiniGameType.Maze)
        {
            miniGameManager.OpenMazeGame();
            return;
        }
        if(choice.miniGameType == MiniGameType.Code)
        {
            miniGameManager.OpenCodeGame();
            return;
        }

        if (choice.nextNode == null)
        {
            Debug.LogWarning("La opcion no tiene un nodo de destino.");
            return;
        }

        //  savedChoices.Add(currentNode, choice);
        currentLineIndex = 0;
        // Avanzamos inmediatamente por la ruta elegida.
        history.Add(choice.nextNode);
        currentIndex++;

        ShowNode();
    }

    private void ShowNode()
    {
        DialogueNode node = history[currentIndex];

        characterNameText.text = node.characterName;
        characterPortrait.sprite = node.characterPortrait;
        characterPortrait.enabled = node.characterPortrait != null;
        if (node.backgroundImage != null)
        {
            backgroundImage.sprite = node.backgroundImage;
        }

        if (node.dialogueBoxImage != null)
        {
            dialogueBoxImage.sprite = node.dialogueBoxImage;
        }
        // Display the current dialogue line
        if (node.dialogueLines.Count > 0)
        {
            currentLineIndex = Mathf.Clamp(
                currentLineIndex, 0, node.dialogueLines.Count - 1);

            dialogueText.text = node.dialogueLines[currentLineIndex];
        }
        else
        {
            dialogueText.text = node.dialogueText;
        }
        dialogueText.color = node.dialogueTextColor;

        backButton.interactable =
            currentIndex > 0 || currentLineIndex > 0;

        for (int i = 0; i < choiceButtons.Count; i++)
            choiceButtons[i].gameObject.SetActive(false);

        bool hasChoices = node.choices.Count > 0;
        bool choiceAlreadyMade = savedChoices.ContainsKey(node);

        if (hasChoices && !choiceAlreadyMade)
        {
            for (int i = 0; i < node.choices.Count; i++)
            {
                if (i >= choiceButtons.Count)
                    break;

                Button button = choiceButtons[i];
                DialogueChoice choice = node.choices[i];

                button.gameObject.SetActive(true);

                TMP_Text buttonText =
                    button.GetComponentInChildren<TMP_Text>();

                if (buttonText != null)
                    buttonText.text = choice.choiceText;

                button.onClick.RemoveAllListeners();

                int optionIndex = i;
                button.onClick.AddListener(
                    () => ChooseOption(optionIndex));
            }
        }

        bool hasMoreLines =
            node.dialogueLines.Count > 0 &&
            currentLineIndex < node.dialogueLines.Count - 1;

        if (hasChoices)
        {
            nextButton.interactable =
                !hasMoreLines &&
                choiceAlreadyMade &&
                GetSavedTarget(node) != null;
        }
        else
        {
            nextButton.interactable =
                hasMoreLines || node.nextNode != null;
        }
    }

    private DialogueNode GetSavedTarget(DialogueNode node)
    {
        if (savedChoices.TryGetValue(node, out DialogueChoice choice))
            return choice.nextNode;

        return null;
    }
}
