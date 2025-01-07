using UnityEngine;
using System.Collections;

public class EmitterMovement : MonoBehaviour {

    public float speed = 1;
    public bool isInverceX = false;
    public bool isInverceY = false;
    private int inverceX = 1;
    private int inverceY = 1;
    private float mouseX, mouseY;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

	void Start () {
        if (isInverceX)
        {
            inverceX *= -1;
        }
        if (isInverceY)
        {
            inverceY *= -1;
        }
	}

    private void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(Vector3.forward * speed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(Vector3.back * speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Vector3.left * speed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(Vector3.right * speed);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            this.transform.Translate(Vector3.up * speed);
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            this.transform.Translate(Vector3.down * speed);
        }
    }
	
	void FixedUpdate () {
        mouseX += Input.GetAxis("Mouse Y") * inverceX;
        mouseY += Input.GetAxis("Mouse X") * inverceY;
        this.transform.eulerAngles = new Vector3(mouseX, mouseY);
	}
}
