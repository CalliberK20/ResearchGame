using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum EnemyType
{
    random,
    normal,
    heavy,
}

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [ShowOnly]public int currentWave = 0;
    public int numOfZombiesInGame = 0;
    public float rewardRate = 5;
    public float difficultyRate = 10;
    [Space]
    public Wave[] waves;
    public GameObject winParent;

    [System.Serializable]
    public class Wave
    {
        [HideInInspector] public string waveName;
        public float minTime = 2;
        public float maxTime = 2;
        public EnemyType[] zombieType;  
    }

    public float currentRewardRate;
    public float currentdifficultyRate;

    private void Awake()
    {
        instance = this;
    }

    void Start ()
    {
        StartCoroutine(WaveDelay());
    }

    public void Update()
    {
        float fps = 1f / Time.unscaledDeltaTime;
        if (fps <= 60)
            Debug.LogError("FPS has reach its minimun");
    }

    private IEnumerator WaveDelay()
    {
        currentWave = 1;
        int increaseDifficulty = 1;
        int milestone = 5;

        for (int i = 0; i < waves.Length; i++)
        {
            Wave wave = waves[i];
            UIManager.Instance.waveText.text = currentWave.ToString();
            foreach (EnemyType enemyType in wave.zombieType)
            {
                float delayTime = Random.Range(wave.minTime, wave.maxTime);
                yield return new WaitForSeconds(delayTime);
                numOfZombiesInGame++;
                EnemySpawner.Instance.SpawnEnemy(enemyType);
            }

            //-----------------------Wait time for the next Wave-------------------
            yield return new WaitForSeconds(15f);

            currentWave++;
            if(currentWave >= 1)
            {
                currentdifficultyRate = difficultyRate * increaseDifficulty;
                currentRewardRate = rewardRate * increaseDifficulty;
                increaseDifficulty++;
                milestone += 5;
            }
        }

        yield return new WaitUntil(() => numOfZombiesInGame <= 0);
        winParent.gameObject.SetActive(true);
        StartCoroutine(SceneLead.ResetScene(3f));
    }

    private void OnValidate()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].waveName = "Wave " + (i + 1);
        }
    }
}
