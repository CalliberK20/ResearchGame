using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public int preBulletCount = 3;
    public GameObject bulletPrefab;
    [Space]
    public float delayShot = 1f;
    [Space, Header("Bullet Setting")]
    public float bulletSpeed = 1f;
    public float bulletDestroyTime = 1f;
    [Space]
    public List<GameObject> bullets = new List<GameObject>();
    private float startTime;

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

        startTime = delayShot;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (startTime >= delayShot)
                Shoot();
        }

        if (startTime < delayShot)
            startTime += Time.deltaTime;

        Debug.DrawLine(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.red);
    }

    void Shoot()
    {
        if (GetActiveBullet() != null)
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
}
