using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        SoundManager.instance.PlayBGM(SoundManager.instance.mainMenuBGM);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Story Intro");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}