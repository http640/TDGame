using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : Singletons<LevelInfo>
{
    [SerializeField]
    private int _index;
    [SerializeField]
    private int _totalWave = 5;
    [SerializeField]
    private int _lastEnemyToUnlock;
    [SerializeField]
    private int _lastTowerToUnlock;

    private bool _levelComplete = false;
    private float _upgradePesentage = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
