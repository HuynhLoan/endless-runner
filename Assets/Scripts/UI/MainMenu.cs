using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayMainMenuMusic();
    }

    public void StartGame()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();

        SceneManager.LoadScene("Gameplay");
    }

    public void QuitGame()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();

        Application.Quit();
    }
}
