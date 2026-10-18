using System;
using UnityEditor.Rendering.LookDev;
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
        DialogueFold.SetActive(true);
        typingSystem.enabled = false;
        mouseMazePlayer.enabled = false;
        wordInputManager.enabled= false;
        codePanel.SetActive(false);
        OnMinigameFinished?.Invoke();
    }

    public void OpenTypingGame()
    {
        DialogueFold.SetActive(false);
        typingPanel.SetActive(true);
        typingSystem.enabled = true;
    }

    public void OpenMazeGame()
    {
        DialogueFold.SetActive(false);
        mazePanel.SetActive(true);
        mouseMazePlayer.enabled = true;
    }
    public void OpenCodeGame()
    {
        DialogueFold.SetActive(false);
        codePanel.SetActive(true);
        wordInputManager.enabled = true;
    }
}
