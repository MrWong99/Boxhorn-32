using UnityEngine;
using System.Collections;

public class SimpleRotateObject : MonoBehaviour {

    // The speed at which the object should rotate
    public float speed;

    // The axis around where the object should be rotated
    public Vector3 rotationAxis;

    private Vector3 pivot;

    void Start ()
    {
        pivot = transform.parent.position;
    }
    
	// Update is called once per frame
	void Update () {
        transform.RotateAround(pivot, rotationAxis, Time.deltaTime * speed);
    }
}
