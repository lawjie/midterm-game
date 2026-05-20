using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EquipmentManager : MonoBehaviour
{
    public Button swordButton;
    public Button bowButton;
    public Button staffButton;
    public Button confirmButton;

    public TextMeshProUGUI descriptionText;

    private string swordDesc = "Sword\n\nStrike (30 mana) — Deal heavy damage, skip minigame\nParry (20 mana) — Reduce next incoming damage\nRend (35 mana) — Apply Rupture stacks";
    private string bowDesc = "Bow\n\nAimed Shot (35 mana) — Bonus damage per correct arrow\nTrap (30 mana) — Skip enemy attack once\nPoison Arrow (35 mana) — Apply Poison stacks";
    private string staffDesc = "Staff\n\nMana Burst (60 mana) — Use mana for bonus damage\nSilence (50 mana) — Suppress enemy ability\nScorch (35 mana) — Apply Burn stacks";

    void Start()
    {
        descriptionText.text = "Pick a weapon to continue.";

        swordButton.onClick.AddListener(() => PreviewWeapon(WeaponType.Sword));
        bowButton.onClick.AddListener(() => PreviewWeapon(WeaponType.Bow));
        staffButton.onClick.AddListener(() => PreviewWeapon(WeaponType.Staff));
        confirmButton.onClick.AddListener(ConfirmSelection);
    }

    void PreviewWeapon(WeaponType type)
    {
        WeaponData.equippedWeapon = type;

        switch (type)
        {
            case WeaponType.Sword: descriptionText.text = swordDesc; break;
            case WeaponType.Bow: descriptionText.text = bowDesc; break;
            case WeaponType.Staff: descriptionText.text = staffDesc; break;
        }
    }

    public void ConfirmSelection()
    {
        if (WeaponData.equippedWeapon == WeaponType.None)
        {
            descriptionText.text = "<color=red>Please select a weapon first!</color>";
            return;
        }

        SceneManager.LoadScene("Game Scene");
    }
}