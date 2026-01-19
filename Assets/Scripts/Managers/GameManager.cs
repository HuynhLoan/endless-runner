using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Score")]
    public float score;
    public int coins;
    public float scoreSpeed = 10f;

    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (isGameOver) return;

        score += Time.deltaTime * scoreSpeed;
    }

    public int GetScore()
    {
        return Mathf.FloorToInt(score);
    }

    public void AddCoin(int amount)
    {
        coins += amount;
    }

    public void GameOver()
    {
        isGameOver = true;
    }
}
