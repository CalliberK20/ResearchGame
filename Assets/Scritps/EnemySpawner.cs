using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

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
                }
                break;
            }
        }
    }

    private void SpawnRadius()
    {
        Collider2D[] collision = Physics2D.OverlapBoxAll(Camera.main.transform.position, FollowCamera.viewPoint, 0f);
        List<GameObject> entry = new List<GameObject>();

        foreach (Collider2D hit in collision)
        {
            if (hit.CompareTag("SpawnPoint"))
            {
                if (!entry.Contains(hit.gameObject))
                    entry.Add(hit.gameObject);
            }
        }
        enemyEntry = entry;
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Camera.main.transform.position, FollowCamera.viewPoint);
    }
}
