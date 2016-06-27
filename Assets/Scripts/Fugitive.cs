using UnityEngine;
using System.Collections;
using System;

public class Fugitive : ImportantCharacter
{
    public float collisionDistance;

    public override void TakeDecision(float startTime)
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
                if (neighbour.Selected && !neighbour.IsVisitedBy(this))
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

    protected override void CheckCollisions()
    {
        foreach (ImportantCharacter character in GetOtherCharacters())
        {
            if (character is Enemy)
            {
                if (character.camp == this.camp || (character.camp == this.previousCamp && this.camp == character.previousCamp))
                {
                    if (Vector2.Distance(t.position, character.transform.position) < collisionDistance)
                    {
                        this.StopMoving();
                        character.StopMoving();
                        Sky.GetSky().Freeze();
                        Director.GetDirector().FugitiveCaptured((Enemy) character);
                        break;
                    }
                }
            }
        }
    }
}
