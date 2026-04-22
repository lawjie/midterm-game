using UnityEngine;
using TMPro;

public class BattleStatus : MonoBehaviour
{
    public TextMeshProUGUI statusText;

    public void SetMessage(string message)
    {
        statusText.text = message;
    }

    public void ClearMessage()
    {
        statusText.text = "Choose an action...";
    }
}