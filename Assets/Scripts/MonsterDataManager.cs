using UnityEngine;
using TMPro;

public enum MonsterType
{
    Wraith,
    Oni,
    Slime,
    Skeleton,
    Seraphim,
    Spider
}

public class MonsterDataManager : MonoBehaviour
{
    public TextMeshProUGUI monsterNameText;
    public TextMeshProUGUI abilityText;

    public MinigameManager minigame;

    public void SetupMonster(string enemyID)
    {
        MonsterType type = GetMonsterType(enemyID);

        switch (type)
        {
            case MonsterType.Slime:
                monsterNameText.text = "Slime";
                abilityText.text = "Arrows are covered in goo, press twice to remove";
                minigame.currentType = MonsterType.Slime;
                break;

            case MonsterType.Spider:
                monsterNameText.text = "Spider";
                abilityText.text = "Timer is slightly faster";
                minigame.currentType = MonsterType.Spider;
                break;

            case MonsterType.Skeleton:
                monsterNameText.text = "Skeleton";
                abilityText.text = "Arrows are reversed";
                minigame.currentType = MonsterType.Skeleton;
                break;

            case MonsterType.Wraith:
                monsterNameText.text = "Wraith";
                abilityText.text = "Arrows turn invisible after a while";
                minigame.currentType = MonsterType.Wraith;
                break;

            case MonsterType.Oni:
                monsterNameText.text = "Oni";
                abilityText.text = "Incorrect arrows deal 2x damage";
                minigame.currentType = MonsterType.Oni;
                break;

            case MonsterType.Seraphim:
                monsterNameText.text = "Seraphim";
                abilityText.text = "Random arrow abilities";
                minigame.currentType = MonsterType.Seraphim;
                break;
        }
    }

    public void ApplySilenceText()
    {
        abilityText.text = "Silenced! Enemy ability suppressed.";
    }

    MonsterType GetMonsterType(string id)
    {
        if (id.Contains("slime")) return MonsterType.Slime;
        if (id.Contains("spider")) return MonsterType.Spider;
        if (id.Contains("bones")) return MonsterType.Skeleton;
        if (id.Contains("wraith")) return MonsterType.Wraith;
        if (id.Contains("oni")) return MonsterType.Oni;
        if (id.Contains("iris")) return MonsterType.Seraphim;

        return MonsterType.Slime;
    }
}