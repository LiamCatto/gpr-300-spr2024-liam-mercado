using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spline : MonoBehaviour
{
    //public List<GameObject> knots;
    public LineRenderer lr;
    public Color lineColor;
    public float timeStep;

    void Update()
    {
        //knots.Clear();

        List<GameObject> knots = new List<GameObject>();
        foreach (Transform child in transform)
        {
            knots.Add(child.gameObject);
        }
        //knots.AddRange(GameObject.FindGameObjectsWithTag("Knot"));       // NOTE: Adds knots backwards (index 0 is the end knot)

        lr = gameObject.GetComponent<LineRenderer>();
        lr.enabled = true;

        Gradient lineGradient = new Gradient();
        lineGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(lineColor, 0.0f), new GradientColorKey(lineColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
        );
        lr.colorGradient = lineGradient;

        int counter = 0;
        foreach (GameObject knot in knots)
        {
            Knot knotScript = knot.GetComponent<Knot>();

            if (!knotScript.wasInitiated) knotScript.Initiate(counter, knots.Count);

            counter++;
        }

        for (int i = 1; i < knots.Count; i++)
        {
            if (knots[i].GetComponent<Knot>().knotID != knots.Count)
            {
                CubicBezier(knots[i - 1], knots[i]);
            }
        }
    }

    // knot1 should always be the starting point with knot2 being the end
    private void CubicBezier(GameObject knot1, GameObject knot2)
    {
        List<Vector3> drawPoints = new List<Vector3>();
        List<Vector3> points = new List<Vector3>(
            new Vector3[] { 
                knot1.transform.position,
                knot1.GetComponent<Knot>().forwardControlPoint.transform.position,
                knot2.GetComponent<Knot>().backwardControlPoint.transform.position,
                knot2.transform.position
            }
        );

        for (float i = 0; i < 1 / timeStep; i += timeStep)
        {
            drawPoints.Add(RecursiveLerp(points, i));
        }

        lr.positionCount = drawPoints.Count;
        lr.SetPositions(drawPoints.ToArray());
    }

    private Vector3 RecursiveLerp(List<Vector3> points, float t)
    {
        if (points.Count == 1)
        {
            return points[0];
        }

        List<Vector3> newPoints = new List<Vector3>();
        for (int i = 1; i < points.Count; i++)
        {
            newPoints.Add(Vector3.Lerp(points[i - 1], points[i], t));
        }

        return RecursiveLerp(newPoints, t);
    }
}
