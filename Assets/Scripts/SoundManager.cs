using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource sfxSource;    // for sound effects
    public AudioSource bgmSource;    // for background music

    [Header("Skill SFX")]
    public AudioClip skill1Clip;
    public AudioClip skill2Clip;
    public AudioClip skill3Clip;

    [Header("Player SFX")]
    public AudioClip footstepsClip;

    [Header("BGM")]
    public AudioClip mainMenuBGM;
    public AudioClip exploringBGM;
    public AudioClip enemyEncounterBGM;
    public AudioClip battleSceneBGM;
    public AudioClip victoryBGM;
    public AudioClip gameOverBGM;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}