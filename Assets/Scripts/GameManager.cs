using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("EnemySpawn")]
    [SerializeField] private GameObject enemy;
    [SerializeField] private int waveCount;
    [SerializeField] private int howManyEnemies = 2;
    private GameObject[] enemySpawnerGo;
    private int enemiesRemaining = 0;
    private float timerBetweenWave = 20;
    private bool isCallingNewWave = false;
    public static GameManager instance;

    [Header("Collectibles")]
    [SerializeField] private int score;

    void Start()
    {
        instance = this;
        enemySpawnerGo = GameObject.FindGameObjectsWithTag("EnemySpawner");
    }

    private void Update()
    {
        // Spawn new wave when there is no more enemies
        if (enemiesRemaining == 0)
        {
            timerBetweenWave -= Time.deltaTime;
            if (timerBetweenWave < 0 && !isCallingNewWave)
            {
                isCallingNewWave = true;
                timerBetweenWave = 20;
                NewWave();
            }
        }
    }

    // Wave System
    void NewWave()
    {
        List<GameObject> spawnerList = new List<GameObject>();
        spawnerList.Clear();

        if (waveCount == 0)
        {
            spawnerList.Add(enemySpawnerGo[0]);
        }
        else if (waveCount > 0 && waveCount <= 5)
        {
            for (int i = 0; i < 2; i++)
            {
                spawnerList.Add(enemySpawnerGo[i]);
            }
        }
        else if (waveCount > 5 && waveCount <= 8)
        {
            for (int i = 0; i < 3; i++)
            {
                spawnerList.Add(enemySpawnerGo[i]);
            }
        }
        else if (waveCount > 8 && waveCount <= 10)
        {
            for (int i = 0; i < 4; i++)
            {
                spawnerList.Add(enemySpawnerGo[i]);
            }
        }
        else if (waveCount > 10 && waveCount <= 12)
        {
            for (int i = 0; i < 5; i++)
            {
                spawnerList.Add(enemySpawnerGo[i]);
            }
        }
        else if (waveCount > 12)
        {
            for (int i = 0; i < 6; i++)
            {
                spawnerList.Add(enemySpawnerGo[i]);
            }
        }

        SpawnEnemy(howManyEnemies, spawnerList);

        waveCount++;
        howManyEnemies = Mathf.RoundToInt(howManyEnemies * 1.5f);
    }

    void SpawnEnemy(int howManyEnemies, List<GameObject> spawnerList)
    {
        enemiesRemaining = howManyEnemies;
        for (int i = 0; i < howManyEnemies; i++)
        {
            int spawner = Random.Range(0, spawnerList.Count);
            Instantiate(enemy, spawnerList[spawner].transform.position, Quaternion.Euler(0, 0, 0));
        }   
    }
}