using UnityEngine;
using TMPro;

public class GameplayUI : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text coinText;

    void Update()
    {
        if (GameManager.Instance == null) return;

        SetScore(GameManager.Instance.GetScore());
        SetCoins(GameManager.Instance.coins);
    }

    public void SetScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void SetCoins(int coins)
    {
        if (coinText != null)
            coinText.text = "Coins: " + coins;
    }
}
