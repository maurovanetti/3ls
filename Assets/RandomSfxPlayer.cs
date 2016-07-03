using UnityEngine;
using System.Collections;
using System;

public class RandomSfxPlayer : MonoBehaviour {

    [Header("Pause between clip plays")]
    public float minPause;
    public float maxPause;

    [Header("In addition to the default clip:")]
    public AudioClip[] alternateClips;
    [Tooltip("Left: even distribution. Right: always default clip.")]
    [Range(0.0f, 1.0f)]
    public float clipsDistribution;

    private AudioSource a;
    private float nextClipPlayTime;

    // Use this for initialization
    void Start () {
        a = GetComponent<AudioSource>();
        SetNextClipPlayTime();
    }

    // Update is called once per frame
    void Update () {
        if (Time.time > nextClipPlayTime)
        {
            AudioClip clip = a.clip;
            if (UnityEngine.Random.value > clipsDistribution)
            {
                int whichClip = UnityEngine.Random.Range(-1, alternateClips.Length);
                if (whichClip >= 0)
                {
                    clip = alternateClips[whichClip];
                } // else, it's still a.clip
            }
            a.PlayOneShot(clip);
            SetNextClipPlayTime();
        }
	
	}

    private void SetNextClipPlayTime()
    {
        nextClipPlayTime = Time.time + UnityEngine.Random.Range(minPause, maxPause);
    }
}
