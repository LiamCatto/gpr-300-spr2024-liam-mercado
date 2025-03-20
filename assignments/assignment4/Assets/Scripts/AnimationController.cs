using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
//using UnityEngine.UI;

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
    
    private int count;
    private bool isAnimRunning;
    private float timeStep;      // Number of time steps in a second
    private float timeStepInterval = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        clip = transform.GetComponent<AnimationClip>();
        count = 0;
        isAnimRunning = false;
        isPlaying = false;
}

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateStats();

        Transform playAnimationButton = settingsPanel.transform.Find("Animation Header").Find("Play Animation").Find("Button");
        if (isPlaying) playAnimationButton.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text = "Playing";
        else playAnimationButton.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text = "Play Animation";

        if (isPlaying && clip.KeyframeList.Count > 0 && playbackTime < clip.duration)
        {
            if (!isAnimRunning) StartCoroutine(Animate());
        }
    }

    private void UpdateStats()
    {
        GameObject animHeader = settingsPanel.transform.Find("Animation Header").gameObject;

        clip.duration = float.Parse(animHeader.transform.Find("Animation Setting 1 (TMPro)").Find("InputField (TMP)").gameObject.GetComponent<TMP_InputField>().text);
        playbackSpeed = float.Parse(animHeader.transform.Find("Animation Setting 2 (TMPro)").Find("InputField (TMP)").gameObject.GetComponent<TMP_InputField>().text);
        playbackTime = float.Parse(animHeader.transform.Find("Animation Setting 3 (TMPro)").Find("InputField (TMP)").gameObject.GetComponent<TMP_InputField>().text);
        isLooping = animHeader.transform.Find("Animation Setting 4 (TMPro)").Find("Toggle").gameObject.GetComponent<UnityEngine.UI.Toggle>().isOn;

        if (!isPlaying)
        {
            playbackTime = 0;
            count = 0;
        }

        // Update keyframe stats

        foreach (GameObject keyframe in clip.KeyframeList)
        {
            Keyframe keyframeScript = keyframe.GetComponent<Keyframe>();
            Transform keyHeader = settingsPanel.transform.Find("Keyframe Header " + keyframeScript.keyframeID);

            keyframeScript.time = float.Parse(keyHeader.Find("Keyframe Setting 1 (TMPro)").Find("InputField (TMP)").gameObject.GetComponent<TMP_InputField>().text);

            Transform setting2 = keyHeader.Find("Keyframe Setting 2 (TMPro)");
            keyframeScript.positionKey = new Vector3
                (
                    float.Parse(setting2.Find("InputField (TMP)").gameObject.GetComponent<TMP_InputField>().text),
                    float.Parse(setting2.Find("InputField (TMP) (1)").gameObject.GetComponent<TMP_InputField>().text),
                    float.Parse(setting2.Find("InputField (TMP) (2)").gameObject.GetComponent<TMP_InputField>().text)
                );

            Transform setting3 = keyHeader.Find("Keyframe Setting 3 (TMPro)");
            keyframeScript.rotationKey = new Vector3
                (
                    float.Parse(setting3.Find("InputField (TMP)").gameObject.GetComponent<TMP_InputField>().text),
                    float.Parse(setting3.Find("InputField (TMP) (1)").gameObject.GetComponent<TMP_InputField>().text),
                    float.Parse(setting3.Find("InputField (TMP) (2)").gameObject.GetComponent<TMP_InputField>().text)
                );

            Transform setting4 = keyHeader.Find("Keyframe Setting 4 (TMPro)");
            keyframeScript.scaleKey = new Vector3
                (
                    float.Parse(setting4.Find("InputField (TMP)").gameObject.GetComponent<TMP_InputField>().text),
                    float.Parse(setting4.Find("InputField (TMP) (1)").gameObject.GetComponent<TMP_InputField>().text),
                    float.Parse(setting4.Find("InputField (TMP) (2)").gameObject.GetComponent<TMP_InputField>().text)
                );
        }
    }

    IEnumerator Animate()
    {
        isAnimRunning = true;

        Vector3[] frameData = new Vector3[3];
        GameObject nextKeyframe = clip.KeyframeList[count];

        if (count == 0)
        {
            frameData = clip.Interpolate(settingsPanel.transform.Find("Initial Keyframe Header").gameObject, nextKeyframe, timeStep);
        }
        else
        {
            frameData = clip.Interpolate(clip.KeyframeList[count - 1], nextKeyframe, timeStep);
        }
        
        targetObject.transform.position = frameData[0];
        targetObject.transform.rotation = Quaternion.Euler(frameData[1]);
        targetObject.transform.localScale = frameData[2];

        timeStep += timeStepInterval;
        playbackTime += timeStep;

        yield return new WaitForSeconds(timeStepInterval / playbackSpeed);

        if (timeStep >= nextKeyframe.GetComponent<Keyframe>().time)
        {
            count++;
            timeStep = 0;
        }

        if (count >= clip.KeyframeList.Count) isPlaying = false;

        isAnimRunning = false;
    }

    public void StartAnimation()
    {
        if (!isPlaying) isPlaying = true;
    }

    public void AddKeyframe()
    {
        GameObject newKeyframe = GameObject.Instantiate(keyframeUIPrefab);

        newKeyframe.GetComponent<Keyframe>().keyframeID = clip.KeyframeList.Count + 1;
        newKeyframe.GetComponent<Keyframe>().uiCollapsed = false;
        newKeyframe.transform.SetParent(settingsPanel.transform);
        newKeyframe.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Keyframe " + newKeyframe.GetComponent<Keyframe>().keyframeID;
        newKeyframe.gameObject.name = "Keyframe Header " + newKeyframe.GetComponent<Keyframe>().keyframeID;
        int totalKeyframes = newKeyframe.GetComponent<Keyframe>().keyframeID;

        Vector3 pos = new Vector3(0, -215, 0);
        Vector3 scale = settingsPanel.transform.Find("Initial Keyframe Header").transform.GetComponent<RectTransform>().localScale;
        int numCollapsed = CountCollapsedHeaders();   // Number of keyframe headers that are currently collapsed in the UI
        int numUnCollapsed = totalKeyframes - numCollapsed;

        newKeyframe.transform.GetComponent<RectTransform>().localPosition = pos - new Vector3(0, (376 * numUnCollapsed) + (73.5f * numCollapsed) + (4f * totalKeyframes), 0);
        newKeyframe.transform.GetComponent<RectTransform>().localScale = scale;

        clip.KeyframeList.Add(newKeyframe);

        settingsPanel.transform.Find("Background").transform.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 380);
        settingsPanel.transform.Find("Background").transform.GetComponent<RectTransform>().localPosition -= new Vector3(0, 190, 0);
    }
    public void RemoveKeyframe(GameObject keyframe)
    {
        GameObject settingsPanel = keyframe.transform.parent.gameObject;
        int totalHeaders = clip.KeyframeList.Count + 2;     // Wrong but seems to work (no plus one) --->  +1 since the list is indexed at 0 then -1 because this header is being removed then +2 more for the two initial headers
        int numCollapsed = CountCollapsedHeaders();
        if (keyframe.GetComponent<Keyframe>().uiCollapsed) numCollapsed--;
        int numUnCollapsed = totalHeaders - numCollapsed;
        float uiHeight = (376 * numUnCollapsed) + (73.5f * numCollapsed) + (4f * totalHeaders);
        float uiWidth = settingsPanel.transform.Find("Background").transform.GetComponent<RectTransform>().sizeDelta.x;
        Vector3 uiPosition = settingsPanel.transform.Find("Background").transform.GetComponent<RectTransform>().localPosition;

        keyframe.GetComponent<CollapsingUI>().MoveHeaders(1, 380);

        clip.KeyframeList.Remove(keyframe);
        Destroy(keyframe);

        int counter = 1;
        foreach (GameObject kf in clip.KeyframeList)
        {
            kf.GetComponent<Keyframe>().keyframeID = counter;
            kf.gameObject.name = "Keyframe " + counter + " Header";
            kf.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Keyframe " + counter;
            counter++;
        }
    }
    public int CountCollapsedHeaders()
    {
        int num = 0;

        foreach (GameObject keyframe in clip.KeyframeList)
        {
            if (keyframe.GetComponent<Keyframe>().uiCollapsed) num++;
        }

        if (settingsPanel.transform.Find("Animation Header").transform.Find("Collapsed Background").gameObject.activeSelf) num++;
        if (settingsPanel.transform.Find("Initial Keyframe Header").transform.Find("Collapsed Background").gameObject.activeSelf) num++;

        return num;
    }
}

// Note: 93 is the y-offset between the positions of a header and the top border of its background.
// Header dimensions:
//      Height: 376
//      Collapsed Height: 73.5
//      Space Between: 4    OR    Upper/Lower padding: 2