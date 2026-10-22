using System;

using UnityEngine;
public class MiniGameManager : MonoBehaviour
{
    [SerializeField] private TypingSystem typingSystem;
    [SerializeField] private MouseMazePlayer mouseMazePlayer;
    [SerializeField] private WordInputManager wordInputManager;
    [SerializeField] private GameObject codePanel;
    [SerializeField] private GameObject typingPanel;
    [SerializeField] private GameObject mazePanel;
    public Action OnMinigameFinished;

    [SerializeField] GameObject DialogueFold;

    [SerializeField] private GameObject miniGamePanel;
    [SerializeField] private GameObject mazeText;
    private void Start()
    {
        miniGamePanel.SetActive(false);
        typingSystem.enabled = false;
        mouseMazePlayer.enabled = false;
        mouseMazePlayer.gameObject.SetActive(false);
        typingPanel.SetActive(false);
        mazePanel.SetActive(false);

        wordInputManager.enabled = false;
        codePanel.SetActive(false);
        
    }


    public void FinishMinigame()
    {
        DialogueFold.SetActive(true);
        typingSystem.enabled = false;
        mouseMazePlayer.enabled = false;
        wordInputManager.enabled= false;
        codePanel.SetActive(false);
        OnMinigameFinished?.Invoke();
        mouseMazePlayer.gameObject.SetActive(false);
        miniGamePanel.SetActive(false);
        typingPanel.SetActive(false);
    }

    public void OpenTypingGame()
    {
        miniGamePanel.SetActive(true);
        mazeText.SetActive(false);
        DialogueFold.SetActive(false);
        typingPanel.SetActive(true);
        typingSystem.enabled = true;
    }

    public void OpenMazeGame()
    {
        miniGamePanel.SetActive(true);
        mazeText.SetActive(true);
        mouseMazePlayer.gameObject.SetActive(true);
        DialogueFold.SetActive(false);
        mazePanel.SetActive(true);
        mouseMazePlayer.enabled = true;
    }
    public void OpenCodeGame()
    {
        miniGamePanel.SetActive(true);
        mazeText.SetActive(false);
        DialogueFold.SetActive(false);
        codePanel.SetActive(true);
        wordInputManager.enabled = true;
    }
    public void ResetMinigames()
    {
        miniGamePanel.SetActive(false);
        typingSystem.enabled = false;
        mouseMazePlayer.enabled = false;
        wordInputManager.enabled = false;
        mouseMazePlayer.gameObject.SetActive(false);
        typingPanel.SetActive(false);
        mazePanel.SetActive(false);
        codePanel.SetActive(false);

        DialogueFold.SetActive(true);
    }
}
