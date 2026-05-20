using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StoryIntroSequence : MonoBehaviour
{
	// Object references
	public Transform hero;
	public Transform boss;
	public GameObject[] normalEnemies;

	// UI references
	public GameObject dialoguePanel;
	public Image heroPortrait;
	public TextMeshProUGUI dialogueText;

	// Settings
	public float walkSpeed = 4f;
	public string nextSceneName = "GameScene";
	public string[] arrogantRemarks;

	void Start()
	{
		dialoguePanel.SetActive(true);
		StartCoroutine(PlayIntroSequence());
	}

	IEnumerator PlayIntroSequence()
	{
		// Loop through and destroy each normal enemy
		for (int i = 0; i < normalEnemies.Length; i++)
		{
			// Set arrogant text based on the enemy index
			if (i < arrogantRemarks.Length)
			{
				dialogueText.text = arrogantRemarks[i];
			}

			// Wait until hero reaches the enemy
			yield return StartCoroutine(WalkToPosition(normalEnemies[i].transform.position));

			// Insta-kill the enemy
			Destroy(normalEnemies[i]);

			// Brief pause before moving to the next
			yield return new WaitForSeconds(0.5f);
		}

		// Approach the boss
		dialogueText.text = "Finally, the boss. Try not to die in one hit.";
		yield return StartCoroutine(WalkToPosition(boss.position));

		// Fake boss death
		SpriteRenderer bossSprite = boss.GetComponent<SpriteRenderer>();
		bossSprite.color = Color.gray;
		dialogueText.text = "Yawn. Is that it? I'm going back to sleep.";

		// Wait for comedic timing
		yield return new WaitForSeconds(1.5f);

		// Boss revival and Balance Update
		bossSprite.color = Color.red;

		// Hide hero portrait and show system text
		heroPortrait.gameObject.SetActive(false);
		dialogueText.text = "<color=red>SYSTEM OVERRIDE: CAST BALANCE UPDATE .</color>";

		// Let the player read the dramatic text
		yield return new WaitForSeconds(2.5f);

		// Teleport to the actual game
		SceneManager.LoadScene(nextSceneName);
	}

	IEnumerator WalkToPosition(Vector3 targetPosition)
	{
		// Move hero towards target until they are very close
		while (Vector3.Distance(hero.position, targetPosition) > 0.1f)
		{
			hero.position = Vector3.MoveTowards(hero.position, targetPosition, walkSpeed * Time.deltaTime);
			yield return null;
		}
	}
}