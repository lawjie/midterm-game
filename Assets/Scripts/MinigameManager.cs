// dito ung minigame na arrow functions, nasa monsterarrow un iba

using UnityEngine;
using System.Collections.Generic;

public class MinigameManager : MonoBehaviour
{
    public GameObject panel;
    public Transform arrowParent;
    public GameObject arrowPrefab;

    public HealthBar timerBar;

    public MonsterType currentType;

    public float timeLimit = 5f;
    private float timer;

    private int incorrectHits = 0;
    private int correctHits = 0;
    private int totalArrows = 0;

    private List<MonsterArrow> arrows = new List<MonsterArrow>();

    private KeyCode[] possibleKeys =
    {
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow
    };

    void Update()
    {
        if (!panel.activeSelf) return;

        // timer bar
        timer -= Time.deltaTime;
        timer = Mathf.Max(timer, 0f);

        timerBar.SetHealth(timer);

        const float epsilon = 0.001f;

        if (timer <= epsilon)
        {
            timer = 0f;
            timerBar.SetHealth(timer);

            EndMinigame(false);
            return;
        }

        // input ng arrow key
        if (Input.anyKeyDown)
        {
            foreach (KeyCode key in possibleKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    HandleInput(key);
                    break;
                }
            }
        }
    }

    public void StartMinigame(int arrowCount)
    {
        totalArrows = arrowCount;
        panel.SetActive(true);

        correctHits = 0;
        incorrectHits = 0;

        // spoder abilty
        float actualTime = timeLimit;

        if (currentType == MonsterType.Spider)
        {
            actualTime *= 0.6f; // edit if too slow
        }

        timer = actualTime;

        timerBar.SetMaxHealth(actualTime);
        timerBar.SetHealth(actualTime);

        // clear arrows
        foreach (Transform child in arrowParent)
        {
            Destroy(child.gameObject);
        }

        arrows.Clear();

        // spawn arrows
        for (int i = 0; i < arrowCount; i++)
        {
            GameObject obj = Instantiate(arrowPrefab, arrowParent);
            MonsterArrow arrow = obj.GetComponent<MonsterArrow>();

            KeyCode randomKey = possibleKeys[Random.Range(0, possibleKeys.Length)];
            arrow.SetDirection(randomKey);

            // apply ability
            arrow.ApplyMonsterEffect(currentType);

            arrows.Add(arrow);
        }
    }

    void HandleInput(KeyCode key)
    {
        if (arrows.Count == 0) return;

        MonsterArrow first = arrows[0];

        KeyCode inputKey = key;

        // sekelton
        if (first.arrowType == MonsterType.Skeleton)
        {
            inputKey = GetReversedKey(key);
        }

        int result = first.TryInput(inputKey);

        if (result == 1)
        {
            correctHits++;
            arrows.RemoveAt(0);

            if (arrows.Count == 0)
            {
                EndMinigame(true);
            }
        }
        else if (result == -1)
        {
            int damage = 1;

            if (currentType == MonsterType.Oni)
            {
                damage = 2;
            }

            incorrectHits += damage;
        }
    }

    KeyCode GetReversedKey(KeyCode key) // for skeleton ability
    {
        switch (key)
        {
            case KeyCode.UpArrow: return KeyCode.DownArrow;
            case KeyCode.DownArrow: return KeyCode.UpArrow;
            case KeyCode.LeftArrow: return KeyCode.RightArrow;
            case KeyCode.RightArrow: return KeyCode.LeftArrow;
        }
        return key;
    }

    void EndMinigame(bool success)
    {
        panel.SetActive(false);

        FindObjectOfType<BattleManager>().ResolveAttack(correctHits, incorrectHits);

        FindObjectOfType<BattleManager>().UpdateUITextVisibility();

        Debug.Log("Hits: " + correctHits);
    }
}