using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypingSystem : MonoBehaviour
{
    [Header("UI")]

    [SerializeField] private TMP_InputField answerField;
    [SerializeField] Button submitButton;
    [SerializeField] TextMeshProUGUI phrase;

    private string correctPhrase = "aprende a decir q no waza";

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
        }
        else
        {
            print("Incorrect. Try again.");
            answerField.text = "";
            answerField.ActivateInputField();
        }
    }
}
