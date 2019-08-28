using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public MovementPath myPath;
    [SerializeField]
    private int target = 0;
    private Transform enemy;
    [SerializeField]
    private int healthPoint;
    [SerializeField]
    private int killRewad;
    private bool isDead = false;
    private Collider2D enemyCollider;
    private Animator anim;

    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Transform>();
        GameManager.Instance.RegisterEnemy(this);
        if (myPath.PathSequence.Length == 0)
            Debug.Log("We need a path to follow");
        enemyCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myPath.PathSequence.Length != 0 && !isDead)
        {
            
            if (target < myPath.PathSequence.Length)
            {
                enemy.position = Vector2.MoveTowards(enemy.position, myPath.PathSequence[target].position, Time.deltaTime * speed);
                    
            }
            
        }
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "path")
        {
            target += 1;
            //Debug.Log("target: " + target);
        }
        else if (col.gameObject.tag == "Finish")
        {
            GameManager.Instance.RoundEscaped += 1;
            GameManager.Instance.TotalEscaped += 1;
            GameManager.Instance.UnregisterEnemy(this);
            GameManager.Instance.isWaveOver();
        }
        else if (col.gameObject.tag == "projectile")
        {
            Projectile newProjectile = col.gameObject.GetComponent<Projectile>();
            enemyHit(newProjectile.AttackStrength);
            Destroy(col.gameObject);
        }

    }
    public void enemyHit(int hitpoints)
    {
        if(healthPoint - hitpoints > 0)
        {
            healthPoint -= hitpoints;
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Hit);
            anim.Play("Hurt"); 
        }
        else
        {
            anim.SetTrigger("didDie");
            //anim.Play("Diying");
            die();
        }
    }
    public void die()
    {
        isDead = true;
        enemyCollider.enabled = false;
        GameManager.Instance.TotalKilled += 1;
        GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Death);
        GameManager.Instance.addMoney(killRewad);
        GameManager.Instance.isWaveOver();
    }
}
