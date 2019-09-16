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
    private int totalWaves;
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
    private Enemy[] enemies;
    [SerializeField]
    private int totalEnemies = 3;
    [SerializeField]
    private int enemiesPerSpawn;
    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private LevelInfo levelInfo;

    private int waveNumber = 0;
    private int totalMoney = 10;
    private int totalEscaped = 0;
    private int roundEscaped = 0;
    private int totalKilled = 0;
    private int enemiesToSpawn = 0;
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
    //Spawn Enemey in each wave
    IEnumerator spawn ()
    {
        if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; ++i)
            {
                if (EnemyList.Count < totalEnemies)
                    
                {
                    Enemy newEnemy = Instantiate(enemies[Random.Range(0,enemiesToSpawn)], spawnPoint.transform.position,Quaternion.identity) as Enemy;
               
                }
            }
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(spawn());
        }
    }
    //Add Enemy to List to know how many enemy spawned
    public void RegisterEnemy(Enemy enemy)
    {
        //call in Start Function in Enemy Script
        EnemyList.Add(enemy);
    }
    //Remove enemy form list when collide by Finish tag (MainTower)
    public void UnregisterEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }
    //Destroy all Enemies and clear EnemyList when Play Button or Next wave Pressed
    public void DestroyAllEnemies()
    {
        foreach(Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }
        EnemyList.Clear();  
    }
    //Add Money when a tower kill an Enemy and killReward will add to TotalMoney
    public void addMoney(int amount)
    {
        TotalMoney += amount;
    }
    //Subtract Tower Price From Total Money When a Tower Deploy
    public void subtractMoney(int amount)
    {
        TotalMoney -= amount;
    }
    //it controls Escaped Label text and call in each collide with Finish tag
    //if Enemies escaped + killed = totalEnemies so wave is over and call in each deth and change GameState and show menu
    public void isWaveOver()
    {
        totalEscapedLbl.text = "Escaped " + TotalEscaped + "/" + escapeLimit;
        if((RoundEscaped + TotalKilled) == totalEnemies)
        {
            if(waveNumber <= enemies.Length)
            {
                enemiesToSpawn = waveNumber;
            }
            setCurrentGameState();
            showMenu(); 
        }
    }
    //Control events when play button pressed
    public void playBtnPressed()
    {
        //if currentState = gameStatus.next wave number will increse and total enemy for next round will add up with waveNumber
        //else all things have to be reset
        switch (currentState)
        {
            case gameStatus.next:
                waveNumber += 1;
                totalEnemies += waveNumber;
                break;
            
            default:
                totalEnemies = 4; //have to be 5
                TotalEscaped = 0;
                TotalMoney = 10;
                enemiesToSpawn = 0;
                waveNumber = 0;
                TowerManager.Instance.DestroyAllTower();
                TowerManager.Instance.RenameBuiltSitesTags();
                totalMoneyLbl.text = TotalMoney.ToString();
                totalEscapedLbl.text = "Escaped " + TotalEscaped + "/" + escapeLimit;
                audioSource.PlayOneShot(SoundManager.Instance.NewGame);
                break;
        }
        //reset parameters for new wave
        DestroyAllEnemies();
        TotalKilled = 0;
        RoundEscaped = 0;
        currentWaveLbl.text = "Wave " + (waveNumber + 1);
        //for new wave we have to call spawn again with new values
        StartCoroutine(spawn());
        playBtn.gameObject.SetActive(false);
    }
    //when isWaveOver we have to change setCurrentGameState 
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
    //foreach state it change playBtnLbl text
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
                playBtnLbl.text = "Go To Next Level";
                break;
            case gameStatus.shop:
                //go to shop menu
                break;
            case gameStatus.stop:
                //go to stop (option, resume, exit) menu
                break;
        }
        //in show menu true and false when it pressed 
        playBtn.gameObject.SetActive(true);
    }
    //when pressed Escaped Key on the Keyboard disableDragSprite function has been called
    private void handelEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TowerManager.Instance.disableDragSprite();
            TowerManager.Instance.towerBtnPressed = null;
        }
    }
}
