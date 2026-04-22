using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    public string message;

    public BattleStatus statusUI;

    public void OnPointerEnter(PointerEventData eventData)
    {
        statusUI.SetMessage(message);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        statusUI.ClearMessage();
    }
}