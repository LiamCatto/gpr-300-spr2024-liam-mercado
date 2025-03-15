using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class AnimationController : MonoBehaviour
{
    [HideInInspector] public AnimationClip clip;

    public GameObject settingsPanel;
    public GameObject keyframeUIPrefab;
    public GameObject targetObject;
    public bool isLooping;
    public float playbackSpeed;
    public float playbackTime;
    public bool isPlaying;

    // Start is called before the first frame update
    void Start()
    {
        clip = transform.GetComponent<AnimationClip>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddKeyframe()
    {
        GameObject newKeyframe = GameObject.Instantiate(keyframeUIPrefab);

        newKeyframe.GetComponent<Keyframe>().keyframeID = clip.KeyframeList.Count + 1;
        newKeyframe.transform.SetParent(settingsPanel.transform);
        newKeyframe.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Keyframe " + newKeyframe.GetComponent<Keyframe>().keyframeID;
        newKeyframe.gameObject.name = "Keyframe Header " + newKeyframe.GetComponent<Keyframe>().keyframeID;

        Vector3 pos = settingsPanel.transform.Find("Initial Keyframe Header").transform.GetComponent<RectTransform>().localPosition;
        Vector3 scale = settingsPanel.transform.Find("Initial Keyframe Header").transform.GetComponent<RectTransform>().localScale;
        newKeyframe.transform.GetComponent<RectTransform>().localPosition = pos - new Vector3(0, 380 + (318.5f * clip.KeyframeList.Count), 0);
        newKeyframe.transform.GetComponent<RectTransform>().localScale = scale;

        clip.KeyframeList.Add(newKeyframe);

        settingsPanel.transform.Find("Background").transform.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 318.5f);
        settingsPanel.transform.Find("Background").transform.GetComponent<RectTransform>().localPosition -= new Vector3(0, 159.25f, 0);
    }
}
