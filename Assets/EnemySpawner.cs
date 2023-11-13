using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int enemyCount;
    public GameObject enemyPrefab;
    [Space]
    public float spawnRadius = 30;
    [Space(20)]
    public EnemyStats[] enemyStats;
    [Space, Header("---------------------------SPAWN SETTINGS--------------------------"), Space(7)]
    public float spawnTime;
    [Space]
    [ShowOnly] public List<GameObject> enemyEntry = new List<GameObject>();
    [ShowOnly] public List<EnemyMovement> enemyInRadius = new List<EnemyMovement>();
    [ShowOnly] public List<EnemyMovement> enemySpawned = new List<EnemyMovement>();
    private GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        enemies = new GameObject[enemyCount];

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);
            enemies[i] = obj;
            enemies[i].name = "Enemy " + i.ToString();
            obj.SetActive(false);
        }

        InvokeRepeating("SpawnEnemy", spawnTime, spawnTime);
    }

    private void Update()
    {
        SpawnRadius();
    }

    void SpawnEnemy()
    {
        foreach(GameObject enemy in enemies)
        {
            if(!enemy.activeInHierarchy)
            {
                if(enemyEntry.Count > 0)
                {
                    int ran = Random.Range(0, enemyEntry.Count);
                    enemy.transform.position = enemyEntry[ran].transform.position;
                    enemy.SetActive(true);
                    EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
                    enemyMovement.enabled = true;
                    enemyMovement.SetEnemyStats(enemyStats[Random.Range(0, enemyStats.Length)]);
                    enemySpawned.Add(enemyMovement);
                }
                break;
            }
        }
    }

    private void SpawnRadius()
    {
        Transform character = GameObject.FindWithTag("Player").transform;
        Collider2D[] collision = Physics2D.OverlapCircleAll(character.position, spawnRadius);
        List<GameObject> entry = new List<GameObject>();
        List<EnemyMovement> enemiesInRadius = new List<EnemyMovement>();

        DisabledAllEnemies();
        foreach (Collider2D hit in collision)
        {
            if (hit.CompareTag("SpawnPoint"))
            {
                if (!entry.Contains(hit.gameObject))
                    entry.Add(hit.gameObject);
            }
            if(hit.CompareTag("Enemy"))
            {
                EnemyMovement found = hit.GetComponent<EnemyMovement>();
                if (!enemiesInRadius.Contains(found))
                {
                    found.gameObject.SetActive(true);
                    enemiesInRadius.Add(found);
                }
            }
        }

        enemyEntry = entry;
        enemyInRadius = enemiesInRadius;
    }

    private void DisabledAllEnemies()
    {
        foreach(EnemyMovement enemy in enemySpawned)
        {
            enemy.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Transform character = GameObject.FindWithTag("Player").transform;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(character.position, spawnRadius);
    }
}
