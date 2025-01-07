using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMove : MonoBehaviour {

    private float m_time = 0;
    private float m_initPosY;
    private float m_rand;

	void Start () {
        m_initPosY = this.transform.position.y;
        m_rand = Random.Range(-1.0f, 1.0f);
    }
	
	void Update () {
        m_time += Time.deltaTime * 60f;
        if(m_time > 360) {
            m_time = 0;
        }
        this.transform.position = new Vector3(this.transform.position.x, m_initPosY - Mathf.Sin(m_time * Mathf.PI / 180) * 80 * m_rand, this.transform.position.z);
    }
}
