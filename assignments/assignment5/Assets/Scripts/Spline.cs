using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Spline : MonoBehaviour
{
    public List<GameObject> knots;
    public GameObject knotPrefab;
    public GameObject movingObjectPrefab;
    public Color lineColor;
    public float timeStep;
    public float animSpeed;

    private GameObject animationTarget;
    private LineRenderer lr;
    private List<Vector3> totalDrawPoints;
    private float animT;
    private int currAnimStep;
    private int animNextKnot;
    private int maxTimeSteps;
    private float currAnimCurveStep;
    private bool playingAnimation;
    private bool crAnimateTarget;       // Is this coroutine currently active

    private void Start()
    {
        playingAnimation = false;
        crAnimateTarget = false;
    }

    void Update()
    {
        knots.Clear();

        maxTimeSteps = (int) (1 / timeStep);

        knots = new List<GameObject>();
        foreach (Transform child in transform)
        {
            knots.Add(child.gameObject);
        }

        totalDrawPoints = new List<Vector3>();

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

            if (!knotScript.isInitiated) knotScript.Initiate(counter, knots.Count);

            counter++;
        }

        for (int i = 1; i < knots.Count; i++)
        {
            if (knots[i].GetComponent<Knot>().knotID != knots.Count)
            {
                totalDrawPoints.AddRange(CubicBezier(knots[i - 1], knots[i]));
            }
        }

        totalDrawPoints.Add(knots[knots.Count - 1].transform.position);
        lr.positionCount = totalDrawPoints.Count;
        lr.SetPositions(totalDrawPoints.ToArray());

        if (playingAnimation && !crAnimateTarget) StartCoroutine(AnimateTarget());
    }
    public void AddKnot()
    {
        GameObject newKnot = GameObject.Instantiate(knotPrefab);
        Vector3 placementDirection = knots[knots.Count - 1].transform.position - knots[knots.Count - 2].transform.position;     // Direction in which the new knot will be placed relative to the last knot
        newKnot.transform.position = knots[knots.Count - 1].transform.position + placementDirection;
        newKnot.transform.parent = transform;

        knots[knots.Count - 1].GetComponent<Knot>().isInitiated = false;

        knots.Add(newKnot);
    }

    public void StartAnimation(Button btn)
    {
        if (!playingAnimation)
        {
            if (animationTarget != null) Destroy(animationTarget);
            playingAnimation = true;
            currAnimStep = 0;
            animNextKnot = 1;
            animT = 0;
            currAnimCurveStep = 0;
            btn.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text = "Stop Animation";
            animationTarget = GameObject.Instantiate(movingObjectPrefab);
            animationTarget.transform.position = totalDrawPoints[0];
        }
        else
        {
            playingAnimation = false;
            btn.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text = "Start Animation";
        }
    }

    // knot1 should always be the starting point with knot2 being the end
    private Vector3[] CubicBezier(GameObject knot1, GameObject knot2)
    {
        List<Vector3> drawPoints = new List<Vector3>();
        List<Vector3> points = new List<Vector3>();
        points.AddRange(TotalControlPoints(knot1, knot2));

        for (float i = 0; i < 1; i += timeStep)
        {
            drawPoints.Add(RecursiveLerp(points, i));
        }

        return drawPoints.ToArray();
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

    private Vector3[] TotalControlPoints(GameObject knot1, GameObject knot2)
    {
        return new Vector3[] {
                knot1.transform.position,
                knot1.GetComponent<Knot>().forwardControlPoint.transform.position,
                knot2.GetComponent<Knot>().backwardControlPoint.transform.position,
                knot2.transform.position
        };
    }

    private Vector3 CubicCurveDerivative(List<Vector3> points, float t)
    {
        return 
            points[0] * (-3 * Mathf.Pow(t, 2) + 6  * t - 3) +
            points[1] * ( 9 * Mathf.Pow(t, 2) - 12 * t + 3) +
            points[2] * (-9 * Mathf.Pow(t, 2) + 6  * t) +
            points[3] * ( 3 * Mathf.Pow(t, 2))
        ;
    }

    private IEnumerator AnimateTarget()
    {
        crAnimateTarget = true;

        float animStep = timeStep / (knots.Count - 1);
        int maxAnimStep = (int) (1 / animStep);
        currAnimStep = (int) (animT * maxAnimStep);

        if (currAnimCurveStep >= 1 + timeStep) currAnimCurveStep = 0;

        List<Vector3> controlPoints = new List<Vector3>();
        controlPoints.AddRange(TotalControlPoints(knots[animNextKnot - 1], knots[animNextKnot]));
        Vector3 ddt = CubicCurveDerivative(controlPoints, currAnimCurveStep);
        ddt = ddt.normalized;

        yield return new WaitForSecondsRealtime(1 / animSpeed);

        animationTarget.transform.position = totalDrawPoints[currAnimStep];
        animationTarget.transform.rotation = Quaternion.LookRotation(ddt, Vector3.up);

        Debug.Log("animT: " + animT + " Step: " + currAnimStep + " V: " + ddt);

        animT += animStep;
        currAnimCurveStep += timeStep;
        if (currAnimCurveStep >= 1 + timeStep) animNextKnot++;

        if (animT > 1 + animStep)
        {
            yield return new WaitForSecondsRealtime(1 / animSpeed);
            animationTarget.transform.position = totalDrawPoints[currAnimStep + 1];

            playingAnimation = false;
            GameObject.FindGameObjectWithTag("Canvas").transform.Find("Start Animation").Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text = "Start Animation";
        }

        crAnimateTarget = false;
    }
}
