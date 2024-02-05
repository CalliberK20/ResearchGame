using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBox : MonoBehaviour
{
    public WeaponStats[] weaponStats;
    private SpriteRenderer spriteObj;
    private Animator chestAnim;
    private GunManager gunManager;
    // Start is called before the first frame update
    void Start()
    {
        gunManager = GameObject.FindGameObjectWithTag("Player").GetComponent<GunManager>();

        chestAnim = GetComponent<Animator>();
        spriteObj = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StartCoroutine(Randomize());
        }
    }

    private IEnumerator Randomize()
    {
        chestAnim.SetTrigger("Open");
        while(enabled)
        {
            spriteObj.sprite = weaponStats[Random.Range(0, weaponStats.Length)].weaponSprite;
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    public void GunToGive()
    {
        StopAllCoroutines();
        WeaponStats weapon = weaponStats[Random.Range(0, weaponStats.Length)];
        spriteObj.sprite = weapon.weaponSprite;
        if (!gunManager.weaponStats.Contains(weapon))
            gunManager.GiveNewWeapon(weapon);
    }
}
