using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Keyframe : MonoBehaviour
{
    [HideInInspector] public bool uiCollapsed;

    public Vector3 positionKey;
    public Vector3 rotationKey;
    public Vector3 scaleKey;
    public float time;
    public int keyframeID;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveKeyframe(GameObject keyframe)
    {
        GameObject.FindGameObjectWithTag("Animator").GetComponent<AnimationController>().RemoveKeyframe(keyframe);
    }

    public void Interpolate(GameObject nextKeyframe, float t)
    {
        Keyframe next = nextKeyframe.gameObject.GetComponent<Keyframe>();

        Vector3.Lerp(positionKey, next.positionKey, t);
        Vector3.Lerp(rotationKey, next.rotationKey, t);
        Vector3.Lerp(scaleKey, next.scaleKey, t);
    }
}
