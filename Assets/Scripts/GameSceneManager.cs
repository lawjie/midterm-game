using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public Transform spawnPoint;

    void Start()
    {
        // from tutorila
        if (GameData.justFinishedTutorial)
        {
            GameData.playerPosition = spawnPoint.position;

            GameData.justFinishedTutorial = false;
            GameData.returningFromBattle = false; // doenst work

            return;
        }

        // after battle
        if (GameData.returningFromBattle)
        {
            GameData.returningFromBattle = false;
            return;
        }

        // spawnpojnt\
        GameData.playerPosition = spawnPoint.position;
    }
}