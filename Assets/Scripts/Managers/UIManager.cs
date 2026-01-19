using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Pause Menu")]
    public PauseMenu pauseMenu;

    [Header("Game Over Panel")]
    public GameOverPanel gameOverPanel;

    [Header("Player")]
    public PlayerController player;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (AudioManager.Instance != null)
        AudioManager.Instance.PlayGameplayMusic();
        
        // auto find nếu quên kéo
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }

        // gán cho PauseMenu
        if (pauseMenu != null)
        {
            pauseMenu.player = player;
        }
    }

    // ===== UI API =====

    public void OnPauseButtonClicked()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();

        if (pauseMenu != null)
            pauseMenu.TogglePause();
    }


    public void ShowGameOver()
    {
            Debug.Log("ShowGameOver CALLED");

        if (gameOverPanel != null)
            gameOverPanel.Show();
    }
}
