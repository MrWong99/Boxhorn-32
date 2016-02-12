using UnityEngine;

public class SwitchToScreenOnBack : MonoBehaviour {

    public GameObject ScreenToSwitchTo;

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ScreenToSwitchTo.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
