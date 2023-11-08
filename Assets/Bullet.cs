using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float time = 3f;

    public void SetBulletStat(float newTime)
    {
        time = newTime;
        StartCoroutine(DestroyObj());
    }

    private IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
