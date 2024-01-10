using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject[] enemySpawnerGo;
    private int nbreOfSpawner;

    private int waveCount;
    private int enemiesRemaining = 0;

    public static GameManager instance;

    void Start()
    {
        instance = this;
        enemySpawnerGo = GameObject.FindGameObjectsWithTag("EnemySpawner");
        nbreOfSpawner = enemySpawnerGo.Length;
    }

    void NewWave()
    {

    }

    void SpawnEnemy(int howManyEnemies, GameObject[] Spawner)
    {
        for (int i = 0; i < howManyEnemies; i++)
        {
            int spawner = Random.Range(0, Spawner.Length);
            //faire spawner un ennemi
        }
    }
}
