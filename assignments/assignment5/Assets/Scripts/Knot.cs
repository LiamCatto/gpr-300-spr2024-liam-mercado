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
    
    [HideInInspector] public bool isInitiated;
    [HideInInspector] public bool isConnectingPoint;       // If true, this knot is not at either end of the line and therefore is a connecting point between curves.

    private GameObject spline;
    private void Start()
    {
        isInitiated = false;
    }
    private void Update()
    {
        UpdateStats();
    }
    private void OnDestroy()
    {
        if (GameObject.FindGameObjectWithTag("Curve"))
        {
            List<GameObject> knotList = GameObject.FindGameObjectWithTag("Curve").GetComponent<Spline>().knots;

            foreach (GameObject knot in knotList)
            {
                knot.GetComponent<Knot>().isInitiated = false;
            }
        }
    }
    public void Initiate(int id, int knotCount)
    {
        isInitiated = true;
        knotID = id;

        if (forwardControlPoint != null) Destroy(forwardControlPoint.gameObject);
        if (backwardControlPoint != null) Destroy(backwardControlPoint.gameObject);

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
    private void UpdateStats()
    {
        // Keep all scale values the same as the x value and prevent them from reaching or going below zero
        float scaleFactor = transform.localScale.x;

        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        if (transform.localScale.x < 0) transform.localScale = Vector3.one / 100;

        // Adjust the scale of each child to keep their sizes consistent while still allowing their position values to be scaled
        foreach (Transform child in transform)
        {
            child.localScale = Vector3.one * (1/transform.localScale.x);
        }
    }
}
