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
        PlayerData.Reset();
        GameData.Reset();
        WeaponData.equippedWeapon = WeaponType.None;
        SceneManager.LoadScene("Main Menu");
    }

}