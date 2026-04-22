using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public void StartBattle()
    {
        SceneManager.LoadScene("Game Scene");
    }

}