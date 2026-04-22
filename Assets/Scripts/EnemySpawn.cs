// diito ung enemy detection galing game scene papunta battle scene

using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    public string enemyID;
    public Sprite sprite;

    // set enemy hp
    public int maxHealth;
}

public class EnemySpawn : MonoBehaviour
{
    public EnemySpawnData[] enemies;

    public Sprite GetSprite(string id)
    {
        foreach (var enemy in enemies)
        {
            if (enemy.enemyID == id)
                return enemy.sprite;
        }

        return null;
    }

    public EnemySpawnData GetEnemyData(string id)
    {
        foreach (var enemy in enemies)
        {
            if (enemy.enemyID == id)
                return enemy;
        }

        return null;
    }
}