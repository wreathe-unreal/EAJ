using UnityEngine;
using System.Collections;

public class BeamEmitter : MonoBehaviour {

    public GameObject beam;
    [Range(0, 360)]
    public int way;
    private float rad;

    void Awake()
    {
        var perRad = 360f / (float)way;
        while (rad < 360f)
        {
            Instantiate(beam, this.transform.position, this.transform.rotation * Quaternion.Euler(new Vector3(rad, 90, 0)));
            rad += perRad;
        }
        Destroy(this.gameObject);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
