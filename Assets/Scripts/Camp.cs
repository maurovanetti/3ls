using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Camp : MonoBehaviour
{

    public List<Camp> neighbourhood;

    public bool Selected
    {
        get; private set;
    }

    public bool ChosenByEnemy
    {
        get; private set;
    }

    private List<ImportantCharacter> visitors;

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
        visitors = new List<ImportantCharacter>();
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

    public void Visit(ImportantCharacter visitor)
    {

        visitors.Add(visitor);
        if (visitor is Enemy)
        {
            ChosenByEnemy = false;
        }
        else
        {
            transform.Find("Fugitive Icon").gameObject.SetActive(true);
        }

    }
}
