using System.Collections.Generic;
using UnityEngine;

// send data
public static class GameData
{
    public static int tutorialEnemiesDefeated = 0;
    public static int tutorialEnemyGoal = 5;
    public static bool canEncounter = false;
    public static List<string> defeatedEnemies = new List<string>();
    public static string currentEnemyID;
    public static Vector3 playerPosition;
    public static bool hasSavedPosition = false;
    public static bool returningFromBattle = false;
    public static bool justFinishedTutorial = false;
}