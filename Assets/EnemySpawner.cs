using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int enemyCount;
    public GameObject enemyPrefab;
    [Space]
    public GameObject[] spawnPoint;
    public float spawnTime;

    private GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        enemies = new GameObject[enemyCount];

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);
            enemies[i] = obj;
            obj.SetActive(false);
        }

        InvokeRepeating("SpawnEnemy", spawnTime, spawnTime);
    }

    void SpawnEnemy()
    {
        foreach(GameObject enemy in enemies)
        {
            if(!enemy.active)
            {
                int ran = Random.Range(0, spawnPoint.Length);
                enemy.transform.position = spawnPoint[ran].transform.position;
                enemy.SetActive(true);
                break;
            }
        }
    }
}