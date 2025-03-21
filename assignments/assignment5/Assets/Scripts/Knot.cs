using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Knot : MonoBehaviour
{
    public List<GameObject> controlPoints;
    public GameObject controlPointPrefab;
    public int knotID;

    private GameObject spline;

    // Start is called before the first frame update
    void Start()
    {
        spline = GameObject.FindGameObjectWithTag("Curve");
        spline.GetComponent<Spline>().knots.Add(gameObject);

        knotID = spline.GetComponent<Spline>().knots.Count - 1;

        GameObject newControlPoint;
        if (knotID == 0 || knotID == spline.GetComponent<Spline>().knots.Count - 1)
        {
            newControlPoint = GameObject.Instantiate(controlPointPrefab);
            controlPoints.Add(newControlPoint);
            newControlPoint.transform.SetParent(transform);

            if (knotID == spline.GetComponent<Spline>().knots.Count - 1) controlPoints[0].transform.localPosition = new Vector3(-1, 0, -1);
            if (knotID == 0) controlPoints[0].transform.localPosition = new Vector3(1, 0, 1);
        }
        else
        {
            newControlPoint = GameObject.Instantiate(controlPointPrefab);
            controlPoints.Add(newControlPoint);
            newControlPoint.transform.SetParent(transform);

            newControlPoint = GameObject.Instantiate(controlPointPrefab);
            controlPoints.Add(newControlPoint);
            newControlPoint.transform.SetParent(transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
