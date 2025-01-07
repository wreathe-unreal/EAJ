using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FollowUI : MonoBehaviour {

    public GameObject target;
    private Text text;

    void Awake()
    {
        text = this.GetComponent<Text>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        var targetScreenPos = Camera.main.WorldToScreenPoint(target.transform.position);
        targetScreenPos = new Vector3(targetScreenPos.x, targetScreenPos.y, 0);
        text.rectTransform.position = targetScreenPos;
	}
}
