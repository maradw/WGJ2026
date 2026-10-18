using UnityEngine;
using System;
public class MiniGameManager : MonoBehaviour
{
    [SerializeField] private TypingSystem typingSystem;
    [SerializeField] private MouseMazePlayer mouseMazePlayer;
    [SerializeField] private WordInputManager wordInputManager;
    [SerializeField] private GameObject codePanel;
    [SerializeField] private GameObject typingPanel;
    [SerializeField] private GameObject mazePanel;
    public Action OnMinigameFinished;
    private void Start()
    {
        typingSystem.enabled = false;
        mouseMazePlayer.enabled = false;

        typingPanel.SetActive(false);
        mazePanel.SetActive(false);

        wordInputManager.enabled = false;
        codePanel.SetActive(false);
        
    }


    public void FinishMinigame()
    {
        typingSystem.enabled = false;
        mouseMazePlayer.enabled = false;
        wordInputManager.enabled= false;
        OnMinigameFinished?.Invoke();
    }

    public void OpenTypingGame()
    {

        typingPanel.SetActive(true);
        typingSystem.enabled = true;
    }

    public void OpenMazeGame()
    {
        mazePanel.SetActive(true);
        mouseMazePlayer.enabled = true;
    }
    public void OpenCodeGame()
    {
        codePanel.SetActive(true);
        wordInputManager.enabled = true;
    }
}
