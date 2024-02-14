using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GunStatPickUp : MonoBehaviour
{
    [HideInInspector]
    public WeaponStats weapon;
    [HideInInspector]
    public int currentAmmo;
    [HideInInspector]
    public int maxAmmo;

    public GameObject statBox;
    public TextMeshProUGUI gunNameText;
    public TextMeshProUGUI gunAtk;
    public TextMeshProUGUI gunAtkSpd;
    private GunManager gunManager;
    private SpriteRenderer spriteRenderer;

    private bool canBeInteracted;
    private void Start()
    {
        gunManager = FindObjectOfType<GunManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = weapon.weaponSprite;
        Destroy(gameObject, 20f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canBeInteracted = true;
            statBox.SetActive(true);
            gunNameText.text = weapon.name;
            gunAtk.text = weapon.damage.ToString();
            gunAtkSpd.text = weapon.atkRate.ToString();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canBeInteracted = false;
            statBox.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetColor();

        if (canBeInteracted && !gunManager.WeaponContainInventory(weapon) && Input.GetKeyDown(KeyCode.E))
        {
            gunManager.GiveNewWeapon(weapon, currentAmmo);
            Destroy(gameObject);
        }
    }

    void SetColor()
    {
        WeaponStats currentWeapon = gunManager.currentWeaponOnHold.weapon;

        if (weapon.damage > currentWeapon.damage)
            gunAtk.color = Color.green;
        else if (weapon.damage == currentWeapon.damage)
            gunAtk.color = Color.white;
        else if(weapon.damage < currentWeapon.damage)
            gunAtk.color = Color.red;

        if (weapon.atkRate > currentWeapon.atkRate)
            gunAtkSpd.color = Color.green;
        else if (weapon.atkRate == currentWeapon.atkRate)
            gunAtkSpd.color = Color.white;
        else if(weapon.atkRate < currentWeapon.atkRate)
            gunAtkSpd.color = Color.red;
    }
}
