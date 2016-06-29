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
            else
            {
                Director.GetDirector().Notify(Director.PlotEvent.EnemiesKeepSeparated, this);
            }
        }

        // Rule #2
        if (candidates.Count == 1)
        {
            camp = candidates[0];
            Director.GetDirector().Notify(Director.PlotEvent.EnemyOneChoice, this);
        }
        else
        {
            // Rule #3
            if (candidates.Remove(previousPreviousCamp))
            {
                Director.GetDirector().Notify(Director.PlotEvent.EnemiesDontWalkBack, this);
            }

            // Rule #4
            List<Camp> unvisited = candidates.FindAll(candidate => !candidate.IsVisitedBy(this));
            if (unvisited.Count > 0) {
                candidates = unvisited;
                if (unvisited.Count == 1)
                {
                    Director.GetDirector().Notify(Director.PlotEvent.EnemyOneUnvisited, this);
                }
                else
                {
                    Director.GetDirector().Notify(Director.PlotEvent.EnemyUnvisited, this);
                }
            }

            // Rule #5
            List<Camp> selected = candidates.FindAll(candidate => candidate.Selected);
            if (selected.Count > 0)
            {
                candidates = selected;
                Director.GetDirector().Notify(Director.PlotEvent.EnemyFires, this);
            }
        }

        // Rule #6
        if (candidates.Count > 0)
        {
            int i = UnityEngine.Random.Range(0, candidates.Count);
            camp = candidates[i];
            if (candidates.Count > 1)
            {
                Director.GetDirector().Notify(Director.PlotEvent.EnemyRandomPick, this);
            }
        }

        Choose();
    }

    protected override void CheckCollisions()
    {
        // Right now, we don't care
    }
}
