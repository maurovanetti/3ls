using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Director : MonoBehaviour
{

    public enum PlotEvent
    {
        FugitiveCaptured,
        AllCampsVisitedByFugitive,
        AllCampsVisitedByEnemy,
        FugitiveUndecided,
        FugitiveOneChoice
    }

    public Text textAbove;
    public Text textBelow;

    private Dictionary<PlotEvent, ImportantCharacter> recentNews;
    private int storyEpisode;

    private string[] story = new string[]
    {
        "They're after me.",
        "I should follow my lucky stars.",
        "I am cold.",
        "I am thirsty.",
        "I am hungry."
    };


    void Start()
    {
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

    }

    public static Director GetDirector()
    {
        return GameObject.Find("Director").GetComponent<Director>();
    }

    public void Notify(PlotEvent plotEvent, ImportantCharacter enemy)
    {
        Debug.Log("Notified plot event " + plotEvent.ToString());
        recentNews.Add(plotEvent, enemy);
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
        }
        else
        {            
            if (storyEpisode < story.Length)
            {
                SetMessageAbove(story[storyEpisode++]);
            }
            else
            {
                HideMessageAbove(timeOfTheDay);
            }
        }
    }

    private void ShowMessageBelow(float timeOfTheDay)
    {
        if (IsHappened(PlotEvent.FugitiveUndecided))
        {
            SetMessageBelow("I'm scared. Where should I go?");
        }
        recentNews.Clear();
    }

    private void SetMessageAbove(string s)
    {
        textAbove.text = s;
    }

    private void SetMessageBelow(string s)
    {
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