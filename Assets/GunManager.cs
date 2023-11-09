using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    pistol,
    chainsaw,
}

public class GunManager : MonoBehaviour
{
    public Animator torsoAnim;
    [Space]
    public int preBulletCount = 3;
    public GameObject bulletPrefab;
    [Space]
    public WeaponStats[] weaponStats;


    [Space(30)]
    [ShowOnly]
    public float weaponDamage;
    [ShowOnly]
    public float delayShot = 1f;
    [ShowOnly]
    public float bulletSpeed = 1f;
    [ShowOnly]
    public float bulletDestroyTime = 1f;
    [Space, ShowOnly]
    public List<GameObject> bullets = new List<GameObject>();
    
    
    //-----------PRIVATE----------------------
    private float startTime;
    private float currentWeapon = 0;
    private bool holdAttack = false;
    private bool isMelee = false;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject parentObj = new GameObject();
        parentObj.name = "Bullet Parent";
        for (int i = 0; i < preBulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.parent = parentObj.transform;
            bullet.SetActive(false);
            bullets.Add(bullet);
        }

        ChangeStats();

        startTime = delayShot;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (GetActiveBullet() != null && startTime >= delayShot)
            {
                torsoAnim.SetTrigger("Shoot");
                if (isMelee)
                {

                }
                else
                    Shoot();
            }
        }
        
        if(Input.GetMouseButtonUp(0))
        {
            torsoAnim.SetBool("Hold", false);
        }

        SwitchWeapon();

        if (startTime < delayShot)
            startTime += Time.deltaTime;

        Debug.DrawLine(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.red);
    }

    void Shoot()
    {
        GameObject bullet = GetActiveBullet();
        bullet.SetActive(true);
        bullet.transform.position = transform.position;

        //----------For Rotation---------------------
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - bullet.transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z);
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * bulletSpeed;
        //----------For Rotation---------------------

        bullet.GetComponent<Bullet>().SetBulletStat(bulletDestroyTime);

        startTime = 0;
    }

/*    void HoldAttack()
    {
        torsoAnim.SetBool("Hold", true);
    }*/

    private void SwitchWeapon()
    {
        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0)
        {
            currentWeapon += Input.mouseScrollDelta.y;

            if (currentWeapon < 0)
                currentWeapon = 1;
            else if (currentWeapon > 1)
                currentWeapon = 0;

            //Change the stats when switching
            ChangeStats();

            torsoAnim.SetFloat("Type", currentWeapon);
        }
    }

    GameObject GetActiveBullet()
    {
        foreach(GameObject bullet in bullets)
        {
            if(!bullet.activeSelf)
                return bullet;
        }
        return null;
    }

    private void ChangeStats()
    {
        isMelee = weaponStats[(int)currentWeapon].isMelee;

        weaponDamage = weaponStats[(int)currentWeapon].damage;

        delayShot = weaponStats[(int)currentWeapon].delayShot;
        bulletSpeed = weaponStats[(int)currentWeapon].bulletSpeed;
        holdAttack = weaponStats[(int)currentWeapon].holdAttack;
    }
}
