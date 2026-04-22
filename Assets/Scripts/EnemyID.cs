using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyID; // identifier

    void Start()
    {
        if (GameData.defeatedEnemies.Contains(enemyID))
        {
            Destroy(gameObject); // pag natalo kalaban delete
        }
    }
}