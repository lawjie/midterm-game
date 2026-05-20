using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum SkillSlot { Skill1, Skill2, Skill3 }
    public SkillSlot skillSlot;

    public BattleStatus statusUI;

    private string GetDescription()
    {
        switch (WeaponData.equippedWeapon)
        {
            case WeaponType.Sword:
                switch (skillSlot)
                {
                    case SkillSlot.Skill1: return "Strike (30 mana) — Deal instant damage and skip minigame.";
                    case SkillSlot.Skill2: return "Parry (20 mana) — Reduces incoming damage next turn.";
                    case SkillSlot.Skill3: return "Rend (35 mana) — Applies a Rupture stack to the enemy.";
                }
                break;

            case WeaponType.Bow:
                switch (skillSlot)
                {
                    case SkillSlot.Skill1: return "Aimed Shot (35 mana) — Grants bonus damage per correct arrow next attack.";
                    case SkillSlot.Skill2: return "Trap (30 mana) — Negates all enemy damage next turn.";
                    case SkillSlot.Skill3: return "Poison Arrow (35 mana) — Applies a Poison stack to the enemy.";
                }
                break;

            case WeaponType.Staff:
                switch (skillSlot)
                {
                    case SkillSlot.Skill1: return "Mana Burst (60 mana) — Converts mana into bonus attack damage next turn.";
                    case SkillSlot.Skill2: return "Silence (50 mana) — Suppresses the enemy's ability for the next attack.";
                    case SkillSlot.Skill3: return "Scorch (35 mana) — Applies a Burn stack to the enemy.";
                }
                break;

            default:
                switch (skillSlot)
                {
                    case SkillSlot.Skill1: return "Strike (30 mana) — Skips the minigame, dealing full damage automatically.";
                    case SkillSlot.Skill2: return "Parry (20 mana) — Reduces incoming enemy damage by half next turn.";
                    case SkillSlot.Skill3: return "Rend (35 mana) — Applies a Rupture stack to the enemy.";
                }
                break;
        }
        return "";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        statusUI.SetMessage(GetDescription());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        statusUI.ClearMessage();
    }
}