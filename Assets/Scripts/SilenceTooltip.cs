using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SilenceTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    public string description;
    public BattleStatus statusUI;

    public void OnPointerEnter(PointerEventData eventData)
    {
        statusUI.SetMessage(description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        statusUI.ClearMessage();
    }
}