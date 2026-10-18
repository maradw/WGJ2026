
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    // Registra la decisión tomada en cada nodo.
    private Dictionary<DialogueNode, DialogueChoice> savedChoices =
        new Dictionary<DialogueNode, DialogueChoice>();

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
    }
    private void OnDisable()
    {
        nextButton.onClick.RemoveListener(NextDialogue);
        backButton.onClick.RemoveListener(PreviousDialogue);
    }
    public void NextDialogue()
    {
        DialogueNode currentNode = history[currentIndex];

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

        history.Add(targetNode);
        currentIndex++;

        ShowNode();
    }

    public void PreviousDialogue()
    {
        if (currentIndex <= 0)
            return;

        currentIndex--;
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

        if (choice.nextNode == null)
        {
            Debug.LogWarning("La opcion no tiene un nodo de destino.");
            return;
        }

        savedChoices.Add(currentNode, choice);

        // Avanzamos inmediatamente por la ruta elegida.
        history.Add(choice.nextNode);
        currentIndex++;

        ShowNode();
    }

    private void ShowNode()
    {
        DialogueNode node = history[currentIndex];

        characterNameText.text = node.characterName;
        dialogueText.text = node.dialogueText;

        characterPortrait.sprite = node.characterPortrait;
        characterPortrait.enabled = node.characterPortrait != null;

        backButton.interactable = currentIndex > 0;

        // Ocultar todas las opciones anteriores.
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            choiceButtons[i].gameObject.SetActive(false);
        }

        bool hasChoices = node.choices.Count > 0;
        bool choiceAlreadyMade = savedChoices.ContainsKey(node);

        // Mostrar opciones solo si todavía no se eligió.
        if (hasChoices && !choiceAlreadyMade)
        {
            for (int i = 0; i < node.choices.Count; i++)
            {
                if (i >= choiceButtons.Count)
                {
                    Debug.LogWarning(
                        "No hay suficientes botones para las opciones.");
                    break;
                }

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

        // En un nodo con decisiones, Siguiente solo funciona
        // cuando ya se ha elegido una opción.
        if (hasChoices)
        {
            nextButton.interactable =
                choiceAlreadyMade &&
                GetSavedTarget(node) != null;
        }
        else
        {
            nextButton.interactable = node.nextNode != null;
        }
    }

    private DialogueNode GetSavedTarget(DialogueNode node)
    {
        if (savedChoices.TryGetValue(node, out DialogueChoice choice))
            return choice.nextNode;

        return null;
    }
}
