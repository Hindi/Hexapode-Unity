using UnityEngine;
using System.Collections;

public class Servo : MonoBehaviour 
{
    Quaternion goal;
    Vector3 goalEuler;
    Vector3 rotation = Vector3.zero;//Keep track of the "real" angle

    private bool reachedTarget;
    public bool ReachedTarget
    { get { return reachedTarget; } }

    public void goTo(Vector3 angle)
    {
        goalEuler = angle;
        goal = Quaternion.Euler(angle);
        StopAllCoroutines();
        StartCoroutine(gotoCoroutine());
    }

    IEnumerator gotoCoroutine()
    {
        transform.localRotation = Quaternion.Euler(rotation);
        Vector3 startRot = rotation;
        Vector3 previousRot = startRot;
        float startTime = Time.time;
        reachedTarget = false;

        while (true)
        {
            yield return null;
            rotation = Vector3.Lerp(startRot, goalEuler, Time.time - startTime);
            transform.localRotation = Quaternion.Euler(rotation);

            if(previousRot == rotation)
            {
                reachedTarget = true;
                break;
            }
            previousRot = rotation;
        }
    }
}