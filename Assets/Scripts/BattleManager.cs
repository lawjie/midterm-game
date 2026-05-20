using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public GameObject silenceIconObject;
    [SerializeField] private int silenceUsesPerStage = 1;
    private int silenceUsesLeft;

    public string gameOverSceneName = "Game Over";

    public HealthBar playerHealthBar;
    public HealthBar playerManaBar;
    public HealthBar enemyHealthBar;

    public SpriteRenderer enemyRenderer;
    public EnemySpawn enemySpawn;
    public MonsterDataManager monsterData;

    public DebuffTooltip[] debuffSlots;
    public DebuffInfo[] debuffDatabase;

    public BattleStatus statusUI;
    public MinigameManager minigame;
    public GameObject minigamePanel;

    public WeaponSkills weaponSkills;

    // keep heal and meditate
    public Button healButton;
    public Button meditateButton;

    // weapon skill buttons (replaces attackUp, silence, debuff)
    public Button skill1Button;
    public Button skill2Button;
    public Button skill3Button;

    public TextMeshProUGUI skill1NameText;
    public TextMeshProUGUI skill2NameText;
    public TextMeshProUGUI skill3NameText;

    [Header("Skill Icons")]
    public Image skill1Icon;
    public Image skill2Icon;
    public Image skill3Icon;

    [Header("Sword Sprites")]
    public Sprite strikeSprite;
    public Sprite parrySprite;
    public Sprite rendSprite;

    [Header("Bow Sprites")]
    public Sprite aimedShotSprite;
    public Sprite trapSprite;
    public Sprite poisonArrowSprite;

    [Header("Staff Sprites")]
    public Sprite manaBurstSprite;
    public Sprite silenceSprite;
    public Sprite scorchSprite;

    [Header("Damage")]
    public int normalEnemyBaseDamage = 10;
    public int bossEnemyBaseDamage = 25;

    public Button[] battleButtons;

    public TextMeshProUGUI monsterHPText;
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI playerManaText;

    public TextMeshProUGUI meditateCDText;
    public TextMeshProUGUI skill1CDText;
    public TextMeshProUGUI skill2CDText;
    public TextMeshProUGUI skill3CDText;

    private int healCost = 15;
    private int meditateCooldown = 0;

    private int healCooldown = 0;
    public TextMeshProUGUI healCDText;

    [Header("Victory")]
    public string finalBossID = "iris_1";
    public string victorySceneName = "Victory Scene";

    void Start()
    {
        SoundManager.instance.PlayBGM(SoundManager.instance.battleSceneBGM);
        silenceUsesLeft = silenceUsesPerStage;
        Debug.Log("Enemy ID at start: " + GameData.currentEnemyID);

        UpdateEnemy();
        UpdateAllBars();
        monsterData.SetupMonster(GameData.currentEnemyID);

        foreach (var slot in debuffSlots)
        {
            slot.debuffType = DebuffType.None;
            slot.stack = 0;
            if (slot.icon != null) slot.icon.enabled = false;
            slot.UpdateStackText();
        }

        weaponSkills.battle = this;
        weaponSkills.minigame = minigame;
        SetupWeaponUI();

        UpdateSkillButtons();
        UpdateCooldownUI();
    }

    void UpdateEnemy()
    {
        if (enemySpawn == null || enemyRenderer == null)
        {
            Debug.LogError("Missing enemy references!");
            return;
        }

        string id = GameData.currentEnemyID;
        Debug.Log("Battle enemy: " + id);

        EnemySpawnData data = enemySpawn.GetEnemyData(id);

        if (data != null)
        {
            enemyRenderer.sprite = data.sprite;
            EnemyData.maxHealth = data.maxHealth;
            EnemyData.currentHealth = data.maxHealth;
        }
        else
        {
            Debug.LogError("Enemy NOT FOUND: " + id);
        }
    }

    void SetupWeaponUI()
    {
        skill1Button.onClick.RemoveAllListeners();
        skill2Button.onClick.RemoveAllListeners();
        skill3Button.onClick.RemoveAllListeners();

        switch (WeaponData.equippedWeapon)
        {
            case WeaponType.Sword:
                if (skill1NameText != null) skill1NameText.text = "Strike";
                if (skill2NameText != null) skill2NameText.text = "Parry";
                if (skill3NameText != null) skill3NameText.text = "Rend";
                if (skill1Icon != null) skill1Icon.sprite = strikeSprite;
                if (skill2Icon != null) skill2Icon.sprite = parrySprite;
                if (skill3Icon != null) skill3Icon.sprite = rendSprite;
                skill1Button.onClick.AddListener(weaponSkills.Strike);
                skill2Button.onClick.AddListener(weaponSkills.Parry);
                skill3Button.onClick.AddListener(weaponSkills.Rend);
                break;

            case WeaponType.Bow:
                if (skill1NameText != null) skill1NameText.text = "Aimed Shot";
                if (skill2NameText != null) skill2NameText.text = "Trap";
                if (skill3NameText != null) skill3NameText.text = "Poison Arrow";
                if (skill1Icon != null) skill1Icon.sprite = aimedShotSprite;
                if (skill2Icon != null) skill2Icon.sprite = trapSprite;
                if (skill3Icon != null) skill3Icon.sprite = poisonArrowSprite;
                skill1Button.onClick.AddListener(weaponSkills.AimedShot);
                skill2Button.onClick.AddListener(weaponSkills.Trap);
                skill3Button.onClick.AddListener(weaponSkills.PoisonArrow);
                break;

            case WeaponType.Staff:
                if (skill1NameText != null) skill1NameText.text = "Mana Burst";
                if (skill2NameText != null) skill2NameText.text = "Silence";
                if (skill3NameText != null) skill3NameText.text = "Scorch";
                if (skill1Icon != null) skill1Icon.sprite = manaBurstSprite;
                if (skill2Icon != null) skill2Icon.sprite = silenceSprite;
                if (skill3Icon != null) skill3Icon.sprite = scorchSprite;
                skill1Button.onClick.AddListener(weaponSkills.ManaBurst);
                skill2Button.onClick.AddListener(weaponSkills.StaffSilence);
                skill3Button.onClick.AddListener(weaponSkills.Scorch);
                break;
        }
    }

    void UpdateSkillButtons()
    {
        if (healButton != null)
            healButton.interactable = PlayerData.currentMana >= healCost && healCooldown == 0;

        if (meditateButton != null)
            meditateButton.interactable = meditateCooldown == 0;

        if (skill1Button != null)
            skill1Button.interactable = PlayerData.currentMana >= weaponSkills.GetSkill1Cost() && weaponSkills.GetSkill1CD() == 0;

        if (skill2Button != null)
            skill2Button.interactable = PlayerData.currentMana >= weaponSkills.GetSkill2Cost() && weaponSkills.GetSkill2CD() == 0;

        if (skill3Button != null)
            skill3Button.interactable = PlayerData.currentMana >= weaponSkills.GetSkill3Cost() && weaponSkills.GetSkill3CD() == 0;
    }

    void UpdateAllBars()
    {
        playerHealthBar.SetMaxHealth(PlayerData.maxHealth);
        playerHealthBar.SetHealth(PlayerData.currentHealth);

        playerManaBar.SetMaxHealth(PlayerData.maxMana);
        playerManaBar.SetHealth(PlayerData.currentMana);

        enemyHealthBar.SetMaxHealth(EnemyData.maxHealth);
        enemyHealthBar.SetHealth(EnemyData.currentHealth);

        if (playerHPText != null) playerHPText.text = PlayerData.currentHealth.ToString();
        if (playerManaText != null) playerManaText.text = PlayerData.currentMana.ToString();
        if (monsterHPText != null) monsterHPText.text = EnemyData.currentHealth.ToString();
    }

    public void UpdateUITextVisibility()
    {
        bool isMinigameOpen = minigamePanel != null && minigamePanel.activeSelf;
        if (monsterHPText != null)
            monsterHPText.gameObject.SetActive(!isMinigameOpen);
    }

    void UpdateCooldownUI()
    {
        if (meditateCDText != null)
            meditateCDText.text = meditateCooldown > 0 ? meditateCooldown.ToString() : "";

        if (healCDText != null)
            healCDText.text = healCooldown > 0 ? healCooldown.ToString() : "";

        if (skill1CDText != null)
            skill1CDText.text = weaponSkills.GetSkill1CD() > 0 ? weaponSkills.GetSkill1CD().ToString() : "";

        if (skill2CDText != null)
            skill2CDText.text = weaponSkills.GetSkill2CD() > 0 ? weaponSkills.GetSkill2CD().ToString() : "";

        if (skill3CDText != null)
            skill3CDText.text = weaponSkills.GetSkill3CD() > 0 ? weaponSkills.GetSkill3CD().ToString() : "";
    }

    public void Attack()
    {
        SetBattleButtons(false);

        if (GameState.TutorialMode)
        {
            ResolveAttackTutorial();
            return;
        }

        if (weaponSkills.skipMinigameNextAttack)
        {
            weaponSkills.skipMinigameNextAttack = false;
            ResolveAttack(6, 0);
            return;
        }

        minigame.StartMinigame(6);
        UpdateUITextVisibility();
    }

    public void Meditate()
    {
        if (meditateCooldown > 0)
        {
            statusUI.SetMessage("<color=yellow>Meditate is on cooldown</color>");
            return;
        }

        PlayerData.currentMana += 30;
        meditateCooldown = 1;

        ClampValues();
        UpdateAllBars();
        UpdateSkillButtons();
        UpdateCooldownUI();
    }

    public void Heal()
    {
        if (PlayerData.currentMana < healCost)
        {
            statusUI.SetMessage("<color=red>Not enough mana</color>");
            return;
        }

        if (healCooldown > 0)
        {
            statusUI.SetMessage("<color=yellow>Heal is on cooldown</color>");
            return;
        }

        PlayerData.currentMana -= healCost;
        PlayerData.currentHealth += 35;
        healCooldown = 2;

        ClampValues();
        UpdateAllBars();
        UpdateSkillButtons();
        UpdateCooldownUI();
    }

    public void RefreshAfterSkill()
    {
        ClampValues();
        UpdateAllBars();
        UpdateSkillButtons();
        UpdateCooldownUI();
    }

    public void AddDebuff(DebuffType type)
    {
        DebuffInfo chosen = System.Array.Find(debuffDatabase, d => d.type == type);
        if (chosen == null) return;

        foreach (var slot in debuffSlots)
        {
            if (slot.debuffType == type)
            {
                slot.stack++;
                slot.icon.sprite = chosen.icon;
                slot.icon.enabled = true;
                slot.UpdateStackText();
                return;
            }
        }

        foreach (var slot in debuffSlots)
        {
            if (slot.debuffType == DebuffType.None)
            {
                slot.debuffType = type;
                slot.stack = 1;
                slot.icon.sprite = chosen.icon;
                slot.icon.enabled = true;
                slot.description = chosen.description;
                slot.UpdateStackText();
                return;
            }
        }
    }

    public void ShowSilenceIcon(bool state)
    {
        if (silenceIconObject != null)
            silenceIconObject.SetActive(state);
    }

    public void SetBattleButtons(bool state)
    {
        foreach (Button btn in battleButtons)
        {
            if (btn != null)
                btn.interactable = state;
        }
    }

    void ResolveAttackTutorial()
    {
        int fixedDamage = 999;
        EnemyData.currentHealth -= fixedDamage;
        statusUI.SetMessage("<color=yellow>DEV HIT: 999 DAMAGE!</color>");

        if (EnemyData.currentHealth <= 0)
        {
            GameData.tutorialEnemiesDefeated++;
            Debug.Log("kill count: " + GameData.tutorialEnemiesDefeated);
            GameData.defeatedEnemies.Add(GameData.currentEnemyID);

            if (GameData.tutorialEnemiesDefeated >= GameData.tutorialEnemyGoal)
            {
                GameState.TutorialMode = false;
                GameData.justFinishedTutorial = true;
                SceneManager.LoadScene("Equipment Scene");
                return;
            }

            SceneManager.LoadScene("Tutorial Scene");
            return;
        }

        ClampValues();
        UpdateAllBars();
        UpdateSkillButtons();
        UpdateCooldownUI();
        SetBattleButtons(true);
    }

    public void ResolveAttack(int correctHits, int incorrectHits)
    {
        int playerBase = 5;
        int enemyBase = GetEnemyBaseDamage();

        int playerDamage = playerBase * correctHits;
        int enemyDamage = enemyBase * incorrectHits;

        // weapon bonuses
        int weaponBonus = weaponSkills.ConsumeManaBurstBonus()
                        + (weaponSkills.ConsumeAimedShotBonus() * correctHits);

        // parry
        if (weaponSkills.parryActive)
        {
            enemyDamage /= 2;
            weaponSkills.parryActive = false;
        }

        // trap
        if (weaponSkills.trapActive)
        {
            enemyDamage = 0;
            weaponSkills.trapActive = false;
        }

        // debuff damage
        int debuffDamage = 0;
        foreach (var slot in debuffSlots)
        {
            switch (slot.debuffType)
            {
                case DebuffType.Burn:
                    debuffDamage += slot.stack * 2;
                    break;
                case DebuffType.Poison:
                    debuffDamage += slot.stack * (correctHits * 2);
                    break;
                case DebuffType.Rupture:
                    debuffDamage += 2 * correctHits;
                    break;
            }
        }

        if (playerDamage > 0) EnemyData.currentHealth -= playerDamage;
        if (weaponBonus > 0) EnemyData.currentHealth -= weaponBonus;
        if (debuffDamage > 0) EnemyData.currentHealth -= debuffDamage;
        if (enemyDamage > 0) PlayerData.currentHealth -= enemyDamage;

        // tick cooldowns
        if (meditateCooldown > 0) meditateCooldown--;
        if (healCooldown > 0) healCooldown--;
        weaponSkills.TickCooldowns();

        if (silenceIconObject != null)
            silenceIconObject.SetActive(false);

        if (statusUI != null)
        {
            string msg = "";
            if (playerDamage + weaponBonus + debuffDamage > 0)
                msg += "You dealt " + (playerDamage + weaponBonus + debuffDamage) + " damage!\n";
            if (enemyDamage > 0)
                msg += "<color=red>Enemy dealt " + enemyDamage + " damage!</color>";
            statusUI.SetMessage(msg);
        }

        if (PlayerData.currentHealth <= 0)
        {
            SceneManager.LoadScene(gameOverSceneName);
            return;
        }

        if (EnemyData.currentHealth <= 0)
        {
            GameData.defeatedEnemies.Add(GameData.currentEnemyID);

            if (GameState.TutorialMode)
            {
                GameData.tutorialEnemiesDefeated++;
                Debug.Log("Tutorial Kills: " + GameData.tutorialEnemiesDefeated);

                if (GameData.tutorialEnemiesDefeated >= GameData.tutorialEnemyGoal)
                {
                    GameState.TutorialMode = false;
                    SceneManager.LoadScene("Equipment Scene");
                }
                else
                {
                    SceneManager.LoadScene("Tutorial Scene");
                }
                return;
            }

            // check if final boss
            if (GameData.currentEnemyID.Contains(finalBossID))
            {
                SoundManager.instance.PlayBGM(SoundManager.instance.victoryBGM);
                SceneManager.LoadScene(victorySceneName);
                return;
            }

            SceneManager.LoadScene("Game Scene");
            return;
        }

        ClampValues();
        UpdateAllBars();
        UpdateSkillButtons();
        UpdateCooldownUI();
        SetBattleButtons(true);
    }
    int GetEnemyBaseDamage()
    {
        if (GameData.currentEnemyID.Contains(finalBossID))
            return bossEnemyBaseDamage;

        return normalEnemyBaseDamage;
    }
    void ClampValues()
    {
        PlayerData.currentHealth = Mathf.Clamp(PlayerData.currentHealth, 0, PlayerData.maxHealth);
        PlayerData.currentMana = Mathf.Clamp(PlayerData.currentMana, 0, PlayerData.maxMana);
        EnemyData.currentHealth = Mathf.Clamp(EnemyData.currentHealth, 0, EnemyData.maxHealth);
    }
}