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
    [ShowOnly] public float weaponDamage;
    [ShowOnly] public float atkDelay = 1f;
    [ShowOnly] public float reloadSpeed = 3;
    [ShowOnly] public int ammo = 5;
    [ShowOnly] public float bulletSpeed = 1f;
    [ShowOnly] public float bulletDestroyTime = 1f;
    [Space, ShowOnly]
    public Vector3 strikeArea;
    
    //-----------PRIVATE----------------------
    private List<GameObject> bullets = new List<GameObject>();
    private int[] currentAmmos;


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
        currentAmmos = new int[weaponStats.Length];
        for (int i = 0; i < currentAmmos.Length; i++)
        {
            if (!weaponStats[i].isMelee)
                currentAmmos[i] = weaponStats[i].ammo;
            else
                currentAmmos[i] = 0;
        }
        startDelay = atkDelay;
        
        UIManager.Instance.ammoText.text = currentAmmos[(int)currentWeapon].ToString();
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
                if (bullet != null && startDelay >= atkDelay && !isReloading)
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

            if (Input.GetMouseButtonUp(0))
            {
                //torsoAnim.SetBool("Hold", false);
            }

            //------------------HANDLES SWITCHING---------------------------
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentWeapon = 0;
                ChangeStats();
                torsoAnim.SetFloat("Type", currentWeapon);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentWeapon = 1;
                ChangeStats();
                torsoAnim.SetFloat("Type", currentWeapon);
            }
            SwitchWeapon();
            //-------------------------------------------------------------

            if (startDelay < atkDelay)
                startDelay += Time.deltaTime;


            if (isReloading)
            {
                startReload += Time.deltaTime;
                if(startReload >= reloadSpeed)
                {
                    currentAmmos[(int)currentWeapon] = ammo;
                    UIManager.Instance.ammoText.text = currentAmmos[(int)currentWeapon].ToString();
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
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * bulletSpeed;
        //----------For Rotation---------------------
        #endregion

        if(!isMelee)
        {
            currentAmmos[(int)currentWeapon]--;
            UIManager.Instance.ammoText.text = currentAmmos[(int)currentWeapon].ToString();
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
            if(hit.GetComponent<EnemyMovement>())
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
        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0)
        {
            currentWeapon += Input.mouseScrollDelta.y;

            if (currentWeapon < 0)
                currentWeapon = 1;
            else if (currentWeapon > 1)
                currentWeapon = 0;

            //Change the stats when switching
            ChangeStats();
            UIManager.Instance.ammoText.text = currentAmmos[(int)currentWeapon].ToString();
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

        if(isMelee)
        {
            atkDelay = 0;
            bulletSpeed = 0;
            holdAttack = weaponStats[(int)currentWeapon].holdAttack;

            ammo = 0;

            bulletDestroyTime = 0;
        }
        else
        {
            atkDelay = weaponStats[(int)currentWeapon].delayShot;
            bulletSpeed = weaponStats[(int)currentWeapon].bulletSpeed;
            holdAttack = weaponStats[(int)currentWeapon].holdAttack;

            ammo = weaponStats[(int)currentWeapon].ammo;

            bulletDestroyTime = weaponStats[(int)currentWeapon].bulletDestroyTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(strikeArea, 0.5f);
    }
}
