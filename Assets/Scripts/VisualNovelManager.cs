
using Game.Audio;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VisualNovelManager : MonoBehaviour
{
    [Header("Story")]
    [SerializeField] private StoryData story;

    [Header("Interface")]
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text storyTimeText;
    [SerializeField] private Image characterPortrait;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image dialogueBoxImage;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;
    [SerializeField] private List<Button> choiceButtons;

    [Header("Minigames")]
    [SerializeField] private MiniGameManager miniGameManager;
    [SerializeField] private GameObject DialogueSys;

    [Header("Audio")]
    [SerializeField] private MusicData Background;

    private List<DialogueNode> history = new List<DialogueNode>();
    private int currentIndex;
    private int currentLineIndex;

    private Dictionary<DialogueNode, DialogueChoice> savedChoices =
        new Dictionary<DialogueNode, DialogueChoice>();


    private void Start()
    {
        if (story == null || story.startNode == null)
        {
            Debug.LogError("Story or Start Node is missing.");
            return;
        }

        history.Add(story.startNode);
        ShowNode();
    }

    private void OnEnable()
    {
        if (nextButton != null)
            nextButton.onClick.AddListener(NextDialogue);

        if (backButton != null)
            backButton.onClick.AddListener(PreviousDialogue);

        if (miniGameManager != null)
            miniGameManager.OnMinigameFinished += ContinueAfterMinigame;
    }

    private void OnDisable()
    {
        if (nextButton != null)
            nextButton.onClick.RemoveListener(NextDialogue);

        if (backButton != null)
            backButton.onClick.RemoveListener(PreviousDialogue);

        if (miniGameManager != null)
            miniGameManager.OnMinigameFinished -= ContinueAfterMinigame;
    }

    private void ContinueAfterMinigame()
    {
        if (currentIndex < 0 || currentIndex >= history.Count)
            return;

        DialogueNode currentNode = history[currentIndex];

        if (!savedChoices.TryGetValue(currentNode, out DialogueChoice choice))
        {
            Debug.LogWarning("No saved choice was found after the minigame.");
            return;
        }

        if (choice.nextNode == null)
        {
            Debug.LogWarning("The selected choice has no Next Node assigned.");
            return;
        }

        history.Add(choice.nextNode);
        currentIndex = history.Count - 1;
        currentLineIndex = 0;

        ShowNode();
    }

    public void NextDialogue()
    {
        if (history.Count == 0)
            return;

        DialogueNode currentNode = history[currentIndex];


        if (currentLineIndex < currentNode.dialogueLines.Count - 1)
        {
            currentLineIndex++;
            ShowNode();
            return;
        }


        if (currentIndex < history.Count - 1)
        {
            currentIndex++;
            currentLineIndex = 0;
            ShowNode();
            return;
        }

        if (currentNode.choices.Count > 0)
        {
            if (savedChoices.TryGetValue(currentNode, out DialogueChoice choice))
            {
                if (choice.nextNode != null)
                {
                    history.Add(choice.nextNode);
                    currentIndex = history.Count - 1;
                    currentLineIndex = 0;
                    ShowNode();
                }
            }

            return;
        }

        if (currentNode.nextNode != null)
        {
            history.Add(currentNode.nextNode);
            currentIndex = history.Count - 1;
            currentLineIndex = 0;
            ShowNode();
        }
    }

    public void PreviousDialogue()
    {
        if (history.Count == 0)
            return;


        if (currentLineIndex > 0)
        {
            currentLineIndex--;
            ShowNode();
            return;
        }


        if (currentIndex > 0)
        {
            currentIndex--;

            DialogueNode previousNode = history[currentIndex];

            currentLineIndex = Mathf.Max(
                0,
                previousNode.dialogueLines.Count - 1
            );

            ShowNode();
        }
    }

    public void ChooseOption(DialogueChoice choice)
    {
        if (choice == null)
            return;

        DialogueNode currentNode = history[currentIndex];


        if (savedChoices.ContainsKey(currentNode))
            return;


        if (choice.miniGameType == MiniGameType.None &&
            choice.nextNode == null)
        {
            Debug.LogWarning("The selected choice has no Next Node.");
            return;
        }

        savedChoices[currentNode] = choice;

        switch (choice.miniGameType)
        {
            case MiniGameType.Typing:
                if (miniGameManager != null)
                    miniGameManager.OpenTypingGame();
                break;

            case MiniGameType.Maze:
                if (miniGameManager != null)
                    miniGameManager.OpenMazeGame();
                break;

            case MiniGameType.Code:
                if (miniGameManager != null)
                    miniGameManager.OpenCodeGame();
                break;

            case MiniGameType.None:
                history.Add(choice.nextNode);
                currentIndex = history.Count - 1;
                currentLineIndex = 0;
                ShowNode();
                break;
        }
    }

    private void ShowNode()
    {
        if (history.Count == 0)
            return;

        DialogueNode node = history[currentIndex];


        if (node.character != null)
        {
            characterNameText.text = node.character.characterName;

            characterPortrait.sprite = node.character.characterImage;
            characterPortrait.enabled =
                node.character.characterImage != null;

            dialogueText.color = node.character.textColor;

            if (node.character.dialogueBox != null)
            {
                dialogueBoxImage.sprite = node.character.dialogueBox;
                dialogueBoxImage.enabled = true;
            }
        }

    
        if (node.background != null)
        {
            backgroundImage.sprite = node.background.backgroundImage;
            backgroundImage.enabled =
                node.background.backgroundImage != null;
        }

        if (node.changeTime && storyTimeText != null)
        {
            storyTimeText.text = node.storyTime;
        }


        if (node.dialogueLines.Count > 0)
        {
            currentLineIndex = Mathf.Clamp(
                currentLineIndex,
                0,
                node.dialogueLines.Count - 1
            );

            dialogueText.text = node.dialogueLines[currentLineIndex];
        }
        else
        {
            dialogueText.text = "";
        }

        backButton.interactable =
            currentIndex > 0 || currentLineIndex > 0;


        foreach (Button button in choiceButtons)
        {
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
        }


        bool hasSavedChoice = savedChoices.ContainsKey(node);

        if (node.choices.Count > 0 && !hasSavedChoice)
        {
            for (int i = 0; i < node.choices.Count &&
                            i < choiceButtons.Count; i++)
            {
                DialogueChoice choice = node.choices[i];
                Button button = choiceButtons[i];

                button.gameObject.SetActive(true);

                TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();

                if (buttonText != null)
                    buttonText.text = choice.choiceText;

                button.onClick.AddListener(() => ChooseOption(choice));
            }
        }


        bool hasMoreLines =
            currentLineIndex < node.dialogueLines.Count - 1;

        bool canFollowNextNode = node.nextNode != null;

        bool canFollowSavedChoice =
            hasSavedChoice &&
            savedChoices[node].nextNode != null;

        nextButton.interactable =
            hasMoreLines ||
            currentIndex < history.Count - 1 ||
            (node.choices.Count == 0 && canFollowNextNode) ||
            (node.choices.Count > 0 && canFollowSavedChoice);
    }




    public void RestartStory()
    {
        //inside
        if (story == null || story.startNode == null)
            return;

        // Cerrar los minijuegos y volver al diálogo.
        if (miniGameManager != null)
            miniGameManager.ResetMinigames();

        // Borrar el progreso anterior.
        history.Clear();
        savedChoices.Clear();

        currentIndex = 0;
        currentLineIndex = 0;

        // Volver al inicio y restaurar las vidas.
        history.Add(story.startNode);

        if (GameProgressManager.Instance != null)
            GameProgressManager.Instance.ResetLives();

        ShowNode();
    }
}
