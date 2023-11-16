using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float time = 3f;
    private float damage = 0;
    public TrailRenderer trail;

    public void SetBulletStat(float newDmg, float newTime)
    {
        trail.Clear();
        time = newTime;
        damage = newDmg;
        StartCoroutine(DestroyObj());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") && collision.GetComponent<EnemyMovement>().enabled)
        {
            collision.GetComponent<EnemyMovement>().Damage(damage);
            gameObject.SetActive(false);
        }
    }

    private IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
