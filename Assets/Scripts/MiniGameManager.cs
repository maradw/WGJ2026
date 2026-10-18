using UnityEngine;
using System;
public class MiniGameManager : MonoBehaviour
{
    [SerializeField] private TypingSystem typingSystem;
    [SerializeField] private MouseMazePlayer mouseMazePlayer;

    [SerializeField] private GameObject typingPanel;
    [SerializeField] private GameObject mazePanel;
    public Action OnMinigameFinished;
    private void Start()
    {
        typingSystem.enabled = false;
        mouseMazePlayer.enabled = false;

        typingPanel.SetActive(false);
        mazePanel.SetActive(false);
    }


    public void FinishMinigame()
    {
        typingSystem.enabled = false;
        mouseMazePlayer.enabled = false;

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
}
