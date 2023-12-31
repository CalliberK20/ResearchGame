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
    [ShowOnly]public int currentWave = 0;
    [Space]
    public Wave[] waves;


    [System.Serializable]
    public class Wave
    {
        [HideInInspector] public string waveName;
        public float minTime = 2;
        public float maxTime = 2;
        public EnemyType[] zombieType;  
    }

    void Start ()
    {
        StartCoroutine(WaveDelay());
    }

    public void Update()
    {
        float fps = 1f / Time.unscaledDeltaTime;
        if (fps <= 100)
            Debug.LogError("FPS has reach its minimun");
    }

    private IEnumerator WaveDelay()
    {
        currentWave = 1;
        
        for (int i = 0; i < waves.Length; i++)
        {
            Wave wave = waves[i];
            UIManager.Instance.waveText.text = currentWave.ToString();
            foreach (EnemyType enemyType in wave.zombieType)
            {
                yield return new WaitForSeconds(Random.Range(wave.minTime, wave.maxTime));
                EnemySpawner.Instance.SpawnEnemy(enemyType);
            }

            //-----------------------Wait time for the next Wave-------------------
            yield return new WaitForSeconds(15f);

            currentWave++;
        }
    }

    private void OnValidate()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].waveName = "Wave " + (i + 1);
        }
    }
}
