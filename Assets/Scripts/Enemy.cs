using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public MovementPath myPath;
    [SerializeField]
    private int target = 0;
    private Transform enemy;

    


    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Transform>();
        GameManager.Instance.RegisterEnemy(this);
        if (myPath.PathSequence.Length == 0)
            Debug.Log("We need a path to follow");
    }

    // Update is called once per frame
    void Update()
    {
        if (myPath.PathSequence.Length != 0)
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
            
            GameManager.Instance.UnregisterEnemy(this);
           
        }
        else if (col.gameObject.tag == "projectile")
        {
            Destroy(col.gameObject);
        }
    }
}
