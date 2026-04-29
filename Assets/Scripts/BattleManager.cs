// dito lahat ng battle functions

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public GameObject silenceIconObject;
    [SerializeField] private int silenceUsesPerStage = 1; // editable in Inspector
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

    public Button debuffButton;
    public Button[] battleButtons;

    public BattleStatus statusUI;
    public MinigameManager minigame;
    public GameObject minigamePanel;

    public Button healButton;
    public Button meditateButton;
    public Button attackUpButton;
    public Button silenceButton;

    public TextMeshProUGUI monsterHPText;
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI playerManaText;

    public TextMeshProUGUI meditateCDText;
    public TextMeshProUGUI atkUpCDText;
    public TextMeshProUGUI silenceCDText;

    // can change mana cost
    private int healCost = 15;
    private int debuffCost = 25;
    private int attackUpCost = 30;
    private int silenceCost = 35;

    // cd for mana regen
    private int meditateCooldown = 0;
    private int atkUpCooldown = 0;
    private int silenceCooldown = 0;

    private int attackBuff = 0;
    private int attackBuffTurns = 0;

    // start here ---------------------------------------------------------------------------------------
    void Start()
    {
        silenceUsesLeft = silenceUsesPerStage;
        Debug.Log("Enemy ID at start: " + GameData.currentEnemyID);

        UpdateEnemy();

        UpdateAllBars();

        monsterData.SetupMonster(GameData.currentEnemyID);

        // init debuff
        foreach (var slot in debuffSlots)
        {
            slot.debuffType = DebuffType.None;
            slot.stack = 0;

            if (slot.icon != null)
                slot.icon.enabled = false;

            slot.UpdateStackText();
        }

        UpdateDebuffButtonState();
        UpdateSkillButtons();
        UpdateCooldownUI();
    }

    // update enemy ---------------------------------------------------------------------------------------
    void UpdateEnemy() // spawn enemy after encounter
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

    // depends on situation, disable or enable ---------------------------------------------------------------------------------------
    void UpdateSkillButtons()
    {
        if (healButton != null)
            healButton.interactable = PlayerData.currentMana >= healCost;

        if (debuffButton != null)
            debuffButton.interactable = PlayerData.currentMana >= debuffCost;

        if (attackUpButton != null)
            attackUpButton.interactable = PlayerData.currentMana >= attackUpCost && atkUpCooldown == 0;

        if (silenceButton != null)
            silenceButton.interactable = PlayerData.currentMana >= silenceCost && silenceUsesLeft > 0;

        if (meditateButton != null)
            meditateButton.interactable = meditateCooldown == 0;
    }

    // update the bar UIs ---------------------------------------------------------------------------------------
    void UpdateAllBars()
    {
        playerHealthBar.SetMaxHealth(PlayerData.maxHealth);
        playerHealthBar.SetHealth(PlayerData.currentHealth);

        playerManaBar.SetMaxHealth(PlayerData.maxMana);
        playerManaBar.SetHealth(PlayerData.currentMana);

        enemyHealthBar.SetMaxHealth(EnemyData.maxHealth);
        enemyHealthBar.SetHealth(EnemyData.currentHealth);

        // text notifier
        if (playerHPText != null)
            playerHPText.text = PlayerData.currentHealth.ToString();

        if (playerManaText != null)
            playerManaText.text = PlayerData.currentMana.ToString();

        if (monsterHPText != null)
            monsterHPText.text = EnemyData.currentHealth.ToString();
    }

    public void UpdateUITextVisibility()
    {
        bool isMinigameOpen = minigamePanel != null && minigamePanel.activeSelf;

        if (monsterHPText != null)
            monsterHPText.gameObject.SetActive(!isMinigameOpen);

    }

    // cooldowns ---------------------------------------------------------------------------------------
    void UpdateCooldownUI()
    {
        if (meditateCDText != null)
            meditateCDText.text = meditateCooldown > 0 ? meditateCooldown.ToString() : "";

        if (atkUpCDText != null)
            atkUpCDText.text = atkUpCooldown > 0 ? atkUpCooldown.ToString() : "";

        if (silenceCDText != null)
            silenceCDText.text = silenceCooldown > 0 ? silenceCooldown.ToString() : "";
    }

    // call minigame using vutton ---------------------------------------------------------------------------------------
    public void Attack()
    {
        SetBattleButtons(false);

        if (GameState.TutorialMode)
        {
            ResolveAttackTutorial();
            return;
        }

        minigame.StartMinigame(6);
        UpdateUITextVisibility();
    }

    public void Meditate() // mana func ---------------------------------------------------------------------------------------
    {
        if (meditateCooldown > 0)
        {
            statusUI.SetMessage("<color=yellow>Meditate is on cooldown</color>");
            return;
        }

        PlayerData.currentMana += 30; // regen mana ---------------------------------------------------------------------------------------

        meditateCooldown = 1;

        ClampValues();
        UpdateAllBars();
        UpdateDebuffButtonState();
        UpdateSkillButtons();
        UpdateCooldownUI();
    }

    public void Heal() // same here ---------------------------------------------------------------------------------------
    {
        if (PlayerData.currentMana < healCost)
        {
            statusUI.SetMessage("<color=red>Not enough mana</color>");
            return;
        }

        PlayerData.currentMana -= healCost;
        PlayerData.currentHealth += 20;

        ClampValues();
        UpdateAllBars();
        UpdateDebuffButtonState();
        UpdateSkillButtons();
    }

    // debuff skill , but no effect yet ----------------------------------------------------------------------------
    public void ApplyRandomDebuff()
    {
        int manaCost = debuffCost;

        if (PlayerData.currentMana < manaCost)
        {
            if (statusUI != null)
                statusUI.SetMessage("<color=red>You have no mana left</color>");
            return;
        }

        PlayerData.currentMana -= manaCost;
        UpdateAllBars();

        DebuffInfo chosen = debuffDatabase[Random.Range(0, debuffDatabase.Length)];

        // debuff stacking here
        foreach (var slot in debuffSlots)
        {
            if (slot.debuffType == chosen.type)
            {
                slot.stack++;

                if (slot.icon != null)
                {
                    slot.icon.sprite = chosen.icon;
                    slot.icon.enabled = true;
                }

                slot.UpdateStackText();
                UpdateDebuffButtonState();
                return;
            }
        }

        // fill the empty slot with a debff 
        foreach (var slot in debuffSlots)
        {
            if (slot.debuffType == DebuffType.None)
            {
                slot.debuffType = chosen.type;
                slot.stack = 1;

                if (slot.icon != null)
                {
                    slot.icon.sprite = chosen.icon;
                    slot.icon.enabled = true;
                }

                slot.description = chosen.description;

                slot.UpdateStackText();
                UpdateDebuffButtonState();
                return;
            }
        }
    }

    // buff skill ----------------------------------------------------------------------------
    public void AttackUp()
    {
        if (PlayerData.currentMana < attackUpCost || atkUpCooldown > 0)
        {
            statusUI.SetMessage("<color=red>Not ready</color>");
            return;
        }

        PlayerData.currentMana -= attackUpCost;

        attackBuff = 10;
        attackBuffTurns = 2;
        atkUpCooldown = 5;

        statusUI.SetMessage("Attack increased for 2 turns!");

        UpdateAllBars();
        UpdateSkillButtons();
        UpdateCooldownUI();
    }

    // wala pa skill ----------------------------------------------------------------------------
    public void Silence()
    {

        if (PlayerData.currentMana < silenceCost || silenceUsesLeft <= 0) return;

        if (silenceIconObject != null)
            silenceIconObject.SetActive(true);
            silenceIconObject.transform.SetAsFirstSibling();

        PlayerData.currentMana -= silenceCost;
        silenceUsesLeft--;
        minigame.isSilenced = true;
        monsterData.ApplySilenceText();

        statusUI.SetMessage("Silenced! Enemy abilities suppressed");

        ClampValues();
        UpdateAllBars();
        UpdateSkillButtons(); // this already handles the silence button
    }

    // konektado dito ung attack function kanina ^^^^^ up
    public void SetBattleButtons(bool state)
    {
        foreach (Button btn in battleButtons)
        {
            if (btn != null)
                btn.interactable = state;
        }
    }

    void UpdateDebuffButtonState()
    {
        if (debuffButton == null) return;

        bool hasMana = PlayerData.currentMana >= debuffCost;

        ColorBlock colors = debuffButton.colors;
        colors.normalColor = hasMana ? Color.white : Color.gray;
        debuffButton.colors = colors;
    }

    // attack fucntion ----------------------------------------------------------------------------
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

                SceneManager.LoadScene("Game Scene");
                return;
            }

            SceneManager.LoadScene("Tutorial Scene"); // this
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
        int enemyBase = 10;

        int playerDamage = (playerBase + attackBuff) * correctHits;
        int enemyDamage = enemyBase * incorrectHits;

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

        if (playerDamage > 0)
            EnemyData.currentHealth -= playerDamage;
        if (debuffDamage > 0)
            EnemyData.currentHealth -= debuffDamage;
        if (enemyDamage > 0)
            PlayerData.currentHealth -= enemyDamage;

        // cooldown reduce
        if (meditateCooldown > 0) meditateCooldown--;
        if (atkUpCooldown > 0) atkUpCooldown--;
        if (silenceCooldown > 0) silenceCooldown--;

        // buff reduce
        if (attackBuffTurns > 0)
        {
            attackBuffTurns--;

            if (attackBuffTurns == 0)
            {
                attackBuff = 0;
                statusUI.SetMessage("Attack buff faded.");
            }
        }

        if (statusUI != null)
        {
            string msg = "";

            if (playerDamage > 0)
                msg += "You dealt " + (playerDamage + debuffDamage) + " damage!\n";

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
                    SceneManager.LoadScene("Game Scene");
                }
                else
                {
                    SceneManager.LoadScene("Tutorial Scene");
                }

                return;
            }

            SceneManager.LoadScene("Game Scene");
            return;
        }

        if (silenceIconObject != null)
            silenceIconObject.SetActive(false);

        ClampValues();
        UpdateAllBars();
        UpdateSkillButtons();
        UpdateCooldownUI();

        SetBattleButtons(true);
    }


    void ClampValues()
    {
        PlayerData.currentHealth = Mathf.Clamp(PlayerData.currentHealth, 0, PlayerData.maxHealth);
        PlayerData.currentMana = Mathf.Clamp(PlayerData.currentMana, 0, PlayerData.maxMana);
        EnemyData.currentHealth = Mathf.Clamp(EnemyData.currentHealth, 0, EnemyData.maxHealth);
    }

}