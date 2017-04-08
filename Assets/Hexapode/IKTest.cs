using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTest : MonoBehaviour {

    [SerializeField]
    private float tibiaLength;

    [SerializeField]
    private float femurLength;

    [SerializeField]
    private Transform alphaOrigin;

    Vector3 goal;


    [SerializeField]
    private Servo gamma;
    [SerializeField]
    private Servo alpha;
    [SerializeField]
    private Servo beta;

    [SerializeField]
    Vector3 gotoDirCenterPosition;

    [SerializeField]
    private Transform sphereGoal;

    private float distanceToTarget;
    private float zOffset;

    private void Update()
    {
        gotoDirCenterPosition = sphereGoal.position;
        goal = gotoDirCenterPosition;
        distanceToTarget = Vector3.Distance(goal, alphaOrigin.transform.position);
        zOffset = (alphaOrigin.transform.position.y - goal.y);
        goal.x = -gotoDirCenterPosition.z;
        goal.y = -gotoDirCenterPosition.x;
        goal.z = gotoDirCenterPosition.y;
        Vector3 angles = processIK();
        
        gamma.goToInstant(new Vector3(0, angles.x, 0));
        alpha.goToInstant(new Vector3(0, 0, angles.y));
        beta.goToInstant(new Vector3(0, 0, angles.z));

    }

    private Vector3 processIK()
    {
        float alpha1 = Mathf.Acos(zOffset / distanceToTarget);
        float alpha2 = Mathf.Acos((Mathf.Pow(tibiaLength, 2) - Mathf.Pow(femurLength, 2) - Mathf.Pow(distanceToTarget, 2)) / (-2 * femurLength * distanceToTarget));
        float beta1 = Mathf.Acos((Mathf.Pow(distanceToTarget, 2) - Mathf.Pow(tibiaLength, 2) - Mathf.Pow(femurLength, 2)) / (-2 * tibiaLength * femurLength));

        float gammaGoal = Mathf.Rad2Deg * Mathf.Atan2(goal.x, goal.y);
        float alphaGoal = -Mathf.Rad2Deg * (alpha1 + alpha2);
        float betaGoal = -Mathf.Rad2Deg * (beta1 - Mathf.PI);

        return new Vector3(gammaGoal, alphaGoal, betaGoal);
    }
}
