using UnityEngine;

public class RotatingStar : MonoBehaviour
{
    public float degreesPerFrame;
    private Vector3 rotationPerFrame;

    protected Transform t;

    protected void Start()
    {
        t = transform;
        rotationPerFrame = new Vector3(0f, 0f, degreesPerFrame);
    }

    protected void Update()
    {
        Spin();
    }

    protected void Spin()
    {
        t.Rotate(rotationPerFrame);
    }

}