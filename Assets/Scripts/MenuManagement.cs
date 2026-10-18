using Game.Audio;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagement : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button creditsButton;
    [SerializeField] MusicData BgMusic;
    void Start()
    {
        Debug.Log("1. Inicio");

        MusicManager.Instance.PlayBG(BgMusic);
        Debug.Log("2. Música iniciada");

        startButton.onClick.AddListener(() => MusicManager.Instance.StopFade(1f));
        Debug.Log("3. Start asignado");

        exitButton.onClick.AddListener(TransitionManager.Instance.ExitGame);
        Debug.Log("4. Exit asignado");

    }

    void Update()
    {
        
    }
    void OnDestroy()
    {
        startButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
        creditsButton.onClick.RemoveAllListeners();

    }
   
}
