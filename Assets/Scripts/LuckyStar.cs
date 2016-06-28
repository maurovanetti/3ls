using UnityEngine;
using System.Collections;
using System;

public class LuckyStar : MonoBehaviour {

    private static bool alarmsSet;
    private static bool draggable;
    private static Texture2D hourglassCursorTexture;

    public float degreesPerFrame;
    private Vector3 rotationPerFrame;
    
    public float springStrength;
    private bool dragged;

    private Transform t;
    private Rigidbody2D rb;
    private GameObject[] constellation;

    // Use this for initialization
    void Start () {
        t = transform;
        rb = GetComponent<Rigidbody2D>();
        rotationPerFrame = new Vector3(0f, 0f, degreesPerFrame);
        constellation = GameObject.FindGameObjectsWithTag("Constellation");
        if (!alarmsSet)
        {
            Sky sky = Sky.GetSky();
            sky.SetAlarm(Sky.SunshineTime, AutodisableDrag);
            sky.SetAlarm(Sky.SunsetTime, AutoenableDrag);
            alarmsSet = true;
            draggable = true;
        }
        GetComponentInParent<Camp>().Select();
    }
	
	// Update is called once per frame
	void Update () {
        if (dragged)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f;
            Vector3 dragTo = Camera.main.ScreenToWorldPoint(mousePosition);
            dragTo.z = t.position.z;
            t.position = dragTo;
        }
        else
        {
            Spin();
            BeAttracted();
        }
	}

    void OnMouseDrag()
    {
        if (draggable)
        {
            dragged = true;
        }
    }

    void OnMouseUp()
    {
        Camp origin = GetComponentInParent<Camp>();        
        dragged = false;
        float minDistance = float.PositiveInfinity;
        GameObject closest = null;
        foreach (GameObject starObject in constellation) {
            float distanceFromStarObject = Vector3.Distance(t.position, starObject.transform.position);
            if (Vector3.Distance(t.position, starObject.transform.position) < minDistance)
            {
                closest = starObject;
                minDistance = distanceFromStarObject;
            }
        }
        t.parent = closest.transform;
        Camp destination = GetComponentInParent<Camp>();
        if (origin != destination)
        {
            origin.UnselectIfNecessary(this);
            destination.Select();
            SetDraggable(false);
        }
    }

    private void Spin()
    {        
        t.Rotate(rotationPerFrame);
    }
    

    private void BeAttracted()
    {
        if (t.parent != null)
        {
            Vector2 v = t.localPosition;
            rb.AddForce(-v * springStrength, ForceMode2D.Force);
        }
    }

    public static void SetDraggable(bool draggable)
    {
        LuckyStar.draggable = draggable;
        if (draggable)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            if (hourglassCursorTexture == null)
            {
                hourglassCursorTexture = Resources.Load<Texture2D>("hourglass-cursor");
            }
            Cursor.SetCursor(hourglassCursorTexture, Vector2.zero, CursorMode.Auto);
        }
    }

    public static void AutodisableDrag(float atWhatTime)
    {
        SetDraggable(false);
    }

    public static void AutoenableDrag(float atWhatTime)
    {
        SetDraggable(true);
    }
}
