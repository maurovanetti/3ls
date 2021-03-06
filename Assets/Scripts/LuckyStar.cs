﻿using UnityEngine;
using System.Collections;
using System;

public class LuckyStar : RotatingStar {

    private static bool alarmsSet;
    private static bool draggable;
    private static bool byeBye;
    private static Texture2D hourglassCursorTexture;

    public float springStrength;
    public AudioClip dragStartSound;

    private bool dragged;
    private Rigidbody2D rb;
    private GameObject[] constellation;
    

    // Use this for initialization
    new void Start()
    {
        base.Start();
        byeBye = false;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
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
    new void Update () {
        if (byeBye)
        {
            rb.gravityScale = -10f;
        }
        if (dragged)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = -Camera.main.transform.position.z;
            Vector3 dragTo = Camera.main.ScreenToWorldPoint(mousePosition);
            dragTo.z = t.position.z;
            t.position = dragTo;
        }
        else
        {
            base.Update();
            BeAttracted();
        }
	}

    void OnMouseDrag()
    {
        if (draggable)
        {
            if (!dragged) // first frame of dragging
            {
                GetComponent<AudioSource>().PlayOneShot(dragStartSound);
            }
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

    public static void Clear()
    {
        SetDraggable(true);
        alarmsSet = false;
    }

    public static void ByeBye()
    {
        byeBye = true;
    }

    public static void Wow()
    {
        wow = true;
    }
}
