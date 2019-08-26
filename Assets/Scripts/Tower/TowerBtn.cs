
using UnityEngine;

public class TowerBtn : MonoBehaviour
{
    [SerializeField]
    private GameObject towerObject;
    [SerializeField]
    private Sprite dragSprite;
    [SerializeField]
    private int towerPrice;

    public GameObject TowerObject
    {
        get
        {
            return towerObject;
        }
    }
    public Sprite DrageSprite
    {
        get
        {
            return dragSprite;
        }
    }
    public int TpwerPrice
    {
        get
        {
            return towerPrice;
        }
    }
}
