using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponMiniShop : MonoBehaviour
{
    public float radius = 3f;
    public WeaponStats weaponToSell;
    [Space]
    [ShowOnly] public float weaponPrice = 0;

    private TextMeshPro priceText;
    private bool hasBought = false;

    private void Start()
    {
        priceText = transform.GetChild(0).GetComponent<TextMeshPro>();
        weaponPrice = weaponToSell.weaponPrice;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Player"));
        if(hit != null && hit.CompareTag("Player"))
        {
            priceText.gameObject.SetActive(true);
            if (hit.GetComponent<GunManager>().weaponStats.Contains(weaponToSell))
            {
                hasBought = true;
                if (!weaponToSell.isMelee)
                {
                    priceText.text = "Buy Bullets";
                }
            }
            else
                priceText.text = "Price: $" + weaponPrice.ToString();

            if(Input.GetKeyDown(KeyCode.E))
            {
                if (hasBought)
                    Debug.Log("Buy Ammo");
                else
                {
                    hit.GetComponent<GunManager>().GiveNewWeapon(weaponToSell);
                    Debug.Log("Buy Gun");
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
