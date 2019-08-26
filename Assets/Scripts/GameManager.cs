using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum gameStatus
{
    next, play, gameover, win, stop, shop
}

public class GameManager : Singletons<GameManager>
{
    [SerializeField]
    private int totalWaves = 10;
    [SerializeField]
    private Text totalMoneyLbl;
    [SerializeField]
    private Text currentWaveLbl;
    [SerializeField]
    private Text playBtnLbl;
    [SerializeField]
    private Text totalEscapedLbl;
    [SerializeField]
    private Button playBtn;
    [SerializeField]
    private GameObject[] enemies;
    [SerializeField]
    private int totalEnemies;
    [SerializeField]
    private int maxEnemiesOnScreen;
    [SerializeField]
    private int enemiesPerSpawn;
    [SerializeField]
    private GameObject spawnPoint;


    private int waveNumber = 0;
    private int totalMoney = 10;
    private int totalEscaped = 0;
    private int roundEscaped = 0;
    private int totalKilled = 0;
    private int whitchEnemyToSpawn = 0;
    private gameStatus currentState = gameStatus.play;

    const float spawnDelay = 0.5f;
    public List<Enemy> EnemyList = new List<Enemy>();
        
    public int TotalMoney
    {
        get
        {
            return totalMoney;
        }
        set
        {
            totalMoney = value;
            totalMoneyLbl.text = totalMoney.ToString();
        }
    }

    private void Start()
    {
        playBtn.gameObject.SetActive(false);
        showMenu();
    }
    private void Update()
    {
        handelEscape();
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
    public void addMoney(int amount)
    {
        TotalMoney += amount;
    }
    public void subtractMoney(int amount)
    {
        TotalMoney -= amount;
    }
    public void showMenu()
    {
        switch (currentState)
        {
            case gameStatus.gameover:
                playBtnLbl.text = "Play Again";
                //add gameover sound
                break;
            case gameStatus.next:
                playBtnLbl.text = "Next Wave";
                break;
            case gameStatus.play:
                playBtnLbl.text = "Play";
                break;
            case gameStatus.win:
                playBtnLbl.text = "Playe";
                break;
            case gameStatus.shop:
                //go to shop menu
                break;
            case gameStatus.stop:
                //go to stop (option, resume, exit) menu
                break;
        }
        playBtn.gameObject.SetActive(true);

    }
    private void handelEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TowerManager.Instance.disableDragSprite();
            TowerManager.Instance.towerBtnPressed = null;
        }
    }
}
