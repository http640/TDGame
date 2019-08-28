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
    private int escapeLimit;
    [SerializeField]
    private Button playBtn;
    [SerializeField]
    private GameObject[] enemies;
    [SerializeField]
    private int totalEnemies = 3;
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
    private AudioSource audioSource;

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
    public int TotalEscaped
    {
        get
        {
            return totalEscaped;
        }
        set
        {
            totalEscaped = value;
        }
    }
    public int EscapeLimit
    {
        get
        {
            return escapeLimit;
        }
    }
    public int RoundEscaped
    {
        get
        {
            return roundEscaped;
        }
        set
        {
            roundEscaped = value;
        }
    }
    public int TotalKilled
    {
        get
        {
            return totalKilled;
        }
        set
        {
            totalKilled = value;
        }
    }
    public AudioSource AudioSource
    {
        get
        {
            return audioSource;
        }
    }

    private void Start()
    {
        playBtn.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        showMenu();
    }
    private void Update()
    {
        handelEscape();
    }
    IEnumerator spawn ()
    {
        if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; ++i)
            {
                if (EnemyList.Count < totalEnemies)
                    
                {
                    GameObject newEnemy = Instantiate(enemies[0]) as GameObject;
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
    public void isWaveOver()
    {
        totalEscapedLbl.text = "Escaped " + TotalEscaped + "/" + escapeLimit;
        if((RoundEscaped + TotalKilled) == totalEnemies)
        {

            setCurrentGameState();
            showMenu(); 
        }
    }
    public void playBtnPressed()
    {
        switch (currentState)
        {
            case gameStatus.next:
                waveNumber += 1;
                totalEnemies += waveNumber;
                break;
            
            default:
                totalEnemies = 3; //have to be 5
                TotalEscaped = 0;
                TotalMoney = 10;
                TowerManager.Instance.DestroyAllTower();
                TowerManager.Instance.RenameBuiltSitesTags();
                totalMoneyLbl.text = TotalMoney.ToString();
                totalEscapedLbl.text = "Escaped " + TotalEscaped + "/" + escapeLimit;
                audioSource.PlayOneShot(SoundManager.Instance.NewGame);
                break;
        }
        DestroyAllEnemies();
        TotalKilled = 0;
        RoundEscaped = 0;
        currentWaveLbl.text = "Wave " + (waveNumber + 1);
        StartCoroutine(spawn());
        playBtn.gameObject.SetActive(false);
    }

    public void setCurrentGameState()
    {
        if (TotalEscaped >= escapeLimit)
        {
            currentState = gameStatus.gameover;
        }
        else if (waveNumber == 0 && (TotalKilled + TotalEscaped) == 0)
        {
            currentState = gameStatus.play;
        }
        else if (waveNumber >= totalWaves)
        {
            currentState = gameStatus.win;
        }
        //add for stop and shop btn
        else
        {
            currentState = gameStatus.next;
        }
    }

    public void showMenu()
    {
        switch (currentState)
        {
            case gameStatus.gameover:
                playBtnLbl.text = "Play Again";
                AudioSource.PlayOneShot(SoundManager.Instance.Gameover);
                break;
            case gameStatus.next:
                playBtnLbl.text = "Next Wave";
                break;
            case gameStatus.play:
                playBtnLbl.text = "Play";
                break;
            case gameStatus.win:
                playBtnLbl.text = "Play";
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
