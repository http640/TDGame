using UnityEngine;

public enum proType
{
    rock, arrow, fireball
};

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private int attackStrength;
    [SerializeField]
    private proType projectileType;
    public Transform ThisTransform { get; set; }


    public void Awake()
    {
        ThisTransform = this.transform;
    }

    public int AttackStrength
    {
        get
        {
            return attackStrength;
        }
    }

    public proType ProjectileType
    {
        get
        {
            return projectileType;
        }
    }
}
