using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CollapsingUI : MonoBehaviour
{
    GameObject animator;
    int keyframeCount;

    void Start()
    {
        //animator = GameObject.FindGameObjectWithTag("Animator");
        //keyframeCount = animator.GetComponent<AnimationController>().clip.KeyframeList.Count + 1;
    }

    public void CollapseHeader()
    {
        bool isCollapsed = false;

        // Collapsed Background is only active when the header is collapsed, as the name implies.
        if (transform.Find("Collapsed Background").gameObject.activeSelf) isCollapsed = true;

        // If not collapsed, deactivate all children except for Title and activate Collapsed Background. Otherwise, do the opposite.
        foreach (Transform child in transform)
        {
            if (child.gameObject.name != "Title")
            {
                if (!isCollapsed)
                {
                    child.gameObject.SetActive(false);
                    if (child.gameObject.name == "Collapsed Background") child.gameObject.SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(true);
                    if (child.gameObject.name == "Collapsed Background") child.gameObject.SetActive(false);
                }
            }
        }

        if (!isCollapsed) MoveHeaders(1);
        else MoveHeaders(-1);
    }

    // sign should only be -1 or +1
    private void MoveHeaders(float sign)
    {
        RectTransform rt;
        int yFactor = 0;    // After the foreach statement reaches this header, start moving all other headers up
        int clickedID = 0;  // The keyframeID of the selected header. If the header does not have an ID, an ID of 0 is used.

        if (sign >= 0) sign = 1;
        else sign = -1;

        foreach (Transform child in transform.parent)
        {
            if (child.gameObject.name.Contains("Header"))
            {
                rt = child.transform.GetComponent<RectTransform>();
                if (gameObject.GetComponent<Keyframe>() != null) clickedID = gameObject.GetComponent<Keyframe>().keyframeID;

                if (child.gameObject.name == gameObject.name) yFactor = 0;


                // Currently having three keyframes is not supported by the UI system


                if (clickedID <= 0) rt.localPosition += new Vector3(0, sign * 305 * yFactor, 0);
                else rt.localPosition += new Vector3(0, sign * 243 * yFactor, 0);

                if (child.gameObject.name == gameObject.name) yFactor = 1;
            }
        }

        if (gameObject.GetComponent<Keyframe>() != null)
        {
            if (gameObject.GetComponent<Keyframe>().keyframeID >= 1)
            {
                transform.parent.Find("Background").transform.GetComponent<RectTransform>().sizeDelta -= new Vector2(0, sign * 245.5f);
                transform.parent.Find("Background").transform.GetComponent<RectTransform>().localPosition += new Vector3(0, sign * 122.75f, 0);
            }
        }
        else
        {
            transform.parent.Find("Background").transform.GetComponent<RectTransform>().sizeDelta -= new Vector2(0, sign * 302.5f);
            transform.parent.Find("Background").transform.GetComponent<RectTransform>().localPosition += new Vector3(0, sign * 151.25f, 0);
        }
    }
}

// DEFINITELY didn't spend 8.5 hours just on UI...
