using UnityEngine;
using System.Collections;
using System;

public class Fire : MonoBehaviour
{
    public bool on;
    public int framesPerSwitch;

    private SpriteRenderer sr;

    // Use this for initialization
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Color color = sr.color;
        if (on && color.a < 1f)
        {            
            color.a = Mathf.Clamp(color.a + (1.0f / framesPerSwitch), 0f, 1f);
            sr.color = color;            
        }
        else if (!on && color.a > 0f)
        {
            color.a = Mathf.Clamp(color.a - (1.0f / framesPerSwitch), 0f, 1f);
            sr.color = color;
        }
    }
}
