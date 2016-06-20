using UnityEngine;
using System.Collections;
using System;

public class Fugitive : MonoBehaviour
{

    public float speed;
    public Camp previousCamp;
    public Camp camp;

    private Transform t;

    // Use this for initialization
    void Start()
    {
        t = transform;
        Sky.GetSky().SetAlarm(Sky.SunshineTime + 1f, TakeDecision);
        camp = GetComponentInParent<Camp>();
    }

    // Update is called once per frame
    void Update()
    {
        if (t.localPosition.magnitude < speed)
        {
            t.localPosition = Vector3.zero;
            camp.Visit(this);
        }
        else
        {
            t.localPosition = t.localPosition.normalized * (t.localPosition.magnitude - speed);
        }
    }

    public void TakeDecision(float startTime)
    {
        previousCamp = camp;

        // Rule #1
        if (previousCamp.neighbourhood.Count == 1)
        {
            camp = previousCamp.neighbourhood[0];
        }
        else
        {
            // Rule #2
            foreach (Camp neighbour in previousCamp.neighbourhood)
            {
                if (neighbour.Selected && !neighbour.VisitedByFugitive)
                {
                    if (camp == previousCamp) // first good option found
                    {
                        camp = neighbour;
                    }
                    else
                    {
                        camp = null;
                    }
                }
            }
            if (camp == null)
            {
                camp = previousCamp;
            }

            // Rule #3
            if (camp == previousCamp)
            {
                foreach (Camp neighbour in previousCamp.neighbourhood)
                {
                    if (neighbour.Selected)
                    {
                        if (camp == previousCamp) // first good option found
                        {
                            camp = neighbour;
                        }
                        else
                        {
                            camp = null;
                        }
                    }
                }
                if (camp == null)
                {
                    camp = previousCamp;
                }
            }
        }
        t.parent = camp.transform.Find("Resting Area");
    }
}
