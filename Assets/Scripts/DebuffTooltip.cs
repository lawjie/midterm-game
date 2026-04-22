using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DebuffTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public DebuffType debuffType = DebuffType.None;
    public int stack = 0;

    [TextArea]
    public string description;

    public BattleStatus statusUI;
    public Image icon;
    public TextMeshProUGUI stackText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        statusUI.SetMessage(description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        statusUI.ClearMessage();
    }

    public void UpdateStackText()
    {
        if (stackText == null) return;

        if (stack > 1)
            stackText.text = stack.ToString();
        else
            stackText.text = "";
    }
}