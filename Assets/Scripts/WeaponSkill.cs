using UnityEngine;

public class WeaponSkills : MonoBehaviour
{
    [HideInInspector] public BattleManager battle;
    [HideInInspector] public MinigameManager minigame;

    // active effects
    public bool parryActive = false;
    public bool trapActive = false;
    public bool skipMinigameNextAttack = false;
    public int manaBurstBonus = 0;
    public int aimedShotBonus = 0;

    // -------------------------------------------------------
    // SWORD
    // -------------------------------------------------------
    [Header("Sword — Strike")]
    public int strikeCost = 30;
    private int strikeMaxCD = 3;
    public int strikeCooldown = 0;

    [Header("Sword — Parry")]
    public int parryCost = 20;
    private int parryMaxCD = 2;
    public int parryCooldown = 0;

    [Header("Sword — Rend")]
    public int rendCost = 35;
    private int rendMaxCD = 1;
    public int rendCooldown = 0;

    // -------------------------------------------------------
    // BOW
    // -------------------------------------------------------
    [Header("Bow — Aimed Shot")]
    public int aimedShotCost = 35;
    private int aimedShotMaxCD = 2;
    public int aimedShotCooldown = 0;

    [Header("Bow — Trap")]
    public int trapCost = 30;
    private int trapMaxCD = 3;
    public int trapCooldown = 0;

    [Header("Bow — Poison Arrow")]
    public int poisonArrowCost = 35;
    private int poisonArrowMaxCD = 1;
    public int poisonArrowCooldown = 0;

    // -------------------------------------------------------
    // STAFF
    // -------------------------------------------------------
    [Header("Staff — Mana Burst")]
    public int manaBurstCost = 60;
    private int manaBurstMaxCD = 4;
    public int manaBurstCooldown = 0;

    [Header("Staff — Silence")]
    public int staffSilenceCost = 50;
    private int staffSilenceMaxCD = 4;
    public int staffSilenceCooldown = 0;

    [Header("Staff — Scorch")]
    public int scorchCost = 35;
    private int scorchMaxCD = 1;
    public int scorchCooldown = 0;

    // -------------------------------------------------------
    // SWORD SKILLS
    // -------------------------------------------------------
    public void Strike()
    {
        if (PlayerData.currentMana < strikeCost || strikeCooldown > 0) return;
        SoundManager.instance.PlaySFX(SoundManager.instance.skill1Clip);
        PlayerData.currentMana -= strikeCost;
        skipMinigameNextAttack = true;
        strikeCooldown = strikeMaxCD;
        battle.statusUI.SetMessage("Strike ready! Next attack skips minigame for heavy damage.");
        battle.RefreshAfterSkill();
    }

    public void Parry()
    {
        if (PlayerData.currentMana < parryCost || parryCooldown > 0) return;
        SoundManager.instance.PlaySFX(SoundManager.instance.skill2Clip);
        PlayerData.currentMana -= parryCost;
        parryActive = true;
        parryCooldown = parryMaxCD;
        battle.statusUI.SetMessage("Parry! Incoming damage reduced next turn.");
        battle.RefreshAfterSkill();
    }

    public void Rend()
    {
        if (PlayerData.currentMana < rendCost || rendCooldown > 0) return;
        SoundManager.instance.PlaySFX(SoundManager.instance.skill3Clip);
        PlayerData.currentMana -= rendCost;
        rendCooldown = rendMaxCD;
        battle.AddDebuff(DebuffType.Rupture);
        battle.RefreshAfterSkill();
    }

    // -------------------------------------------------------
    // BOW SKILLS
    // -------------------------------------------------------
    public void AimedShot()
    {
        if (PlayerData.currentMana < aimedShotCost || aimedShotCooldown > 0) return;
        SoundManager.instance.PlaySFX(SoundManager.instance.skill1Clip);
        PlayerData.currentMana -= aimedShotCost;
        aimedShotBonus = 5;
        aimedShotCooldown = aimedShotMaxCD;
        battle.statusUI.SetMessage("Aimed Shot! Next attack deals bonus damage per arrow.");
        battle.RefreshAfterSkill();
    }

    public void Trap()
    {
        if (PlayerData.currentMana < trapCost || trapCooldown > 0) return;
        SoundManager.instance.PlaySFX(SoundManager.instance.skill2Clip);
        PlayerData.currentMana -= trapCost;
        trapActive = true;
        trapCooldown = trapMaxCD;
        battle.statusUI.SetMessage("Trap set! Enemy attack will be skipped next turn.");
        battle.RefreshAfterSkill();
    }

    public void PoisonArrow()
    {
        if (PlayerData.currentMana < poisonArrowCost || poisonArrowCooldown > 0) return;
        SoundManager.instance.PlaySFX(SoundManager.instance.skill3Clip);
        PlayerData.currentMana -= poisonArrowCost;
        poisonArrowCooldown = poisonArrowMaxCD;
        battle.AddDebuff(DebuffType.Poison);
        battle.RefreshAfterSkill();
    }

    // -------------------------------------------------------
    // STAFF SKILLS
    // -------------------------------------------------------
    public void ManaBurst()
    {
        if (PlayerData.currentMana < manaBurstCost || manaBurstCooldown > 0) return;
        SoundManager.instance.PlaySFX(SoundManager.instance.skill1Clip);
        int manaSpent = Mathf.Min(PlayerData.currentMana, 50);
        PlayerData.currentMana -= manaSpent;
        manaBurstBonus = manaSpent / 2;
        manaBurstCooldown = manaBurstMaxCD;
        battle.statusUI.SetMessage("Mana Burst! +" + manaBurstBonus + " bonus damage next attack.");
        battle.RefreshAfterSkill();
    }

    public void StaffSilence()
    {
        if (PlayerData.currentMana < staffSilenceCost || staffSilenceCooldown > 0) return;
        SoundManager.instance.PlaySFX(SoundManager.instance.skill2Clip);
        PlayerData.currentMana -= staffSilenceCost;
        staffSilenceCooldown = staffSilenceMaxCD;
        minigame.isSilenced = true;
        battle.monsterData.ApplySilenceText();
        battle.ShowSilenceIcon(true);
        battle.statusUI.SetMessage("Silenced! Enemy abilities suppressed.");
        battle.RefreshAfterSkill();
    }

    public void Scorch()
    {
        if (PlayerData.currentMana < scorchCost || scorchCooldown > 0) return;
        SoundManager.instance.PlaySFX(SoundManager.instance.skill3Clip);
        PlayerData.currentMana -= scorchCost;
        scorchCooldown = scorchMaxCD;
        battle.AddDebuff(DebuffType.Burn);
        battle.RefreshAfterSkill();
    }

    // -------------------------------------------------------
    // HELPERS
    // -------------------------------------------------------
    public void TickCooldowns()
    {
        if (strikeCooldown > 0) strikeCooldown--;
        if (parryCooldown > 0) parryCooldown--;
        if (rendCooldown > 0) rendCooldown--;
        if (aimedShotCooldown > 0) aimedShotCooldown--;
        if (trapCooldown > 0) trapCooldown--;
        if (poisonArrowCooldown > 0) poisonArrowCooldown--;
        if (manaBurstCooldown > 0) manaBurstCooldown--;
        if (staffSilenceCooldown > 0) staffSilenceCooldown--;
        if (scorchCooldown > 0) scorchCooldown--;
    }

    public int ConsumeManaBurstBonus()
    {
        int val = manaBurstBonus;
        manaBurstBonus = 0;
        return val;
    }

    public int ConsumeAimedShotBonus()
    {
        int val = aimedShotBonus;
        aimedShotBonus = 0;
        return val;
    }

    public int GetSkill1Cost()
    {
        switch (WeaponData.equippedWeapon)
        {
            case WeaponType.Sword: return strikeCost;
            case WeaponType.Bow: return aimedShotCost;
            case WeaponType.Staff: return manaBurstCost;
        }
        return 0;
    }

    public int GetSkill2Cost()
    {
        switch (WeaponData.equippedWeapon)
        {
            case WeaponType.Sword: return parryCost;
            case WeaponType.Bow: return trapCost;
            case WeaponType.Staff: return staffSilenceCost;
        }
        return 0;
    }

    public int GetSkill3Cost()
    {
        switch (WeaponData.equippedWeapon)
        {
            case WeaponType.Sword: return rendCost;
            case WeaponType.Bow: return poisonArrowCost;
            case WeaponType.Staff: return scorchCost;
        }
        return 0;
    }

    public int GetSkill1CD()
    {
        switch (WeaponData.equippedWeapon)
        {
            case WeaponType.Sword: return strikeCooldown;
            case WeaponType.Bow: return aimedShotCooldown;
            case WeaponType.Staff: return manaBurstCooldown;
        }
        return 0;
    }

    public int GetSkill2CD()
    {
        switch (WeaponData.equippedWeapon)
        {
            case WeaponType.Sword: return parryCooldown;
            case WeaponType.Bow: return trapCooldown;
            case WeaponType.Staff: return staffSilenceCooldown;
        }
        return 0;
    }

    public int GetSkill3CD()
    {
        switch (WeaponData.equippedWeapon)
        {
            case WeaponType.Sword: return rendCooldown;
            case WeaponType.Bow: return poisonArrowCooldown;
            case WeaponType.Staff: return scorchCooldown;
        }
        return 0;
    }
}