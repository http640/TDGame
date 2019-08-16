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
    const float spawnDelay = 0.5f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
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
