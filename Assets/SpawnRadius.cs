using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class SpawnRadius : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*List<GameObject> entry = new List<GameObject>();
        List<EnemyMovement> enemiesInRadius = new List<EnemyMovement>();

        if (collision.CompareTag("SpawnPoint"))
        {
            if (!entry.Contains(collision.gameObject))
                entry.Add(collision.gameObject);
        }

        if (collision.CompareTag("Enemy"))
        {
            EnemyMovement found = collision.GetComponent<EnemyMovement>();
            enemiesInRadius.Add(found);
        }

        EnemySpawner.Instance.enemyEntry = entry;
        EnemySpawner.Instance.enemyInRadius = enemiesInRadius;*/
    }
}
