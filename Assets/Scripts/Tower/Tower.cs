using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private float timeBetweenAttacks;
    [SerializeField]
    private float attackRadius;
    [SerializeField]
    private float projectileSpeed = 5f;
    [SerializeField]
    private Projectile projectile;
    [SerializeField]
    private float healthPoint;
    private Enemy targetEnemy = null;
    private float attackCounter;
    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // find Enemy and Attack to it
    void Update()
    {
        attackCounter -= Time.deltaTime;
        if(targetEnemy == null || targetEnemy.IsDead)
        {
            Enemy nearestEnemy = GetNearestEnemyInRange();
            if(nearestEnemy != null && Vector2.Distance(transform.localPosition, nearestEnemy.transform.localPosition) <= attackRadius)
            {
                targetEnemy = GetNearestEnemyInRange();
            }
        }
        else
        {
            if (attackCounter <= 0)
            {
                isAttacking = true;
                //reset attackCounter
                attackCounter = timeBetweenAttacks;
            }
            else
            {
                isAttacking = false;

            }
            if (Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) > attackRadius)
            {
                targetEnemy = null;
            }
        }
    }
    private void FixedUpdate()
    {
        if (isAttacking)
        {
            Attack();
        }
        
    }
    //create projectile and its sounds and shooting projectiles
    public void Attack()
    {
        isAttacking = false;
        Projectile newProjectile = Instantiate(projectile) as Projectile;
        newProjectile.transform.localPosition = transform.localPosition;
        if(newProjectile.ProjectileType == proType.arrow)
        {
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Arrow);
        }
        else if(newProjectile.ProjectileType == proType.fireball)
        {
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Fireball);
        }
        else if(newProjectile.ProjectileType == proType.rock)
        {
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Rock);
        }
        if(targetEnemy == null)
        {
            Destroy(newProjectile.ThisTransform.gameObject);
        }
        else
        {
            //move Projectile to Enenmy
            StartCoroutine (ShootingProjectile(newProjectile));
        }
    }
    //shooting a rotate projectile to enemy
    IEnumerator ShootingProjectile(Projectile projectile)
    {
        while (getTargetDistance(targetEnemy) > 0.10f && projectile != null && targetEnemy != null)
        {
            var dir = (targetEnemy.transform.position - projectile.transform.position);
            /*
            Quaternion rot = Quaternion.LookRotation(dir);
            rot.x = 0;
            rot.z = 0;
            projectile.ThisTransform.rotation = Quaternion.Slerp(projectile.ThisTransform.rotation, rot, Time.deltaTime * 15f);
            */
            var angleDirection = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angleDirection,Vector3.forward);
            
            projectile.transform.localPosition = Vector2.MoveTowards(projectile.transform.localPosition, targetEnemy.transform.localPosition, projectileSpeed * Time.deltaTime);
            
            
            
            

            Debug.DrawRay(transform.position, dir, Color.red);
            yield return null;
        }
        if(projectile != null || targetEnemy == null)
        {
            Destroy(projectile);
        }
    }
    private float getTargetDistance(Enemy thisEnemy)
    {
        if(thisEnemy == null)
        {
            thisEnemy = GetNearestEnemyInRange();
            if (thisEnemy == null)
                return 0f;  
        }
        return Mathf.Abs(Vector2.Distance(transform.localPosition, thisEnemy.transform.localPosition));
    }
    //if enemy be in range it will add to a list  
    private List<Enemy> GetEnemiesInRange()
    {
        List<Enemy> enemiesInRange = new List<Enemy>();
        foreach(Enemy enemy in GameManager.Instance.EnemyList)
        {
            if(Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= attackRadius)
            {
                enemiesInRange.Add(enemy);
            }
        }
        return enemiesInRange;
    }
    //find the nearest enemy to tower and select it as targetEnemy
    private Enemy GetNearestEnemyInRange()
    {
        Enemy nearestEnemy = null;
        float smallestDistance = float.PositiveInfinity;
        foreach(Enemy enemy in GetEnemiesInRange())
        {
            if(Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < smallestDistance)
            {
                smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy; 
    }
}
