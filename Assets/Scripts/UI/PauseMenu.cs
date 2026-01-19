using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Panel")]
    public GameObject pausePanel;

    [Header("Player")]
    public PlayerController player;

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);
    }

    // Gọi từ Pause Button
    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;

        if (player != null)
            player.PausePlayer();
    }


    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();
        if (player != null)
            player.ResumePlayer();
    }

    public void QuitToMainMenu()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
