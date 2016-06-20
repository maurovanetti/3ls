using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Camp : MonoBehaviour {

    public List<Camp> neighbourhood;

    public bool Selected
    {
        get; private set;
    }

    public bool VisitedByFugitive
    {
        get; private set;
    }

    // Use this for initialization
    void Start () {
        foreach (Camp neighbour in neighbourhood)
        {
            if (!neighbour.neighbourhood.Contains(this))
            {
                neighbour.neighbourhood.Add(this);
            }
            GetComponentInChildren<Star>().Connect(neighbour.GetComponentInChildren<Star>());
        }
    }

    // Update is called once per frame
    void Update () {
	
	}

    internal void UnselectIfNecessary(LuckyStar detachingLuckyStar)
    {
        LuckyStar[] attachedLuckyStars = GetComponentsInChildren<LuckyStar>();
        if ((attachedLuckyStars.Length == 0) || (attachedLuckyStars.Length == 1 && attachedLuckyStars[0] == detachingLuckyStar))
        {
            Selected = false;
            GetComponentInChildren<Fire>().on = false;
        }
    }

    public void Select()
    {
        Selected = true;
        GetComponentInChildren<Fire>().on = true;
    }

    public void Visit(Fugitive visitor)
    {
        VisitedByFugitive = true;
        transform.Find("Fugitive Icon").gameObject.SetActive(true);
    }
}
