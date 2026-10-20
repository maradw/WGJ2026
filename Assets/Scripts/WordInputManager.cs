
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class WordInputManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField[] inputFields;
    [Header("Answer")]
    [SerializeField] private string correctWord;

    [Header("Feedback")]
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private MiniGameManager miniGameManager;
    private void Start()
    {
        for (int i = 0; i < inputFields.Length; i++)
        {
            int index = i;

            inputFields[i].characterLimit = 1;

            inputFields[i].onValueChanged.AddListener(
                (text) => OnLetterEntered(text, index)
            );
        }
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            (Keyboard.current.enterKey.wasPressedThisFrame ||
             Keyboard.current.numpadEnterKey.wasPressedThisFrame))
        {
            CheckAnswer();
        }
    }
    private void OnLetterEntered(string text, int index)
    {
        if (string.IsNullOrEmpty(text))
            return;

        inputFields[index].SetTextWithoutNotify(text.ToUpper());

        for (int i = index + 1; i < inputFields.Length; i++)
        {
            if (inputFields[i].interactable)
            {
                inputFields[i].ActivateInputField();
                inputFields[i].Select();
                return;
            }
        }
    }

    public void CheckAnswer()
    {
        string answer = "";

        for (int i = 0; i < inputFields.Length; i++)
        {
            answer += inputFields[i].text;
        }

        if (answer.Trim().Equals(
            correctWord.Trim(),
            System.StringComparison.OrdinalIgnoreCase))
        {
            feedbackText.text = "Correct!";
            Debug.Log("Correct answer!");

            miniGameManager.FinishMinigame();
        }
        else
        {
            feedbackText.text = "Incorrect!";
            GameProgressManager.Instance.LoseLife();
            Debug.Log("Incorrect answer!");
        }
    }
}
