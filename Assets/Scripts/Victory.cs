using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
    void Start()
    {
        SoundManager.instance.PlayBGM(SoundManager.instance.victoryBGM);
    }

    public void StartBattle()
    {
        SceneManager.LoadScene("Menu Scene");
    }

}