using UnityEngine;
using System.Collections;

public class AX12 : MonoBehaviour {

    [SerializeField]
    private Vector3 axis;

    [SerializeField]
    private Transform rotatingPart;

    private int rotation = 0;
    private bool reachedTarget = true;
    private float speed = 10f;

    public void SetGoal(int goal)
    {
        rotation = goal - 135;

        rotatingPart.localRotation = Quaternion.Euler(axis * rotation);
        /*StopAllCoroutines();
        StartCoroutine(gotoCoroutine());*/
    }

    IEnumerator gotoCoroutine()
    {
        Vector3 startRot = rotatingPart.rotation.eulerAngles;
        Vector3 previousRot = startRot;
        float startTime = Time.time;
        reachedTarget = false;

        while (!reachedTarget)
        {
            yield return null;
            rotatingPart.rotation = Quaternion.Euler(Vector3.Lerp(startRot, axis * rotation, (Time.time - startTime) * speed));

            if (previousRot == rotatingPart.rotation.eulerAngles)
            {
                reachedTarget = true;
                break;
            }
            previousRot = rotatingPart.rotation.eulerAngles;
        }
    }
}
