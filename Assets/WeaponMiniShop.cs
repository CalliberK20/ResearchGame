using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponMiniShop : MonoBehaviour
{
    public float radius = 3f;
    public WeaponStats weaponToSell;
    [Space]
    public SpriteRenderer gunTypeRender;
    [Space]
    [ShowOnly] public float weaponPrice = 0;
    
    private float bulletCostRate = 0.3f;

    private TextMeshPro priceText;
    private bool hasBought = false;
    private CashManager cashManager;
    private GunManager gunManager;
    public float rate;

    private void Start()
    {
        cashManager = CashManager.instance;
        priceText = transform.GetChild(0).GetComponent<TextMeshPro>();
        gunManager = GameObject.FindWithTag("Player").GetComponent<GunManager>();

        gunTypeRender.sprite = weaponToSell.weaponSprite;
        weaponPrice = weaponToSell.weaponPrice;
        rate = weaponPrice * bulletCostRate;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Player"));
        if(hit != null)
        {
            priceText.gameObject.SetActive(true);

            if (gunManager.WeaponContainInventory(weaponToSell))
            {
                priceText.text = "";
                if (!weaponToSell.isMelee)
                {
                    priceText.text = "Buy Bullets : $" + rate.ToString();
                }
            }
            else
                priceText.text = "Price: $" + weaponPrice.ToString();

            if(Input.GetKeyDown(KeyCode.E))
            {
                BuyWeapon();
            }
        }
        else
            priceText.gameObject.SetActive(false);
    }

    void BuyWeapon()
    {
        float price;

        if (!hasBought)
            price = weaponPrice;
        else
            price = rate;

        if (cashManager.SuffiecientAmount(price))
        {
            cashManager.LoseMoney(price);
            AudioManager.instance.PlayAudio("Buy");
            if (!hasBought)
            {
                gunManager.GiveNewWeapon(weaponToSell);
                hasBought = true;
                return;
            }
            if (!weaponToSell.isMelee)
                gunManager.GetMoreAmmo(weaponToSell.ammo);
        }    
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
