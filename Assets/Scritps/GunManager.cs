using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Weapon
{
    public WeaponStats weapon;
    public int currentAmmo;
    public int maxAmmo;

    public Weapon() { }

    public Weapon(WeaponStats weapon, int currentAmmo, int maxAmmo)
    {
        this.weapon = weapon;
        this.currentAmmo = currentAmmo;
        this.maxAmmo = maxAmmo;
    }
}

public class GunManager : MonoBehaviour
{
    public GameObject weaponDropPrefab;
    [Space]
    public Image reloadImage;
    public Animator torsoAnim;
    [Space]
    public int preBulletCount = 3;
    public GameObject bulletPrefab;
    [Space]
    //public List<WeaponStats> weaponStats = new List<WeaponStats>();
    public Weapon currentWeaponOnHold;
    public List<Weapon> weaponInventory = new List<Weapon>();

    #region Properties
    [Space(30)]
    [ShowOnly] public float weaponDamage;
    [ShowOnly] public float atkRate = 1f;
    [ShowOnly] public float reloadSpeed = 3;
    [ShowOnly] public int ammo = 5;
    [ShowOnly] public float bulletSpeed = 1f;
    [ShowOnly] public float bulletDestroyTime = 1f;
    [Space, ShowOnly]
    public Vector3 strikeArea;
    #endregion


    //-----------PRIVATE----------------------
    private List<GameObject> bullets = new List<GameObject>();

    [HideInInspector]
    public float givenReloadSpeed = 0f;

    private float startDelay;
    private float startReload;

    private float currentWeapon = 0;
    private bool holdAttack = false;
    private bool isMelee = false;

    private Movement movement;
    private bool isReloading = false;

    private AudioSource audioSource;
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

        currentWeaponOnHold = weaponInventory[(int)currentWeapon];
        ChangeStats();
        torsoAnim.SetFloat("Type", currentWeaponOnHold.weapon.weaponAnimatorType);

        startDelay = atkRate;

        movement = GetComponent<Movement>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movement.isDead)
            return;

        Sound sound = AudioManager.instance.GetAudio(currentWeaponOnHold.weapon.weaponAudioName);

        if (Input.GetMouseButtonDown(0))
        {
            if(isReloading)
            {
                Sound emptyGun = AudioManager.instance.GetAudio("Empty");
                audioSource.clip = emptyGun.clip;
                audioSource.loop = emptyGun.loop;
                audioSource.Play();
            }
        }

        if (Input.GetMouseButton(0))
        {
            GameObject bullet = GetActiveBullet();
            if(currentWeaponOnHold.weapon.holdAttack)
            {
                Debug.Log("Running Chainsaw");
                torsoAnim.SetTrigger("Shoot");
                torsoAnim.SetBool("Hold", true);
                if (startDelay >= atkRate)
                {
                    Strike();
                }
                audioSource.loop = sound.loop;
                audioSource.clip = sound.clip;
                if (!audioSource.isPlaying)
                    audioSource.Play();
            }
            else if (bullet != null && startDelay >= atkRate && !isReloading)
            {
                torsoAnim.SetTrigger("Shoot");
                if (isMelee)
                {
                    Strike();
                }
                else
                    Shoot(bullet);

                audioSource.clip = sound.clip;
                audioSource.loop = sound.loop;
                audioSource.Play();
                startDelay = 0;
                
            }
        }

        if (holdAttack && Input.GetMouseButtonUp(0))
        {
            torsoAnim.SetBool("Hold", false);
            if (audioSource.isPlaying && sound.loop)
                audioSource.Stop();
            startDelay = 0;
        }

        if(weaponInventory.Count > 1 && Input.GetKeyDown(KeyCode.Q))
        {
            GameObject dropGun = Instantiate(weaponDropPrefab, transform.position - new Vector3(0, -0.4f), Quaternion.identity);
            GunStatPickUp gunStatPickUp = dropGun.GetComponent<GunStatPickUp>();
            gunStatPickUp.weapon = currentWeaponOnHold.weapon;
            gunStatPickUp.currentAmmo = currentWeaponOnHold.currentAmmo;
            gunStatPickUp.maxAmmo = currentWeaponOnHold.maxAmmo;
            weaponInventory.Remove(currentWeaponOnHold);

            if (currentWeapon >= weaponInventory.Count)
                currentWeapon--;
        }


        //------------------HANDLES SWITCHING---------------------------
        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0)
        {
            currentWeapon -= Input.mouseScrollDelta.y;

            if (currentWeapon < 0)
                currentWeapon = weaponInventory.Count - 1;
            else if (currentWeapon > weaponInventory.Count - 1)
                currentWeapon = 0;
        }

        if (weaponInventory.Count > 0)
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

        if (isReloading && currentWeaponOnHold.maxAmmo > 0)
        {
            reloadImage.gameObject.SetActive(true);
            startReload += Time.deltaTime;
            float converReloadSpeed = reloadSpeed - givenReloadSpeed;
            float convert = startReload / converReloadSpeed;
            reloadImage.fillAmount = convert;
            if (startReload >= converReloadSpeed)
            {
                int bulletToGive;
                if (currentWeaponOnHold.maxAmmo >= ammo)
                    bulletToGive = ammo;
                else
                    bulletToGive = currentWeaponOnHold.maxAmmo;

                currentWeaponOnHold.currentAmmo = bulletToGive;
                currentWeaponOnHold.maxAmmo -= bulletToGive;

                startReload = 0;
                isReloading = false;
                reloadImage.fillAmount = 0;
                reloadImage.gameObject.SetActive(false);
            }
        }

        Debug.DrawLine(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.red);
    
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
            currentWeaponOnHold.currentAmmo--;
            UIManager.Instance.ammoText.text = currentWeaponOnHold.currentAmmo.ToString() + "/" + currentWeaponOnHold.maxAmmo.ToString();
            if (currentWeaponOnHold.currentAmmo <= 0)
            {
                isReloading = true;
            }
        }
        bullet.GetComponent<Bullet>().SetBulletStat(weaponDamage, bulletDestroyTime);
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

    private void SwitchWeapon()
    {
        //Change the stats when switching
        currentWeaponOnHold = weaponInventory[(int)currentWeapon];
        ChangeStats();
        UIManager.Instance.ammoText.text = currentWeaponOnHold.currentAmmo.ToString() + "/" + currentWeaponOnHold.maxAmmo.ToString();
        torsoAnim.SetFloat("Type", currentWeaponOnHold.weapon.weaponAnimatorType);
        isReloading = false;

        if (!isMelee)
        {
            if (currentWeaponOnHold.currentAmmo <= 0)
            {
                isReloading = true;
            }
        }
    }

    public void GiveNewWeapon(WeaponStats newWeapon)
    {
        Weapon weapon = new Weapon(newWeapon, newWeapon.ammo, newWeapon.ammo);
        if (!WeaponContainInventory(newWeapon))
        {
            weaponInventory.Add(weapon);
        }
        else
            Debug.Log("Has the gun");

        currentWeapon = weaponInventory.Count - 1;
    }

    public void GiveNewWeapon(WeaponStats newWeapon, int currentAmmo)
    {
        Weapon weapon = new Weapon(newWeapon, currentAmmo, newWeapon.ammo);
        if (!WeaponContainInventory(newWeapon))
        {
            weaponInventory.Add(weapon);
        }
        else
            Debug.Log("Has the gun");

        currentWeapon = weaponInventory.Count - 1;
    }

    public void GetMoreAmmo(int amount)
    {
        currentWeaponOnHold.maxAmmo += amount;
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
        holdAttack = currentWeaponOnHold.weapon.holdAttack;
        isMelee = currentWeaponOnHold.weapon.isMelee;
        atkRate = currentWeaponOnHold.weapon.atkRate;

        weaponDamage = currentWeaponOnHold.weapon.damage;

        if(isMelee)
        {
            bulletSpeed = 0;

            ammo = 0;

            bulletDestroyTime = 0;
        }
        else
        {
            bulletSpeed = currentWeaponOnHold.weapon.bulletSpeed;

            ammo = currentWeaponOnHold.weapon.ammo;

            bulletDestroyTime = currentWeaponOnHold.weapon.bulletDestroyTime;
        }
    }

    public WeaponStats WeaponContainInventory(WeaponStats weaponToSearch)
    {
        foreach(Weapon weapon in weaponInventory)
        {
            if (weapon.weapon == weaponToSearch)
                return weapon.weapon;
        }
        Debug.Log("Weapon not found");
        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(strikeArea, 0.5f);
    }
}
