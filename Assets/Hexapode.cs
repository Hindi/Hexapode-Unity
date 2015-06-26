using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hexapode : MonoBehaviour 
{
    [SerializeField]
    private GameObject goalSphere;

    [SerializeField]
    private Leg leg1;
    [SerializeField]
    private Leg leg2;
    [SerializeField]
    private Leg leg3;
    [SerializeField]
    private Leg leg4;
    [SerializeField]
    private Leg leg5;
    [SerializeField]
    private Leg leg6;

    List<Leg> legs;

	// Use this for initialization
    void Start()
    {
        legs = new List<Leg>();

        legs.Add(leg1);
        legs.Add(leg2);
        legs.Add(leg3);
        legs.Add(leg4);
        legs.Add(leg5);
        legs.Add(leg6);

        leg1.initRelativePosition(leg1.transform.position, 0);
        leg2.initRelativePosition(leg2.transform.position, -60);
        leg3.initRelativePosition(leg3.transform.position, -120);
        leg4.initRelativePosition(leg4.transform.position, -180);
        leg5.initRelativePosition(leg5.transform.position, -240);
        leg6.initRelativePosition(leg6.transform.position, -300);

        StartCoroutine(goToPositionCoroutine());
    }
	
	// Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
	}

    private bool legsReachGoal()
    {
        foreach (Leg l in legs)
            if (!l.hasReachedGoal())
                return false;
        return true;
    }

    private void calcGoal()
    {
        legs.ForEach(l => l.setDirection(goalSphere.transform.localPosition.normalized));
    }

    IEnumerator goToPositionCoroutine()
    {
        while (true)
        {
            /*leg1.goTo(new Vector3(Random.Range(-3, 3), Random.Range(0, -3), Random.Range(8, 12)));
            leg2.goTo(new Vector3(Random.Range(6, 9), Random.Range(0, -3), Random.Range(0, 5)));
            leg3.goTo(new Vector3(Random.Range(6, 9), Random.Range(0, -3), Random.Range(0, -5)));
            leg4.goTo(new Vector3(Random.Range(-3, 3), Random.Range(0, -3), Random.Range(-8, -12)));
            leg5.goTo(new Vector3(Random.Range(-6, -9), Random.Range(0, -3), Random.Range(0, -5)));
            leg6.goTo(new Vector3(Random.Range(-6, -9), Random.Range(0, -3), Random.Range(0, 5)));*/

            calcGoal();
            legs.ForEach(l => l.goToStartMovement());
            while (!legsReachGoal())
                yield return new WaitForSeconds(0.1f);
            legs.ForEach(l => l.goToEndMovement());
            while (!legsReachGoal())
                yield return new WaitForSeconds(0.1f);
            legs.ForEach(l => l.goToIdlePosition());

            yield return new WaitForSeconds(0.5f);
        }
    }
}
