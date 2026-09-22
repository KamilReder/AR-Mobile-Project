using UnityEngine;
using TMPro; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Rules")]
    [SerializeField] private float timeLimitSeconds = 60f;

    [Header("UI")] //optional - unnecessary if I'm not making UI but can be useful
    [SerializeField] private TextMeshProUGUI statusText;

    private int totalCards;
    private int collectedCards;
    private float timeRemaining;
    private bool gameOver;

    private void Awake()
    {
       
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        timeRemaining = timeLimitSeconds;
        UpdateStatusText();
    }

    private void Update()
    {
        if (gameOver || totalCards == 0)
            return; // don't start the timer until cards have actually spawned

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            Lose();
        }

        UpdateStatusText();
    }

    // Called once by CardSpawner right after it spawns the cards, so
    // the win condition knows how many need collecting.
    public void SetTotalCards(int count)
    {
        totalCards = count;
        collectedCards = 0;
        gameOver = false;
        UpdateStatusText();
    }

    // Reports to GameManager when the player successfully taps a card.
    public void ReportCardCollected()
    {
        if (gameOver)
            return;

        collectedCards++;

        if (collectedCards >= totalCards)
            Win();
        else
            UpdateStatusText();
    }

    private void Win()
    {
        gameOver = true;
        if (statusText != null)
            statusText.text = $"You win! Collected {collectedCards}/{totalCards} cards.";
        Debug.Log("[GameManager] Player won.");
    }

    private void Lose()
    {
        gameOver = true;
        if (statusText != null)
            statusText.text = $"Time's up! Collected {collectedCards}/{totalCards} cards.";
        Debug.Log("[GameManager] Player lost -- timer ran out.");
    }

    private void UpdateStatusText()
    {
        if (statusText == null)
            return;

        if (!gameOver)
            statusText.text = $"Cards: {collectedCards}/{totalCards}    Time: {timeRemaining:F0}s";
    }
}