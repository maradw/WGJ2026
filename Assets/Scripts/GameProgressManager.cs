
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance { get; private set; }

    [Header("Lives")]
    [SerializeField] private int startingLives = 3;

    [Header("Events")]
    [SerializeField] private UnityEvent<int> onLivesChanged;
    [SerializeField] private UnityEvent onGameOver;

    public int CurrentLives { get; private set; }

    [Header("Lives UI")]
    [SerializeField] private Image[] heartImages;
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;




    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        CurrentLives = startingLives;

        UpdateLivesUI();
    }

    private void Start()
    {

        onLivesChanged?.Invoke(CurrentLives);
    }
    private void UpdateLivesUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] == null)
                continue;

            heartImages[i].sprite =
                i < CurrentLives ? fullHeartSprite : emptyHeartSprite;
        }
    }
    public void LoseLife()
    {
        if (CurrentLives <= 0)
            return;

        CurrentLives--;
        UpdateLivesUI();
        onLivesChanged?.Invoke(CurrentLives);

        Debug.Log("Lives remaining: " + CurrentLives);

        if (CurrentLives == 0)
        {

            onGameOver?.Invoke();
        }
    }
    public void ResetLives()
    {
        CurrentLives = startingLives;
        UpdateLivesUI();
        onLivesChanged?.Invoke(CurrentLives);
       
    }
}
