using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spline : MonoBehaviour
{
    public List<GameObject> knots;
    public float timeStep;

    private int index;
    private int countTimeSteps;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject knot in knots)
        {
            if (knot.GetComponent<Knot>().knotID != knots.Count)
            {
                CubicBezier(knot, knots[index + 1]);
            }
            index++;
        }

        countTimeSteps++;
        if (countTimeSteps >= 1) countTimeSteps = 0;
    }

    private void CubicBezier(GameObject knot1, GameObject knot2)
    {
        //if (knot1.GetComponent<Knot>().controlPoints.Count == 1) Vector3 point = Vector3.Lerp(knot1.transform.position, knot1.GetComponent<Knot>().controlPoints[0].transform.position, timeStep);
    }
}
