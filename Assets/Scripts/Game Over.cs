using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void StartBattle()
    {
        SceneManager.LoadScene("Game Scene");
    }

}