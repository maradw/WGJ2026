using System;
using Game.Audio;
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

    [Header("Audio")]
    [SerializeField] private MusicData mazeMusic;
    [SerializeField] private MusicData codeMusic;
    [SerializeField] private MusicData typingMusic;
    [SerializeField] private AudioClip typingSfx;
    [SerializeField] private AudioClip successSfx;
    // Música de la historia que sonaba antes del minijuego, para retomarla al terminar.
    private MusicData storyMusic;

    private void PlayMinigameMusic(MusicData music)
    {
        if (MusicManager.Instance == null)
            return;

        storyMusic = MusicManager.Instance.CurrentMusic;
        MusicManager.Instance.PlayBG(music);
    }

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
        // Antes de continuar la historia: el siguiente nodo puede cambiar la música otra vez.
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlaySFX(successSfx);
            MusicManager.Instance.PlayBG(storyMusic);
        }

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
        PlayMinigameMusic(typingMusic);
        if (MusicManager.Instance != null)
            MusicManager.Instance.PlaySFX(typingSfx);
    }

    public void OpenMazeGame()
    {
        miniGamePanel.SetActive(true);
        mazeText.SetActive(true);
        mouseMazePlayer.gameObject.SetActive(true);
        DialogueFold.SetActive(false);
        mazePanel.SetActive(true);
        mouseMazePlayer.enabled = true;
        PlayMinigameMusic(mazeMusic);
    }
    public void OpenCodeGame()
    {
        miniGamePanel.SetActive(true);
        mazeText.SetActive(false);
        DialogueFold.SetActive(false);
        codePanel.SetActive(true);
        wordInputManager.enabled = true;
        PlayMinigameMusic(codeMusic);
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
