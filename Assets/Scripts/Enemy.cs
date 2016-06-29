using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Enemy : ImportantCharacter
{
    private Camp previousPreviousCamp;

    public override void TakeDecision(float startTime)
    {
        if (previousCamp == null)
        {
            previousPreviousCamp = camp; // 1st turn
        }
        else
        {
            previousPreviousCamp = previousCamp; // following turns
        }
        previousCamp = camp;
        List<Camp> candidates = new List<Camp>(previousCamp.neighbourhood.Count);

        // Rule #1
        foreach (Camp neighbour in previousCamp.neighbourhood)
        {
            if (!neighbour.ChosenByEnemy)
            {
                candidates.Add(neighbour);
            }
        }

        // Rule #2
        if (candidates.Count == 1)
        {
            camp = candidates[0];
        }
        else
        {
            // Rule #3
            candidates.Remove(previousPreviousCamp);

            // Rule #4
            List<Camp> unvisited = candidates.FindAll(candidate => !candidate.IsVisitedBy(this));
            if (unvisited.Count > 0) {
                candidates = unvisited;
            }

            // Rule #5
            List<Camp> selected = candidates.FindAll(candidate => candidate.Selected);
            if (selected.Count > 0)
            {
                candidates = selected;
            }
        }

        // Rule #6
        if (candidates.Count > 0)
        {
            int i = UnityEngine.Random.Range(0, candidates.Count);
            camp = candidates[i];
        }

        Choose();
    }

    protected override void CheckCollisions()
    {
        // Right now, we don't care
    }
}
