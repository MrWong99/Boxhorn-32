using UnityEngine;
using System.Collections;

public class _RocketStart : MonoBehaviour
{
    public Rigidbody turbine;
    Vector3 start;
    private float liftoff;
    private bool resetInitiated = false;

    // Use this for initialization
    void Start()
    {
        start = transform.position;
        liftoff = Time.time + 4.0f; // 4 Sekunden, initaler Timer
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > liftoff)  //LIFTOFF
        {
            Debug.Log("LIFTOFF");
            turbine.AddForce(Vector3.up * Time.deltaTime * 100000, ForceMode.Acceleration);
            if (!resetInitiated)
            {
                StartCoroutine(resetRocket()); // STOPS ROCKET X SECONDS AFTER LIFTOFF
                resetInitiated = true;
            }
        }
    }

    IEnumerator resetRocket()
    {
        yield return new WaitForSeconds(2);
        turbine.velocity = Vector3.zero;
        Debug.Log("RESET");
        liftoff = Time.time + 10.0f;	// 10 Sekunden zwischen weiteren Starts
        transform.position = start;
        resetInitiated = false;
    }
}