using Game.Audio;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManagement : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button creditsButton;
    [SerializeField] MusicData Background;
    [SerializeField] MusicData Alarm;
    [SerializeField] string gameplayScene = "GamePlay";
   IEnumerator Start()
    {
        startButton.onClick.AddListener(() => MusicManager.Instance.StopFade(1f));
        startButton.onClick.AddListener(() => TransitionManager.Instance.LoadScene(gameplayScene));

        // El panel de créditos se abre desde el OnClick del botón en la escena.
        exitButton.onClick.AddListener(() => TransitionManager.Instance.ExitGame());
        yield return new WaitForSeconds(0.5f);
        MusicManager.Instance.PlayBG(Alarm);

       
        yield return new WaitForSeconds(7f);
        MusicManager.Instance.PlayBG(Background);

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
