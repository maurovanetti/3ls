using UnityEngine;
using System.Collections;
using System;

public class Fire : MonoBehaviour
{
    public bool on;
    public int framesPerSwitch;

    private SpriteRenderer sr;
    private Transform flame;
    private float flameSize;
    private GameObject smoke;

    // Use this for initialization
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        flame = transform.Find("Flame");
        if (flame != null)
        {
            flameSize = flame.localScale.x;
            smoke = flame.Find("Smoke").gameObject;
            if (!on)
            {
                flame.localScale = Vector3.zero;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sr != null)
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
        if (flame != null)
        {
            if (on && flame.localScale.x < flameSize)
            {
                flame.localScale = Vector3.one * Mathf.Clamp(flame.localScale.x + (flameSize / framesPerSwitch), 0f, flameSize);
            }
            else if (!on && flame.localScale.x > 0f)
            {
                flame.localScale = Vector3.one * Mathf.Clamp(flame.localScale.x - (flameSize / framesPerSwitch), 0f, flameSize);
            }
            smoke.SetActive(on);
        }
    }
}
