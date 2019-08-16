using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public GameObject spawnPoint;
    public GameObject[] enemies;
    public int totalEnemies;
    public int maxEnemiesOnScreen;
    public int enemiesPerSpawn;

    private int enemiesOnScreen = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemy();   
    }

    void SpawnEnemy()
    {
        if (enemiesPerSpawn > 0 && enemiesOnScreen < maxEnemiesOnScreen)
        {
            for (int i=0; i<enemiesPerSpawn; ++i)
            {
                if (enemiesOnScreen < maxEnemiesOnScreen)
                    for (int j = enemiesOnScreen; j < maxEnemiesOnScreen; ++j)
                    {
                        GameObject newEnemy = Instantiate(enemies[1]) as GameObject;
                        newEnemy.transform.position = spawnPoint.transform.position;
                        enemiesOnScreen += 1;
                    }

            }
        }
    }
    public void RemoveEnemyOnScreen()
    {
        if (enemiesOnScreen > 0)
        {
            enemiesOnScreen -= 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
