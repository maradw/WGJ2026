using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypingSystem : MonoBehaviour
{
    [Header("UI")]

    [SerializeField] private TMP_InputField answerField;
    [SerializeField] Button submitButton;
    [SerializeField] TextMeshProUGUI phrase;

    [SerializeField] private string instruction = "Escribe la siguiente frase:";
    [SerializeField] private string correctPhrase = "Tengo que aprender a decir que no";
    [SerializeField] private MiniGameManager miniGameManager;

    private void Start()
    {
        phrase.text = instruction;
        submitButton.onClick.AddListener(SubmitAnswer);
    }

    public void SubmitAnswer()
    {
        if (answerField.text.Trim().Equals(
            correctPhrase.Trim(),
            System.StringComparison.OrdinalIgnoreCase))
        {
            print("Correct!");
            miniGameManager.FinishMinigame();
        }
        else
        {
           
            print("Incorrect. Try again.");
            GameProgressManager.Instance.LoseLife();
            answerField.text = "";
            answerField.ActivateInputField();
        }
    }
}
