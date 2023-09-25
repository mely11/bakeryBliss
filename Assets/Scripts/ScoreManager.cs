using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int playerScore = 0;
    
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
