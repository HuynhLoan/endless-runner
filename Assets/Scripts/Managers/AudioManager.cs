using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music Clips")]
    public AudioClip mainMenuMusic;
    public AudioClip gameplayMusic;

    [Header("SFX Clips")]
    public AudioClip jumpSound;
    public AudioClip coinSound;
    public AudioClip buttonClickSound;
    public AudioClip hitSound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ================= MUSIC =================
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
    public void PlayHit()
    {
        sfxSource.PlayOneShot(hitSound);
    }

    public void PlayMainMenuMusic()
    {
        PlayMusic(mainMenuMusic);
    }

    public void PlayGameplayMusic()
    {
        PlayMusic(gameplayMusic);
    }

    // ================= SFX =================
    public void PlayJump()
    {
        sfxSource.PlayOneShot(jumpSound);
    }

    public void PlayCoin()
    {
        sfxSource.PlayOneShot(coinSound);
    }

    public void PlayButtonClick()
    {
        sfxSource.PlayOneShot(buttonClickSound);
    }
}
