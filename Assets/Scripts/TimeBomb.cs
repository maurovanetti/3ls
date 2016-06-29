using UnityEngine;
using System.Collections;

public class TimeBomb : MonoBehaviour {

    public float timeOut;
    private float countDown;
    private Vector3 lastMousePosition;

    // Use this for initialization
    void Start () {
        countDown = timeOut;
	}
	
	// Update is called once per frame
	void Update () {
        countDown -= Time.deltaTime;
        if (Input.mousePosition != lastMousePosition)
        {
            countDown = timeOut;
            lastMousePosition = Input.mousePosition;
        }
        if (countDown < 0f)
        {
            Debug.LogError("Timeout: " + timeOut + "\" of mouse inactivity. Quitting the game.");
            Application.Quit();
        }
	}
}
