using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Star : MonoBehaviour
{
    public GameObject lineBetweenStarsPrefab;
    private List<Star> connected = new List<Star>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Connect(Star star)
    {
        if (!connected.Contains(star))
        {
            GameObject line = Instantiate(lineBetweenStarsPrefab, this.transform.position, Quaternion.identity) as GameObject;
            line.GetComponent<LineRenderer>().SetPositions(new Vector3[] {
                this.transform.position + (Vector3.forward * 0.1f),
                star.transform.position + (Vector3.forward * 0.1f)
            });
            line.name = "Link";
            this.connected.Add(star);
            star.connected.Add(this);
        }
    }
}
