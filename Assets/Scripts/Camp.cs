﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Camp : MonoBehaviour
{
    public static readonly string RestingAreaName = "Resting Area";

    public List<Camp> neighbourhood;

    public bool Selected
    {
        get; private set;
    }

    public bool ChosenByEnemy
    {
        get; private set;
    }

    private HashSet<ImportantCharacter> visitors;


    // Use this for initialization
    void Start()
    {
        foreach (Camp neighbour in neighbourhood)
        {
            if (!neighbour.neighbourhood.Contains(this))
            {
                neighbour.neighbourhood.Add(this);
            }
            GetComponentInChildren<Star>().Connect(neighbour.GetComponentInChildren<Star>());
        }
        visitors = new HashSet<ImportantCharacter>();
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, transform.position - Camera.main.transform.position);
        Debug.DrawRay(Camera.main.transform.position, transform.position - Camera.main.transform.position, Color.green, 1.0f);
        if (Physics.Raycast(ray, out hit, 400f, ~(1 << LayerMask.NameToLayer("Ignore Raycast"))))
        {
            transform.Find(RestingAreaName).transform.position = hit.point;
        }
        Sky sky = Sky.GetSky();
        sky.SetAlarm(0f, ResetChosenByEnemy);
    }

    // Update is called once per frame
    void Update()
    {

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

    public bool IsVisitedBy(ImportantCharacter who)
    {
        return visitors.Contains(who);
    }

    public void Select()
    {
        Selected = true;
        GetComponentInChildren<Fire>().on = true;
    }

    public void SetChosenBy(ImportantCharacter who)
    {
        if (who is Enemy)
        {
            ChosenByEnemy = true;
        }
    }

    private void ResetChosenByEnemy(float timeOfTheDay)
    {
        ChosenByEnemy = false;
    }

    public void Visit(ImportantCharacter visitor)
    {

        visitors.Add(visitor);
        transform.Find(RestingAreaName + "/" + visitor.iconName).gameObject.SetActive(true);

    }
}
