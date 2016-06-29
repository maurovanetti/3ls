using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Director : MonoBehaviour
{

    public enum PlotEvent
    {
        FugitiveCaptured,
        AllCampsVisitedByFugitive,
        AllCampsVisitedByEnemy,
        FugitiveUndecided,
        FugitiveOneChoice,
        FugitiveOneUnvisitedFire,
        FugitiveOneFire,
        EnemiesKeepSeparated,
        EnemyOneChoice,
        EnemiesDontWalkBack,
        EnemyUnvisited,
        EnemyOneUnvisited,
        EnemyFires,
        EnemyRandomPick
    }

    public Text textAbove;
    public Text textBelow;

    private Dictionary<PlotEvent, ImportantCharacter> recentNews;
    private int storyEpisode;

    private string[] story = new string[]
    {
        "They're after me.",
        "I should follow my lucky stars.",
        "I must avoid them.",
        "The stars show the path.",
        "They follow the stars too!",
        "I am thirsty.",
        "I am cold.",
        "I am hungry."
    };
    private string lastMessageBelow;
    private string secondLastMessageBelow;
    private bool gameOver;

    void Start()
    {
        gameOver = false;
        storyEpisode = 0;
        recentNews = new Dictionary<PlotEvent, ImportantCharacter>();
        Sky.GetSky().SetAlarm(Sky.SunshineTime + 4f, ShowMessageBelow);
        Sky.GetSky().SetAlarm(0f, HideMessageBelow);
        Sky.GetSky().SetAlarm(Sky.SunsetTime + (Sky.TwilightDuration / 2), ShowMessageAbove);
        Sky.GetSky().SetAlarmAtSkyFreeze(ShowMessageAbove);
        ShowMessageAbove(Sky.GetSky().whatTimeIsIt);
        HideMessageBelow(Sky.GetSky().whatTimeIsIt);
    }

    void Update()
    {
        if (gameOver && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }

    public static Director GetDirector()
    {
        return GameObject.Find("Director").GetComponent<Director>();
    }

    public void Notify(PlotEvent plotEvent, ImportantCharacter who)
    {
        Debug.Log("Notified plot event " + plotEvent.ToString());
        if (!recentNews.ContainsKey(plotEvent)  // first plot event of this kind
            || UnityEngine.Random.value > 0.5f) // both enemies did that, pick one randomly
        {
            recentNews.Remove(plotEvent);
            recentNews.Add(plotEvent, who);
        }
    }

    private void HideMessageAbove(float timeOfTheDay)
    {
        textAbove.text = "";
    }

    private void HideMessageBelow(float timeOfTheDay)
    {
        textBelow.text = "";
    }

    private void ShowMessageAbove(float timeOfTheDay)
    {
        if (IsHappened(PlotEvent.FugitiveCaptured))
        {
            SetMessageAbove("GAME OVER");
            gameOver = true;
            LuckyStar.Clear();
        }
        else
        {    
            SetMessageAbove(story[storyEpisode++ % story.Length]);
        }
    }

    private void ShowMessageBelow(float timeOfTheDay)
    {
        List<string> possibleMessages = new List<string>();

        if (IsHappened(PlotEvent.FugitiveUndecided))
        {
            possibleMessages.Add("I'm confused. Where should I go?");
        }
        if (IsHappened(PlotEvent.FugitiveOneChoice))
        {
            possibleMessages.Add("There's one path from here. No choice.");
        }
        if (IsHappened(PlotEvent.FugitiveOneUnvisitedFire))
        {
            possibleMessages.Add("That fire... I've never been there.");
        }
        if (IsHappened(PlotEvent.FugitiveOneFire))
        {
            possibleMessages.Add("I'll follow the fire.");
        }

        ImportantCharacter enemy;
        if (WhoDid(PlotEvent.EnemyOneChoice, out enemy))
        {
            possibleMessages.Add(enemy.surname + " could only take that path.");
        }
        if (WhoDid(PlotEvent.EnemyOneUnvisited, out enemy))
        {
            possibleMessages.Add(enemy.surname + " had one new camp to explore.");
        }
        if (WhoDid(PlotEvent.EnemyFires, out enemy))
        {
            possibleMessages.Add(enemy.surname + " noticed the fire.");
        }
        if (WhoDid(PlotEvent.EnemyUnvisited, out enemy))
        {
            possibleMessages.Add(enemy.surname + " had new camps to explore.");
        }
        if (IsHappened(PlotEvent.EnemiesKeepSeparated))
        {
            possibleMessages.Add("Hmm, they always remain separated.");
        }
        if (IsHappened(PlotEvent.EnemiesDontWalkBack))
        {
            possibleMessages.Add("They don't walk back.");
        }
        if (IsHappened(PlotEvent.EnemyRandomPick))
        {
            possibleMessages.Add("They move whenever they can.");
        }

        if (possibleMessages.Count > 1)
        {
            possibleMessages.Remove(lastMessageBelow);
        }
        if (possibleMessages.Count > 1)
        {
            possibleMessages.Remove(secondLastMessageBelow);
        }
        if (possibleMessages.Count > 0)
        {
            SetMessageBelow(possibleMessages[0]);
        }
        else // it's very unlikely, perhaps impossible
        {
            HideMessageBelow(timeOfTheDay);
        }
        recentNews.Clear();
    }

    private void SetMessageAbove(string s)
    {
        textAbove.text = s;
    }

    private void SetMessageBelow(string s)
    {
        secondLastMessageBelow = lastMessageBelow;
        lastMessageBelow = s;
        textBelow.text = s;
    }

    private bool IsHappened(PlotEvent plotEvent)
    {
        return recentNews.ContainsKey(plotEvent);
    }

    private bool WhoDid(PlotEvent plotEvent, out ImportantCharacter who)
    {
        return recentNews.TryGetValue(plotEvent, out who);
    }
}