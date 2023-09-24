using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    private int playerScore = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        }
    }

    public void AddToScore(int amount)
    {
        playerScore += amount;
        // Update UI or perform other actions as needed
    }

    public void DeductFromScore(int amount)
    {
        playerScore -= amount;
        // Update UI or perform other actions as needed
    }

    
}
