using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singletons<GameManager>
{
    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private GameObject[] enemies;
    [SerializeField]
    private int totalEnemies;
    [SerializeField]
    private int maxEnemiesOnScreen;
    [SerializeField]
    private int enemiesPerSpawn;

    private int enemiesOnScreen = 0;
    const float spawnDelay = 0.5f;

  
    // Start is called before the first frame update

    private void Start()
    {
        StartCoroutine(spawn());
    }
    IEnumerator spawn ()
    {
        if (enemiesPerSpawn > 0 && enemiesOnScreen < maxEnemiesOnScreen)
        {
            for (int i = 0; i < enemiesPerSpawn; ++i)
            {
                if (enemiesOnScreen < maxEnemiesOnScreen)
                    
                {
                    GameObject newEnemy = Instantiate(enemies[1]) as GameObject;
                    newEnemy.transform.position = spawnPoint.transform.position;
                    enemiesOnScreen += 1;
                }

            }
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(spawn());

        }
    }
    public void RemoveEnemyOnScreen()
    {
        if (enemiesOnScreen > 0)
        {
            enemiesOnScreen -= 1;
        }
    }

   
}
