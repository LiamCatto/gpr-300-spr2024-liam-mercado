using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private float hypotenuseLength;
    private float height;
    private float side1Length;
    private float side2Length;

    // Start is called before the first frame update
    void Start()
    {
        hypotenuseLength = 1.155f;
        height = 1.0f;
        side1Length = 0.577f;
        side2Length = 1.0f;

        InitiateTriangle(new Vector3(0, height / 2, 0), "Upper Triangle");
        InitiateTriangle(new Vector3(0, -height / 2, 0), "Lower Triangle");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Find("Upper Triangle").rotation = transform.rotation;
        transform.Find("Lower Triangle").rotation = transform.rotation;
    }

    private void InitiateTriangle(Vector3 pos, string label)
    {
        GameObject triangle = new GameObject();
        triangle.gameObject.name = label;
        if (pos.y < 0) triangle.transform.rotation = Quaternion.Euler(0, 0, 180);
        triangle.transform.SetParent(transform);
        triangle.transform.localPosition = pos;

        triangle.AddComponent(typeof(MeshRenderer));
        triangle.AddComponent(typeof(MeshFilter));

        triangle.GetComponent<MeshFilter>().mesh = CreateTriangle();
        triangle.GetComponent<MeshRenderer>().material = transform.Find("Quad").GetComponent<MeshRenderer>().material;
    }

    private Mesh CreateTriangle()
    {
        Mesh triangleMesh = new Mesh();

        triangleMesh.vertices = new Vector3[]
        {
            new Vector3(-side1Length, 0, -side2Length / 2),
            new Vector3( side1Length, 0, -side2Length / 2),
            new Vector3(              0, 0, side2Length / 2)
        };

        triangleMesh.triangles = new int[]
        {
            0, 2, 1
        };

        triangleMesh.normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up };

        return triangleMesh;
    }
}
