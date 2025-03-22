using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Knot : MonoBehaviour
{
    public GameObject forwardControlPoint;      // Control point for the curve ahead of this knot
    public GameObject backwardControlPoint;     // Control point for the curve behind this knot
    public GameObject controlPointPrefab;
    public Vector3 defaultControlPointPos;
    public int knotID;
    
    [HideInInspector] public bool wasInitiated;
    [HideInInspector] public bool isConnectingPoint;       // If true, this knot is not at either end of the line and therefore is a connecting point between curves.

    private GameObject spline;

    void Start()
    {
        wasInitiated = false;
    }
    private void Update()
    {
        UpdateStats();
    }
    public void Initiate(int id, int knotCount)
    {
        //spline = GameObject.FindGameObjectWithTag("Curve");
        //spline.GetComponent<Spline>().knots.Add(gameObject);

        //int knotCount = spline.GetComponent<Spline>().knots.Count - 1;
        //knotID = knotCount;
        wasInitiated = true;
        knotID = id;

        GameObject newControlPoint;
        if (knotID == 0 || knotID == knotCount - 1)
        {
            isConnectingPoint = false;

            newControlPoint = GameObject.Instantiate(controlPointPrefab);
            newControlPoint.transform.SetParent(transform);

            if (knotID == 0)
            {
                newControlPoint.transform.localPosition = defaultControlPointPos;
                forwardControlPoint = newControlPoint;
            }
            if (knotID == knotCount - 1)
            {
                newControlPoint.transform.localPosition = -defaultControlPointPos;
                backwardControlPoint = newControlPoint;
            }
        }
        else
        {
            isConnectingPoint = true;

            newControlPoint = GameObject.Instantiate(controlPointPrefab);
            newControlPoint.transform.SetParent(transform);
            newControlPoint.transform.localPosition = defaultControlPointPos;
            forwardControlPoint = newControlPoint;

            newControlPoint = GameObject.Instantiate(controlPointPrefab);
            newControlPoint.transform.SetParent(transform);
            newControlPoint.transform.localPosition = -defaultControlPointPos;
            backwardControlPoint = newControlPoint;
        }
    }
    void UpdateStats()
    {
        float scaleFactor = transform.localScale.x;

        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        if (transform.localScale.x < 0) transform.localScale = Vector3.one / 100;

        foreach (Transform child in transform)
        {
            child.localScale = Vector3.one * (1/transform.localScale.x);
        }
    }
}
