// dito ung collision ng player at enemy, tas punta next scene

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemyEncounter : MonoBehaviour
{
    public string encounterSceneName = "Battle Scene";
    private bool hasTriggered = false;
    private bool canTrigger = false;

    void Start()
    {
        StartCoroutine(EnableTriggerDelay());
    }

    IEnumerator EnableTriggerDelay()
    {
        yield return new WaitForSeconds(0.5f);
        canTrigger = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered) return;
        if (!canTrigger) return;

        if (collision.GetComponentInParent<Player>() != null)
        {
            hasTriggered = true;
            StartCoroutine(StartEncounter(collision.gameObject));
        }
    }

    IEnumerator StartEncounter(GameObject player)
    {
        Player playerScript = player.GetComponentInParent<Player>();

        if (playerScript != null)
        {
            GameData.playerPosition = player.transform.position;
            GameData.hasSavedPosition = true;

            // pasa data
            PlayerData.maxHealth = playerScript.maxHealth;
            PlayerData.currentHealth = playerScript.currentHealth;

            // pasa data
            PlayerData.maxMana = 100;
            PlayerData.currentMana = 100;

            // cant move
            playerScript.enabled = false;
        }

        Enemy enemy = GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            GameData.currentEnemyID = enemy.enemyID;
        }
        else
        {
            Debug.LogError("Enemy script not found"); // error test
        }

        yield return new WaitForSeconds(0.3f); // delay

        SceneManager.LoadScene(encounterSceneName);
    }
}