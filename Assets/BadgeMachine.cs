using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum BadgeType
{
    green,
    blue, 
    yellow
}

[System.Serializable]
public class BadgeSprite
{
    public Sprite machineSprite;
    public Sprite badgeSprite;
}

public class BadgeMachine : MonoBehaviour
{
    public BadgeMachine script;
    public BadgeSprite[] badgeSprites;
    public float pointsPrice = 1000;
    public BadgeType badgeType;
    public bool canBeInteracted = false;
    [Space]
    public float healthToGive = 30f;
    public float speedToGive = 6.0f;
    public float deductiveReloadSpeed = 6.0f;
    public float time = 6f;
    [Space]
    public float refillTime = 3f;

    private float refilling = 0;
    private Movement movement;
    private GunManager gunManager;
    private SpriteRenderer spriteRenderer;
    private TextMeshPro text;

    private bool perkAcquired = false;

    private void OnValidate()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = badgeSprites[(int)badgeType].machineSprite;
    }

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        text = transform.GetChild(0).GetComponent<TextMeshPro>();
        movement = player.GetComponent<Movement>();
        gunManager = player.GetComponent<GunManager>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = badgeSprites[(int)badgeType].machineSprite;
        
        refilling = refillTime;
        text.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            text.text = "Price: " + pointsPrice;
            canBeInteracted = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canBeInteracted= false;
            text.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!perkAcquired && canBeInteracted)
        {
            text.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
                TakeEf();
        }
    }

    private void GreenBadge()
    {
        movement.GiveHealth(healthToGive);
    }

    private void BlueBadge()
    {
        movement.givenSpeed = speedToGive / 100f;
    }

    private void YellowBadge()
    {
        gunManager.givenReloadSpeed = deductiveReloadSpeed / 100f;
    }

    void TakeEf()
    {
        perkAcquired = true;
        text.gameObject.SetActive(false);
        canBeInteracted = false;
        StopAllCoroutines();

        Image image = Instantiate(UIManager.Instance.badgeImage, UIManager.Instance.badgeParent.transform).GetComponent<Image>();
        image.sprite = badgeSprites[(int)badgeType].badgeSprite;
        switch (badgeType)
        {
            case BadgeType.green:
                GreenBadge();
                break;
            case BadgeType.blue:
                BlueBadge();
                break;
            case BadgeType.yellow:
                YellowBadge();
                break;
        }
    }
}
