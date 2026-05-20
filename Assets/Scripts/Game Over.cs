using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    void Start()
    {
        SoundManager.instance.PlayBGM(SoundManager.instance.gameOverBGM);
    }

    public void StartBattle()
    {
        SceneManager.LoadScene("Menu Scene");
    }

}