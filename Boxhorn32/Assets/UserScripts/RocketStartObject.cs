using UnityEngine;
using System.Collections;

public class RocketStartObject : MonoBehaviour
{
    public float FlightTime;
    public Rigidbody Turbine;
    public TextMesh CountdownText;
    public AudioSource CountdownAudio;
    Vector3 start;
    private float liftoff;
    private bool resetInitiated = false;

    // Use this for initialization
    void Start()
    {
        start = transform.position;
        liftoff = Time.time + 1.0f; // 4 Sekunden, initaler Timer
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

    void OnDisable()
    {
        if (CountdownAudio != null && CountdownAudio.isPlaying)
        {
            CountdownAudio.Stop();
        }
    }

    void OnDestroy()
    {
        if (CountdownAudio != null && CountdownAudio.isPlaying)
        {
            CountdownAudio.Stop();
        }
    }

    IEnumerator countdown(float endTime)
    {
        CountdownAudio.Play();
        bool doOnce = true;
        while (endTime > Time.time)
        {
            CountdownText.text = "" + (int)(endTime - Time.time);
            yield return new WaitForSeconds(0.2f);
            if (doOnce && endTime - Time.time < 6)
            {
                endTime += 0.7f;
                doOnce = false;
            }
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
        CountdownAudio.Stop();
        resetInitiated = false;
        StartCoroutine(countdown(liftoff));
    }
}