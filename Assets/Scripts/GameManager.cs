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

    const float spawnDelay = 0.5f;
    public List<Enemy> EnemyList = new List<Enemy>();
        
    // Start is called before the first frame update

    private void Start()
    {
        StartCoroutine(spawn());
    }
    IEnumerator spawn ()
    {
        if (enemiesPerSpawn > 0 && EnemyList.Count < maxEnemiesOnScreen)
        {
            for (int i = 0; i < enemiesPerSpawn; ++i)
            {
                if (EnemyList.Count < maxEnemiesOnScreen)
                    
                {
                    GameObject newEnemy = Instantiate(enemies[1]) as GameObject;
                    newEnemy.transform.position = spawnPoint.transform.position;
                }

            }
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(spawn());

        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }
    /*public void RemoveEnemyOnScreen()
    {
        if (enemiesOnScreen > 0)
        {
            enemiesOnScreen -= 1;
        }
    }*/

   public void DestroyAllEnemies()
    {
        foreach(Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }

        EnemyList.Clear();  
    }
}
