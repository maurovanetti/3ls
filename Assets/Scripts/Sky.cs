using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Sky : MonoBehaviour
{

    public static readonly float SunshineTime = 6.0f; // 6 am
    public static readonly float SunsetTime = 18.0f; // 6 pm
    public static readonly float TwilightDuration = 2f; // 2 hours before, 2 after
    float dawnStart, dawnEnd, duskStart, duskEnd;

    public delegate void Alarm(float timeOfTheDay);

    public float whatTimeIsIt;
    public float daysPerSecond;

    private float whatTimeWasIt;
    private Dictionary<float, List<Alarm>> alarms = new Dictionary<float, List<Alarm>>();

    private SpriteRenderer nightSky;
    private SpriteRenderer sunshineFlare;
    private SpriteRenderer daySky;
    private SpriteRenderer sunsetFlare;

    // Use this for initialization
    void Start()
    {
        whatTimeWasIt = whatTimeIsIt;
        nightSky = transform.Find("Night Sky").GetComponent<SpriteRenderer>();
        sunshineFlare = transform.Find("Sunshine Flare").GetComponent<SpriteRenderer>();
        daySky = transform.Find("Day Sky").GetComponent<SpriteRenderer>();
        sunsetFlare = transform.Find("Sunset Flare").GetComponent<SpriteRenderer>();
        dawnStart = SunshineTime - TwilightDuration;
        dawnEnd = SunshineTime + TwilightDuration;
        duskStart = SunsetTime - TwilightDuration;
        duskEnd = SunsetTime + TwilightDuration;
    }

    // Update is called once per frame
    void Update()
    {
        whatTimeWasIt = whatTimeIsIt;
        whatTimeIsIt += Time.deltaTime * daysPerSecond * 24;
        foreach (KeyValuePair<float, List<Alarm>> alarmsCluster in alarms)
        {
            float alarmsClusterTime = alarmsCluster.Key;
            float alarmsClusterTimeAnd24 = alarmsCluster.Key + 24f;
            if ((whatTimeWasIt < alarmsClusterTime && alarmsClusterTime <= whatTimeIsIt) ||
                (whatTimeWasIt < alarmsClusterTimeAnd24 && alarmsClusterTimeAnd24 <= whatTimeIsIt))
            {
                foreach (Alarm alarm in alarmsCluster.Value)
                {
                    alarm(alarmsClusterTime); // RINGS!
                }
            }
        }
        whatTimeIsIt %= 24f;

        float nightAlpha = 0f;
        float sunshineFlareAlpha = 0f;
        float dayAlpha = 0f;
        float sunsetFlareAlpha = 0f;

        // Day and night
        if (whatTimeIsIt < SunshineTime)
        {
            nightAlpha = 1.0f;
        }
        else if (whatTimeIsIt < dawnEnd)
        {
            nightAlpha = Mathf.Lerp(1.0f, 0f, (whatTimeIsIt - SunshineTime) / TwilightDuration);
            dayAlpha = 1.0f - nightAlpha;
        }
        else if (whatTimeIsIt < SunsetTime)
        {
            dayAlpha = 1.0f;
        }
        else if (whatTimeIsIt < duskEnd)
        {
            dayAlpha = Mathf.Lerp(1.0f, 0f, (whatTimeIsIt - SunsetTime) / TwilightDuration);
            nightAlpha = 1.0f - dayAlpha;
        }
        else
        {
            nightAlpha = 1.0f;
        }

        // Twilight flares
        if (whatTimeIsIt > dawnStart && whatTimeIsIt < dawnEnd)
        {
            sunshineFlareAlpha = Mathf.Lerp(1.0f, 0f, Mathf.Abs(whatTimeIsIt - SunshineTime) / TwilightDuration);
        }
        else if (whatTimeIsIt > duskStart && whatTimeIsIt < duskEnd)
        {
            sunsetFlareAlpha = Mathf.Lerp(1.0f, 0f, Mathf.Abs(whatTimeIsIt - SunsetTime) / TwilightDuration);
        }

        SetAlphas(nightAlpha, sunshineFlareAlpha, dayAlpha, sunsetFlareAlpha);
    }

    public void Freeze()
    {
        daysPerSecond = 0f;
    }

    private void SetAlphas(float nightAlpha, float sunshineFlareAlpha, float dayAlpha, float sunsetFlareAlpha)
    {
        SetAlpha(nightSky, nightAlpha);
        SetAlpha(sunshineFlare, sunshineFlareAlpha);
        SetAlpha(daySky, dayAlpha);
        SetAlpha(sunsetFlare, sunsetFlareAlpha);
    }

    private void SetAlpha(SpriteRenderer spriteRenderer, float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    public void SetAlarm(float atWhatTime, Alarm alarm)
    {
        if (!alarms.ContainsKey(atWhatTime))
        {
            alarms.Add(atWhatTime, new List<Alarm>(1));
        }
        List<Alarm> alarmsAtThatTime = alarms[atWhatTime];
        alarmsAtThatTime.Add(alarm);
    }

    public void UnsetAlarm(float atWhatTime, Alarm alarm)
    {
        if (alarms.ContainsKey(atWhatTime))
        {
            List<Alarm> alarmsAtThatTime = alarms[atWhatTime];
            alarmsAtThatTime.Remove(alarm);
        }
    }

    public static Sky GetSky()
    {
        return GameObject.Find("Sky").GetComponent<Sky>();
    }

}
