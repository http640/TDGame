using UnityEngine.EventSystems;
using UnityEngine;

public class TowerManager : Singletons<TowerManager>
{
    private TowerBtn towerBtnPressed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mapPoint, Vector2.zero);
            if (hit.collider.tag == "BuildSite")
                placeTower(hit);
        }
        
    }

    public void placeTower(RaycastHit2D hit)
    {
        if(!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null)
        {
            GameObject newTower = Instantiate(towerBtnPressed.TowerObject);
            newTower.transform.position = hit.transform.position;  
        }
        
    }
    public void SelectedTower (TowerBtn towerSelected)
    {
        towerBtnPressed = towerSelected;
        //Debug.Log("Tower Selected: " + towerBtnPressed.gameObject);
    }
}
