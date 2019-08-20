using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Transform mainTower;
    [SerializeField]
    private Transform[] wayPoints;
    [SerializeField]
    private float navigationUpdate;

    private int target = 0;
    private Transform enemy;
    private float navigationTime = 0 ;


    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Transform>();
        GameManager.Instance.RegisterEnemy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (wayPoints != null)
        {
            navigationTime += Time.deltaTime;
            if (navigationTime > navigationUpdate)
            {
                if (target < wayPoints.Length)
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, wayPoints[target].position, navigationTime);
                    
                }
                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, mainTower.position, navigationTime);
                }
                navigationTime = 0;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "CheckPoint")
        {
            target += 1;
            Debug.Log(target);
        }
        else if (other.tag == "Finish")
        {
            
            GameManager.Instance.UnregisterEnemy(this);

        }
    }
}
