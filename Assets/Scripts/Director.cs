using System;
using UnityEngine;
using UnityEngine.UI;

public class Director : MonoBehaviour
{

    public Text text;

    void Start()
    {

    }

    void Update()
    {

    }

    public static Director GetDirector()
    {
        return GameObject.Find("Director").GetComponent<Director>();
    }

    public void FugitiveCaptured(Enemy enemy)
    {
        text.text = "He caught me. But I'll sell my life dearly!";
    }
}