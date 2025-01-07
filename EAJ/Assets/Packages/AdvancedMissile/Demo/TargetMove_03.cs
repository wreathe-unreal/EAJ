using UnityEngine;
using System.Collections;

public class TargetMove_03 : MonoBehaviour {

    public float speed;
    public float range;
	private bool isIn = true;
	private bool isSet = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		var dis = Vector3.Distance(Vector3.zero, this.transform.position);
		if (dis >= range)
        {
            isIn = false;
        }
        else
		{
			isSet = false;
            isIn = true;
		}
		if (!isIn && !isSet || this.transform.position.y < 0) 
		{
			SetPos ();
		}
        transform.Translate(Vector3.forward * speed);
	}

    void CallMethod(float damage)
    {
        Debug.Log("Damage : " + damage);
    }

	void SetPos()
	{
		isSet = true;
		var half = range / 2f;
		var target = Vector3.zero + new Vector3(Random.Range(-half, half), Random.Range(-half, half), Random.Range(-half, half));
		this.transform.LookAt(target);
	}
}
