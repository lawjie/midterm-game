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
        PlayerData.Reset();
        GameData.Reset();
        WeaponData.equippedWeapon = WeaponType.None;
        SceneManager.LoadScene("Tutorial Scene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}