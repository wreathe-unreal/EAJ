using System.Collections;
using System.Collections.Generic;
using EAJ;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private SixDOFController PlayerTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void SetPlayerTarget(SixDOFController player)
    {
        PlayerTarget = player;
    }
}
