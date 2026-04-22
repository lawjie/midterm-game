using UnityEngine;

public class DoorUnlock : MonoBehaviour
{
    public string requiredEnemyID;

    public Collider2D col;
    public SpriteRenderer spriteRenderer;

    public Sprite closedSprite;
    public Sprite openSprite;

    void Start()
    {
        CheckDoorState();
    }

    void CheckDoorState()
    {
        if (GameData.defeatedEnemies.Contains(requiredEnemyID))
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    void OpenDoor()
    {
        if (col != null)
            col.enabled = false;

        if (spriteRenderer != null && openSprite != null)
            spriteRenderer.sprite = openSprite;
    }

    void CloseDoor()
    {
        if (col != null)
            col.enabled = true;

        if (spriteRenderer != null && closedSprite != null)
            spriteRenderer.sprite = closedSprite;
    }
}