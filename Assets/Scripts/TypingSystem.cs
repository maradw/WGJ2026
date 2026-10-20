using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypingSystem : MonoBehaviour
{
    [Header("UI")]

    [SerializeField] private TMP_InputField answerField;
    [SerializeField] Button submitButton;
    [SerializeField] TextMeshProUGUI phrase;

    private string correctPhrase = "aprende a decir que no";
    [SerializeField] private MiniGameManager miniGameManager;

    private void Start()
    {
        phrase.text = correctPhrase;
        submitButton.onClick.AddListener(SubmitAnswer);
    }

    public void SubmitAnswer()
    {
        if (answerField.text == correctPhrase)
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
