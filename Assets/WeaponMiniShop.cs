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

    private void Start()
    {
        cashManager = CashManager.instance;
        priceText = transform.GetChild(0).GetComponent<TextMeshPro>();
        gunTypeRender.sprite = weaponToSell.weaponSprite;
        weaponPrice = weaponToSell.weaponPrice;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Player"));
        float rate = weaponPrice * bulletCostRate;
        if(hit != null && hit.CompareTag("Player"))
        {
            priceText.gameObject.SetActive(true);

            GunManager gunManager = hit.GetComponent<GunManager>();

            if (gunManager.WeaponContainInventory(weaponToSell))
            {
                hasBought = true;
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
                if (hasBought)
                {
                    if (!weaponToSell.isMelee && cashManager.SuffiecientAmount(rate))
                    {
                        gunManager.GetMoreAmmo(weaponToSell.ammo);
                        Debug.Log("Buy Ammo");
                        cashManager.LoseMoney(rate);
                    }
                }
                else
                {
                    if(cashManager.SuffiecientAmount(weaponPrice))
                    {
                        gunManager.GiveNewWeapon(weaponToSell);
                        Debug.Log("Buy Gun");
                        cashManager.LoseMoney(weaponPrice);
                    }
                }
            }
        }
        else
            priceText.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
