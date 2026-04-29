using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    void Start()
    {
        GameState.TutorialMode = true;

        if (GameData.tutorialEnemiesDefeated == 0)
            GameData.tutorialEnemiesDefeated = 0;
    }

    public void StartBattle()
    {
        SceneManager.LoadScene("Game Scene");
    }
}