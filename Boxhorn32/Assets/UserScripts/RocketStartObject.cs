using UnityEngine;
using System.Collections;

public class RocketStartObject : MonoBehaviour
{
    public float FlightTime;
    public Rigidbody Turbine;
    public TextMesh CountdownText;
    Vector3 start;
    private float liftoff;
    private bool resetInitiated = false;

    // Use this for initialization
    void Start()
    {
        start = transform.position;
        liftoff = Time.time + 4.0f; // 4 Sekunden, initaler Timer
        StartCoroutine(countdown(liftoff));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > liftoff)  //LIFTOFF
        {
            Debug.Log("LIFTOFF");
            Turbine.AddForce(Vector3.up * Time.deltaTime * 100000, ForceMode.Acceleration);
            if (!resetInitiated)
            {
                StartCoroutine(resetRocket()); // STOPS ROCKET X SECONDS AFTER LIFTOFF
                resetInitiated = true;
            }
        }
    }

    IEnumerator countdown(float endTime)
    {
        while (endTime > Time.time)
        {
            CountdownText.text = "" + (int)(endTime - Time.time);
            yield return new WaitForSeconds(0.2f);
        }
        CountdownText.text = "";
    }

    IEnumerator resetRocket()
    {
        yield return new WaitForSeconds(FlightTime);
        Turbine.velocity = Vector3.zero;
        Debug.Log("RESET");
        liftoff = Time.time + 10.5f;	// 10 Sekunden zwischen weiteren Starts
        transform.position = start;
        resetInitiated = false;
        StartCoroutine(countdown(liftoff));
    }
}