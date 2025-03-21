using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Knot : MonoBehaviour
{
    //public List<GameObject> controlPoints;
    public GameObject forwardControlPoint;      // Control point for the curve ahead of this knot
    public GameObject backwardControlPoint;     // Control point for the curve behind this knot
    public GameObject controlPointPrefab;
    public int knotID;
    public bool wasInitiated;

    private GameObject spline;

    void Start()
    {
        wasInitiated = false;
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
            newControlPoint = GameObject.Instantiate(controlPointPrefab);
            newControlPoint.transform.SetParent(transform);
            //controlPoints.Add(newControlPoint);

            if (knotID == knotCount - 1) //controlPoints[0].transform.localPosition = new Vector3(-1, 0, -1);
            {
                newControlPoint.transform.localPosition = new Vector3(-1, 0, -1);
                backwardControlPoint = newControlPoint;
            }
            if (knotID == 0) //controlPoints[0].transform.localPosition = new Vector3(1, 0, 1);
            {
                newControlPoint.transform.localPosition = new Vector3(1, 0, 1);
                forwardControlPoint = newControlPoint;
            }
        }
        else
        {
            newControlPoint = GameObject.Instantiate(controlPointPrefab);
            newControlPoint.transform.SetParent(transform);
            //controlPoints.Add(newControlPoint);
            forwardControlPoint = newControlPoint;

            newControlPoint = GameObject.Instantiate(controlPointPrefab);
            newControlPoint.transform.SetParent(transform);
            //controlPoints.Add(newControlPoint);
            backwardControlPoint = newControlPoint;
        }
    }
}
