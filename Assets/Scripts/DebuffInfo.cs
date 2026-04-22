using UnityEngine;

[System.Serializable]
public class DebuffInfo
{
    public DebuffType type;
    public Sprite icon;

    [TextArea]
    public string description;
}

//debuff information, pede edit sa inspector