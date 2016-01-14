using UnityEngine;
using System.Collections;

public class SwitchToScreen : MonoBehaviour
{

    public float SwitchAfterSeconds;

    public GameObject ScreenToActivate;

    void Start()
    {
        StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        yield return new WaitForSeconds(SwitchAfterSeconds);
        gameObject.SetActive(false);
        ScreenToActivate.SetActive(true);
    }
}
