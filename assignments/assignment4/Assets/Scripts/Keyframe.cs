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
}
