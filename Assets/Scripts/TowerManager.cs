using UnityEngine.EventSystems;
using UnityEngine;

public class TowerManager : Singletons<TowerManager>
{
    private TowerBtn towerBtnPressed;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mapPoint, Vector2.zero);
            if (hit.collider.tag == "BuildSite")
            {
                hit.collider.tag = "buildSiteFull";
                placeTower(hit);
            }
                
        }
        if (spriteRenderer.enabled)
        {
            followMouse();
        }

    }

    public void placeTower(RaycastHit2D hit)
    {
        if(!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null)
        {
            GameObject newTower = Instantiate(towerBtnPressed.TowerObject);
            newTower.transform.position = hit.transform.position;
            disableDragSprite();
        }
        
    }
    public void SelectedTower (TowerBtn towerSelected)
    {
        towerBtnPressed = towerSelected;
        //Debug.Log("Tower Selected: " + towerBtnPressed.gameObject);
        enableDragSprite(towerBtnPressed.DrageSprite);
    }
    public void followMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }
    public void enableDragSprite(Sprite sprite)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;

    }
    public void disableDragSprite()
    {
        spriteRenderer.enabled = false;

    }

}
