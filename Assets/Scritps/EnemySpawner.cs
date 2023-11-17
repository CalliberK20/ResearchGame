using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

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

    private void Awake()
    {
        Instance = this;
    }

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
    }

    private void Update()
    {
        SpawnRadius();
    }

    public void SpawnEnemy(EnemyType type)
    {
        foreach(GameObject enemy in enemies)
        {
            if(!enemy.activeInHierarchy)
            {
                if(enemyEntry.Count > 0)
                {
                    enemy.SetActive(true);
                    int ran = Random.Range(0, enemyEntry.Count);
                    enemy.transform.position = enemyEntry[ran].GetComponent<SpawnPoint>().Spawn(); 
                    EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
                    enemyMovement.enabled = true;
                    enemyMovement.SetEnemyStats(TypeToStats(type));
                    if (!enemySpawned.Contains(enemyMovement))
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
                    enemiesInRadius.Add(found);
            }
        }

        enemyEntry = entry;
        enemyInRadius = enemiesInRadius;

        foreach (EnemyMovement enemy in enemySpawned)
        {
            if(!enemyInRadius.Contains(enemy))
            {
                int ran = Random.Range(0, enemyEntry.Count);
                enemy.transform.position = enemyEntry[ran].GetComponent<SpawnPoint>().Spawn();
                enemy.path = new List<NodeGrid>();
            }
        }
    }

    private EnemyStats TypeToStats(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.normal:return enemyStats[0];
            case EnemyType.heavy: return enemyStats[1];
        }
        int ran = Random.Range(0, enemyStats.Length);
        return enemyStats[ran];
    }

    private void OnDrawGizmos()
    {
        Transform character = GameObject.FindWithTag("Player").transform;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(character.position, spawnRadius);
    }
}
