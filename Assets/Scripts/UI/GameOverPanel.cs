using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverPanel : MonoBehaviour
{
    [Header("Result UI")]
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalCoinText;

    public void Show()
    {
        gameObject.SetActive(true);

        finalScoreText.text = "Score: " + Mathf.FloorToInt(GameManager.Instance.score);
        finalCoinText.text  = "Coin: "  + GameManager.Instance.coins;
    }

    public void OnRestartClicked()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
