using UnityEngine;

// pang pasa ng data sa iba scene
public static class PlayerData
{
    public static int maxHealth = 100;
    public static int currentHealth = 100;
    public static int maxMana = 100;
    public static int currentMana = 100;

    public static void Reset()
    {
        maxHealth = 100;
        currentHealth = 100;
        maxMana = 100;
        currentMana = 100;
    }
}