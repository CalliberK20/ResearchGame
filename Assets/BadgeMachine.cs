using System.Collections;
using System.Collections.Generic;
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
    public BadgeType badgeType;
    public bool canBeInteracted = false;
    [Space]
    public float healthToGive = 30f;
    public float speedToGive = 6.0f;
    public float reloadSpdToGive = 6.0f;
    public float time = 6f;
    [Space]
    public float refillTime = 3f;

    private float refilling = 0;
    private Movement movement;
    private GunManager gunManager;
    private SpriteRenderer spriteRenderer;
    private Image badgeSource;

    private void OnValidate()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = badgeSprites[(int)badgeType].machineSprite;
    }

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        movement = player.GetComponent<Movement>();
        gunManager = player.GetComponent<GunManager>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = badgeSprites[(int)badgeType].machineSprite;

        badgeSource = UIManager.Instance.badgeImage;

        refilling = refillTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            canBeInteracted = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canBeInteracted= false;
        }
    }

    private void Update()
    {
        if (refilling >= refillTime && canBeInteracted && !badgeSource.gameObject.activeInHierarchy)
        {
            if(Input.GetKeyDown(KeyCode.E))
            TakeEf();
        }

        refilling += Time.deltaTime;
    }

    private IEnumerator GreenBadge()
    {
        movement.GiveHealth(healthToGive);
        yield return new WaitForSeconds(time);
        badgeSource.gameObject.SetActive(false);
    }

    private IEnumerator BlueBadge()
    {
        movement.givenSpeed = speedToGive /100f;
        float spilt = time / 2;
        yield return new WaitForSeconds(spilt);

        int i = 10;
        while (i > 0)
        {
            yield return new WaitForSeconds(spilt / 10);
            UIManager.Instance.badgeImage.gameObject.SetActive(true);
            yield return new WaitForSeconds(spilt / 10);
            UIManager.Instance.badgeImage.gameObject.SetActive(false);
            i--;
        }

        movement.givenSpeed = 0;
    }

    private IEnumerator YellowBadge()
    {
        gunManager.givenReloadSpeed = reloadSpdToGive / 100f;
        float spilt = time / 2;
        Debug.Log(spilt);
        yield return new WaitForSeconds(spilt);

        int i = 10;
        while(i > 0)
        {
            yield return new WaitForSeconds(spilt / 10);
            badgeSource.gameObject.SetActive(true);
            yield return new WaitForSeconds(spilt / 10);
            badgeSource.gameObject.SetActive(false);
            i--;
        }

        gunManager.givenReloadSpeed = 0;
    }

    void TakeEf()
    {
        canBeInteracted = false;
        StopAllCoroutines();

        badgeSource.gameObject.SetActive(true);
        badgeSource.sprite = badgeSprites[(int)badgeType].badgeSprite;
        switch (badgeType)
        {
            case BadgeType.green:
                StartCoroutine(GreenBadge());
                break;
            case BadgeType.blue:
                StartCoroutine(BlueBadge());
                break;
            case BadgeType.yellow:
                StartCoroutine(YellowBadge());
                break;
        }
    }
}
