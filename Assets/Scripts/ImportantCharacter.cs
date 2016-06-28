using System.Collections.Generic;
using UnityEngine;

public abstract class ImportantCharacter : MonoBehaviour
{
    public float delayAfterSunshine;
    public float speed;
    public Camp previousCamp;
    public Camp camp;
    public string iconName;

    protected Transform t;
    private List<ImportantCharacter> otherCharacters; // cache
    

    // Use this for initialization
    void Start()
    {
        t = transform;
        Sky.GetSky().SetAlarm(Sky.SunshineTime + delayAfterSunshine, TakeDecision);
        camp = GetComponentInParent<Camp>();
    }

    // Update is called once per frame
    void Update()
    {
        if (t.localPosition.magnitude < speed)
        {
            t.localPosition = Vector3.zero;
            camp.Visit(this);
        }
        else
        {
            t.localPosition = t.localPosition.normalized * (t.localPosition.magnitude - speed);
        }
        CheckCollisions();
    }

    protected abstract void CheckCollisions();

    protected void Choose()
    {
        t.parent = camp.transform.Find(Camp.RestingAreaName);
        camp.SetChosenBy(this);
    }

    public abstract void TakeDecision(float startTime);
    
    public void StopMoving()
    {
        speed = 0f;
    }

    protected List<ImportantCharacter> GetOtherCharacters()
    {
        if (otherCharacters == null)
        {
            GameObject[] characterObjects = GameObject.FindGameObjectsWithTag("Character");
            otherCharacters = new List<ImportantCharacter>(2);
            foreach (GameObject characterObject in characterObjects)
            {
                ImportantCharacter character = characterObject.GetComponent<ImportantCharacter>();
                if (character != this)
                {
                    otherCharacters.Add(character);
                }
            }
        }
        return otherCharacters;
    }
}