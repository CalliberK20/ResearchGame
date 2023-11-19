using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public Animator torsoAnim;
    [Space]
    public int preBulletCount = 3;
    public GameObject bulletPrefab;
    [Space]
    public List<WeaponStats> weaponStats = new List<WeaponStats>();

    #region Properties
    [Space(30)]
    [ShowOnly] public float weaponDamage;
    [ShowOnly] public float reloadSpeed = 3;
    [ShowOnly] public int ammo = 5;
    [ShowOnly] public float atkRate = 1f;
    [ShowOnly] public float bulletSpeed = 1f;
    [ShowOnly] public float bulletDestroyTime = 1f;
    [Space, ShowOnly]
    public Vector3 strikeArea;
    #endregion


    //-----------PRIVATE----------------------
    private List<GameObject> bullets = new List<GameObject>();
    private List<int> currentAmmos = new List<int>();
    private List<int> maxAmmos = new List<int>();


    private float startDelay;
    private float startReload;

    private float currentWeapon = 0;
    private bool holdAttack = false;
    private bool isMelee = false;

    private Movement movement;
    private bool isReloading = false;

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
        torsoAnim.SetFloat("Type", weaponStats[(int)currentWeapon].weaponAnimatorType);

        for (int i = 0; i < weaponStats.Count; i++)
        {
            int getAmmo = 0;
            if (!weaponStats[i].isMelee)
                getAmmo = weaponStats[i].ammo;
            currentAmmos.Add(getAmmo);
            maxAmmos.Add(getAmmo);
        }
        startDelay = atkRate;

        movement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!movement.isDead)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject bullet = GetActiveBullet();
                if (bullet != null && startDelay >= atkRate && !isReloading)
                {
                    torsoAnim.SetTrigger("Shoot");
                    if (isMelee)
                    {
                        Strike();
                    }
                    else
                        Shoot(bullet);
                }
            }
            
            if(holdAttack && Input.GetMouseButton(0))
            {
                torsoAnim.SetBool("Hold", true);
                if (startDelay >= atkRate)
                {
                    Strike();
                    startDelay = 0;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                torsoAnim.SetBool("Hold", false);
            }

            //------------------HANDLES SWITCHING---------------------------
            if (Mathf.Abs(Input.mouseScrollDelta.y) > 0)
            {
                currentWeapon += Input.mouseScrollDelta.y;

                if (currentWeapon < 0)
                    currentWeapon = weaponStats.Count - 1;
                else if (currentWeapon > weaponStats.Count - 1)
                    currentWeapon = 0;
            }

            if(weaponStats.Count > 0)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    currentWeapon = 0;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    currentWeapon = 1;
                }
            }
            SwitchWeapon();
            //-------------------------------------------------------------

            if (startDelay < atkRate)
                startDelay += Time.deltaTime;


            if (isReloading && maxAmmos[(int)currentWeapon] > 0)
            {
                startReload += Time.deltaTime;
                if(startReload >= reloadSpeed)
                {
                    int bulletToGive = 0;
                    if (maxAmmos[(int)currentWeapon] >= ammo)
                        bulletToGive = ammo;
                    else
                        bulletToGive = maxAmmos[(int)currentWeapon];

                    currentAmmos[(int)currentWeapon] = bulletToGive;
                    maxAmmos[(int)currentWeapon] -= bulletToGive;

                    startReload = 0;
                    isReloading = false;
                }
            }

            Debug.DrawLine(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.red);
        }
    }

    void Shoot(GameObject bullet)
    {
        bullet.SetActive(true);
        bullet.transform.position = transform.position;

        #region Rotation
        //----------For Rotation---------------------
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - bullet.transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z);
        bullet.transform.position = bullet.transform.position + (bullet.transform.right * 0.4f);
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * bulletSpeed;
        //----------For Rotation---------------------
        #endregion

        if(!isMelee)
        {
            currentAmmos[(int)currentWeapon]--;
            UIManager.Instance.ammoText.text = currentAmmos[(int)currentWeapon].ToString() + "/" + maxAmmos[(int)currentWeapon].ToString();
            if (currentAmmos[(int)currentWeapon] <= 0)
            {
                isReloading = true;
            }
        }
        bullet.GetComponent<Bullet>().SetBulletStat(weaponDamage, bulletDestroyTime);
        startDelay = 0;
    }

    void Strike()
    {
        Vector3 offsetArea = Vector2.zero;
        if(movement.isFlip)
            offsetArea = new Vector2(-0.7f, 0);
        else
            offsetArea = new Vector2(0.7f, 0);

        strikeArea = transform.position + offsetArea;

        Collider2D[] strikeZone = Physics2D.OverlapCircleAll(strikeArea, 0.7f, LayerMask.GetMask("Enemy"));
        foreach(Collider2D hit in strikeZone)
        {
            if(hit.GetComponent<EnemyMovement>() && hit.GetComponent<EnemyMovement>().enabled)
            {
                hit.GetComponent<EnemyMovement>().Damage(weaponDamage);
            }
        }
    }

/*    void HoldAttack()
    {
        torsoAnim.SetBool("Hold", true);
    }*/

    private void SwitchWeapon()
    {
        //Change the stats when switching
        ChangeStats();
        UIManager.Instance.ammoText.text = currentAmmos[(int)currentWeapon].ToString() + "/" + maxAmmos[(int)currentWeapon].ToString();
        torsoAnim.SetFloat("Type", weaponStats[(int)currentWeapon].weaponAnimatorType);
        isReloading = false;

        if (!isMelee)
        {
            if (currentAmmos[(int)currentWeapon] <= 0)
            {
                isReloading = true;
            }
        }
    }

    public void GiveNewWeapon(WeaponStats newWeapon)
    {
        if (!weaponStats.Contains(newWeapon))
        {
            weaponStats.Add(newWeapon);

            int getAmmo = 0;
            if (!newWeapon.isMelee)
                getAmmo = newWeapon.ammo;
            currentAmmos.Add(getAmmo);
            maxAmmos.Add(getAmmo);
        }
        else
            Debug.Log("Has the gun");
    }

    public void GetMoreAmmo(int amount)
    {
        maxAmmos[(int)currentWeapon] += amount;
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
        holdAttack = weaponStats[(int)currentWeapon].holdAttack;
        isMelee = weaponStats[(int)currentWeapon].isMelee;

        weaponDamage = weaponStats[(int)currentWeapon].damage;

        if(isMelee)
        {
            atkRate = 0;
            bulletSpeed = 0;

            ammo = 0;

            bulletDestroyTime = 0;
        }
        else
        {
            atkRate = weaponStats[(int)currentWeapon].delayShot;
            bulletSpeed = weaponStats[(int)currentWeapon].bulletSpeed;

            ammo = weaponStats[(int)currentWeapon].ammo;

            bulletDestroyTime = weaponStats[(int)currentWeapon].bulletDestroyTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(strikeArea, 0.5f);
    }
}
