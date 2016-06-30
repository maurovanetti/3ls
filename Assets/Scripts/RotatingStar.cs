using UnityEngine;

public class RotatingStar : MonoBehaviour
{
    protected static bool wow;

    public float degreesPerFrame;
    protected Vector3 rotationPerFrame;
    

    protected Transform t;

    protected void Start()
    {
        wow = false;
        t = transform;
        rotationPerFrame = new Vector3(0f, 0f, degreesPerFrame);
    }

    protected void Update()
    {
        Spin();
    }

    protected void Spin()
    {
        t.Rotate(rotationPerFrame * (wow ? 30 : 1));
    }

}