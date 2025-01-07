using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiMissileEmitter : MonoBehaviour {

    private GameObject randomMoveFollowMissile;

    void Awake()
    {
        randomMoveFollowMissile = Resources.Load("MissilePrefab/RandomMoveFollowMissile") as GameObject;
    }

	void Start () {
        for(int i=0; i<20; i++)
        {
            Instantiate(randomMoveFollowMissile, this.transform.position, Quaternion.LookRotation(new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f))));
        }
        Destroy(this.gameObject, 1.0f);
	}
	
	void Update () {
		
	}
}
