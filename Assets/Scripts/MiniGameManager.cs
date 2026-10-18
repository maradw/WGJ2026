using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    [SerializeField] TypingSystem typingSystem;
    [SerializeField] MouseMazePlayer mouseMazePlayer;

    void Start()
    {
        mouseMazePlayer.enabled = false;
        typingSystem.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
