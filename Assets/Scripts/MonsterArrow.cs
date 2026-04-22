//dito ung spawn ng arrow at mga ability

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonsterArrow : MonoBehaviour
{
    public KeyCode correctKey;

    public Image arrowImage;
    public MonsterType arrowType;

    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    private Color originalColor;

    //abilities
    private int requiredHits = 1;
    private int currentHits = 0;

    //private bool isInvisible = false;

    void Awake()
    {
        originalColor = arrowImage.color;
    }

    public void ApplyMonsterEffect(MonsterType type)
    {
        // boss
        if (type == MonsterType.Seraphim)
        {
            type = (MonsterType)Random.Range(0, 6);
        }

        arrowType = type; 

        switch (type)
        {
            case MonsterType.Slime:
                arrowImage.color = Color.green;
                requiredHits = 2;
                break;

            case MonsterType.Spider:
                arrowImage.color = Color.yellow;
                break;

            case MonsterType.Skeleton:
                arrowImage.color = Color.gray;
                break;

            case MonsterType.Wraith:
                arrowImage.color = Color.black;
                StartCoroutine(FadeOut());
                break;

            case MonsterType.Oni:
                arrowImage.color = new Color(1f, 0.5f, 0f); // orange
                break;

            case MonsterType.Seraphim:
                MonsterType random = (MonsterType)Random.Range(0, 6);
                ApplyMonsterEffect(random);
                break;
        }

        originalColor = arrowImage.color;
    }

    public void SetDirection(KeyCode key)  // para sa minigame, lalabas asset, asset = key
    {
        correctKey = key;

        switch (key)
        {
            case KeyCode.UpArrow:
                arrowImage.sprite = upSprite;
                break;
            case KeyCode.DownArrow:
                arrowImage.sprite = downSprite;
                break;
            case KeyCode.LeftArrow:
                arrowImage.sprite = leftSprite;
                break;
            case KeyCode.RightArrow:
                arrowImage.sprite = rightSprite;
                break;
        }
    }

    public int TryInput(KeyCode key)
    {
        if (key == correctKey)
        {
            currentHits++;

            if (currentHits >= requiredHits)
            {
                Destroy(gameObject);
                return 1; 
            }

            return 0;
        }
        else
        {
            StartCoroutine(FlashRed());
            return -1;
        }
    }

    IEnumerator FlashRed() //mali inpout
    {
        arrowImage.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        arrowImage.color = originalColor;
    }

    //abilities
    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(0.5f); // fade transition, change,,

        float duration = 0.5f;
        float elapsed = 0f;

        Color color = arrowImage.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            arrowImage.color = new Color(color.r, color.g, color.b, alpha);

            yield return null;
        }

        arrowImage.color = new Color(color.r, color.g, color.b, 0f);
    }
}