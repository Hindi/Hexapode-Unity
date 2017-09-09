using UnityEngine;
using System.Collections;

public class AX12 : MonoBehaviour
{

    [SerializeField]
    private Vector3 axis;

    [SerializeField]
    private Transform rotatingPart;

    private int rotation = 0;
    private int previousAngle;
    private bool reachedTarget = true;
    private float speed = 1;

    public void SetGoal(int goal)
    {
        previousAngle = rotation;
        rotation = goal - 150;

        //rotatingPart.localRotation = Quaternion.Euler(axis * rotation);
        StopAllCoroutines();
        StartCoroutine(gotoCoroutine());
    }

    IEnumerator gotoCoroutine()
    {
        Quaternion startRot = rotatingPart.rotation;
        Quaternion previousRot = startRot;

        float startTime = Time.time;
        reachedTarget = false;

        while (!reachedTarget)
        {
            yield return null;
            rotatingPart.localRotation = Quaternion.AngleAxis(Mathf.Lerp(previousAngle, rotation, (Time.time - startTime) * speed), axis);

            if (previousRot == rotatingPart.rotation)
            {
                reachedTarget = true;
                break;
            }
            previousRot = rotatingPart.rotation;
        }
    }
}