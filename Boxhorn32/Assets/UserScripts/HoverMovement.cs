using UnityEngine;
using System.Collections;

public class HoverMovement : MonoBehaviour {

    public float Speed;

    public float Movement;

    private float YValue;

    private bool UpMovement = true;

	// Use this for initialization
	void Start () {
        YValue = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
	    if (UpMovement)
        {
            transform.Translate(Vector3.up * Time.deltaTime * Speed);
            if (transform.position.y > YValue + Movement)
            {
                UpMovement = false;
            }
        } 
        else
        {
            transform.Translate(Vector3.down * Time.deltaTime * Speed);
            if (transform.position.y < YValue - Movement)
            {
                UpMovement = true;
            }
        }
	}
}
