using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("EnemySpawn")]
    [SerializeField] private GameObject enemy;
    [SerializeField] private int waveCount;
    [SerializeField] private int howManyEnemies = 2;
    private GameObject[] enemySpawnerGo;
    private GameObject[] itemSpawnerGo;
    private GameObject[] itemGo;
    [SerializeField] private GameObject[] itemList;
    private int enemiesRemaining;
    private float timerBetweenWave = 20;
    private bool isCallingNewWave = false;
    private bool isSpawningItem;
    public static GameManager instance;

    [Header("Collectibles")]
    [SerializeField] private int score;
    private int kills;

    void Start()
    {
        instance = this;
        enemySpawnerGo = GameObject.FindGameObjectsWithTag("EnemySpawner");
        itemSpawnerGo = GameObject.FindGameObjectsWithTag("ItemSpawner");
    }

    private void Update()
    {
        // Spawn new wave when there is no more enemies
        if (enemiesRemaining == 0)
        {
            if(waveCount > 0)
            {
                SpawnItem(itemSpawnerGo);
            }
            HUDManager.instance.ShowTimer();
            HUDManager.instance.UpdateTimerTxt(Mathf.RoundToInt(timerBetweenWave));
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
        HUDManager.instance.HideTimer();
        foreach (GameObject var in itemSpawnerGo)
        {
            var.transform.GetChild(0).gameObject.SetActive(false);
        }
        List<GameObject> spawnerList = new List<GameObject>();
        spawnerList.Clear();
        isSpawningItem = false;

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

        HUDManager.instance.UpdateWaveTxt(waveCount + 1);
        if (waveCount >= 1)
        {
            AddScore(200 + waveCount * 20);
        }
        waveCount++;
        howManyEnemies = Mathf.RoundToInt(howManyEnemies * 1.5f);
        isCallingNewWave = false;
    }

    void SpawnEnemy(int howManyEnemies, List<GameObject> spawnerList)
    {
        enemiesRemaining = howManyEnemies;
        HUDManager.instance.UpdateEnemiesRemaining(enemiesRemaining);
        for (int i = 0; i < howManyEnemies; i++)
        {
            int spawner = Random.Range(0, spawnerList.Count);
            Instantiate(enemy, spawnerList[spawner].transform.position, Quaternion.Euler(0, 0, 0));
        }   
    }

    void SpawnItem(GameObject[] spawnerList)
    {
        if (!isSpawningItem)
        {
            itemGo = GameObject.FindGameObjectsWithTag("Item");
            if (itemGo != null)
            {
                foreach (GameObject item in itemGo)
                {
                    Destroy(item);
                }
            }
            List<int> spawnerId = new List<int>();
            spawnerId.Clear();
            isSpawningItem = true;
            int spawner;
            int howManyItems = Random.Range(1, waveCount + 1);

            if (howManyItems > 4) { howManyItems = 4; }
            for (int i = 0; i < howManyItems; i++)
            {
                do
                {
                    spawner = Random.Range(0, spawnerList.Length);
                } while (spawnerId.Contains(spawner));
                spawnerId.Add(spawner);

                Instantiate(itemList[Random.Range(0, itemList.Length)], spawnerList[spawner].transform.position + new Vector3(0, 1.4f, 0), Quaternion.Euler(0, 0, 0));
                spawnerList[spawner].transform.GetChild(0).gameObject.SetActive(true);
            }
            
        }     
    }

    public void SetEnemiesRemaing()
    {
        enemiesRemaining--;
        kills++;
        HUDManager.instance.UpdateKillsTxt(kills);
        HUDManager.instance.UpdateEnemiesRemaining(enemiesRemaining);
    }

    public void AddScore(int plusScore)
    {
        score += plusScore;
        HUDManager.instance.UpdateScoreTxt(score);
    }

    public void SetFinalGameInfos()
    {
        HUDManager.instance.UpdateScoreTxt(score);
        HUDManager.instance.UpdateWaveTxt(waveCount);
        HUDManager.instance.UpdateKillsTxt(kills);
    }
    public int GetScore()
    {
        return score;
    }
}
