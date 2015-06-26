using UnityEngine;
using System.Collections;

public class Leg : MonoBehaviour 
{
    [SerializeField]
    private Servo gamma;
    [SerializeField]
    private Servo alpha;
    [SerializeField]
    private Servo beta;
    [SerializeField]
    private GameObject tibia;
    [SerializeField]
    private GameObject femur;
    [SerializeField]
    private Transform sphereGoal;
    [SerializeField]
    private Transform startSphere;
    [SerializeField]
    private Transform endSphere;
    [SerializeField]
    private Transform gotoDirCenterTransform;

    private float tibiaLength;
    private float femurLength;
    private float angleToHexapod;
    private float speed;

    Vector3 goal;
    Vector3 distanceToHexapod;
    Vector3 gotoDirCenterPosition;

    Vector3 startMovement;
    Vector3 endMovement;
    Vector3 idlePosition;

	// Use this for initialization
    void Awake()
    {
        speed = 1;
        tibiaLength = 4f;
        femurLength = 4f;
        gotoDirCenterPosition = gotoDirCenterTransform.position;
	}

    void Update()
    {
        if (startMovement != null && endMovement != null && gotoDirCenterTransform != null)
        {
            Debug.DrawLine(startMovement, endMovement);
        }
    }

    public void initRelativePosition(Vector3 dist, float angle)
    {
        distanceToHexapod = dist;
        angleToHexapod = Mathf.Deg2Rad * angle;
    }

    private Vector3 hexapodSpaceToLocalSpace(Vector3 pos)
    {
        Vector3 localPos;
        pos -= distanceToHexapod;
        localPos.x = Mathf.Cos(angleToHexapod) * pos.x + Mathf.Sin(angleToHexapod) * pos.z;
        localPos.z = Mathf.Cos(angleToHexapod) * pos.z - Mathf.Sin(angleToHexapod) * pos.x;
        localPos.y = pos.y;

        return localPos;
    }

    public void setDirection(Vector3 dir)
    {
        startMovement = hexapodSpaceToLocalSpace(gotoDirCenterPosition + dir * speed);
        endMovement = hexapodSpaceToLocalSpace(gotoDirCenterPosition - dir * speed);
        idlePosition = hexapodSpaceToLocalSpace(gotoDirCenterPosition + new Vector3(0, 3, 0));

        startSphere.transform.localPosition = startMovement;
        endSphere.transform.localPosition = endMovement;
    }

    public void goToStartMovement()
    {
        goTo(startMovement, false);
    }

    public void goToEndMovement()
    {
        goTo(endMovement, false);
    }

    public void goToIdlePosition()
    {
        goTo(idlePosition, false);
    }

    public void goTo(Vector3 pos, bool convertSpace = true)
    {
        if(convertSpace)
            pos = hexapodSpaceToLocalSpace(pos);
        goal = pos;
        sphereGoal.localPosition = goal;
        float distanceToTarget = Vector3.Distance(goal, Vector3.zero);

        float alpha1 = Mathf.Acos(-goal.y / distanceToTarget);
        float alpha2 = Mathf.Acos((Mathf.Pow(tibiaLength, 2) - Mathf.Pow(femurLength, 2) - Mathf.Pow(distanceToTarget, 2)) / (-2 * femurLength * distanceToTarget));
        float beta1 = Mathf.Acos((Mathf.Pow(distanceToTarget, 2) - Mathf.Pow(tibiaLength, 2) - Mathf.Pow(femurLength, 2)) / (-2 * tibiaLength * femurLength));

        float gammaGoal = Mathf.Rad2Deg * Mathf.Atan2(goal.x, goal.z);
        float alphaGoal = -Mathf.Rad2Deg * (alpha1 + alpha2 - Mathf.PI/2);
        float betaGoal = Mathf.Rad2Deg * (-beta1 + Mathf.PI);

        gamma.goTo(new Vector3(0, gammaGoal, 0));
        alpha.goTo(new Vector3(alphaGoal, 0, 0));
        beta.goTo(new Vector3(betaGoal, 0, 0));
    }

    public bool hasReachedGoal()
    {
        return (gamma.ReachedTarget && alpha.ReachedTarget && beta.ReachedTarget);
    }
}
