using System.Collections.Generic;
using UnityEngine;

public abstract class ImportantCharacter : MonoBehaviour
{
    public string surname;
    public float delayAfterSunshine;
    public float speed;
    public Camp previousCamp;
    public Camp camp;
    public string iconName;
    public AudioClip newCampVisitedSound;

    protected Transform t;
    private List<ImportantCharacter> otherCharacters; // cache
    private int campsToVisit;

    private float deltaZ;


    // Use this for initialization
    void Start()
    {
        campsToVisit = GameObject.FindGameObjectsWithTag("Constellation").Length;
        Debug.Log(campsToVisit + " camps to visit");
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
            if (!camp.IsVisitedBy(this))
            {
                camp.Visit(this);
                OnVisitNewCamp(camp);
                campsToVisit--;
                if (campsToVisit == 0)
                {
                    if (this is Enemy)
                    {
                        Director.GetDirector().Notify(Director.PlotEvent.AllCampsVisitedByEnemy, this);
                    }
                    else
                    {
                        if (!camp.ChosenByEnemy)
                        {
                            Director.GetDirector().Notify(Director.EndingEvent.AllCampsVisitedByFugitive, this);
                            Sky.GetSky().Freeze();
                        }
                    }
                }
                deltaZ = UnityEngine.Random.Range(-0.5f, +0.5f);
            }
        }
        else
        {
            t.localPosition = t.localPosition.normalized * (t.localPosition.magnitude - speed);
            Vector3 newRotation = t.GetChild(0).localRotation.eulerAngles;
            newRotation.z += deltaZ;
            t.GetChild(0).localRotation = Quaternion.Euler(newRotation);
        }
        if (speed > 0f)
        {
            CheckCollisions();
        }
    }


    protected void OnVisitNewCamp(Camp camp)
    {
        if (newCampVisitedSound != null)
        {
            GetComponent<AudioSource>().PlayOneShot(newCampVisitedSound);
        }
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