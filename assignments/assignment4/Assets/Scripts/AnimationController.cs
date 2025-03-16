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
        newKeyframe.GetComponent<Keyframe>().uiCollapsed = false;
        newKeyframe.transform.SetParent(settingsPanel.transform);
        newKeyframe.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Keyframe " + newKeyframe.GetComponent<Keyframe>().keyframeID;
        newKeyframe.gameObject.name = "Keyframe Header " + newKeyframe.GetComponent<Keyframe>().keyframeID;

        Vector3 pos = settingsPanel.transform.Find("Initial Keyframe Header").transform.GetComponent<RectTransform>().localPosition;
        Vector3 scale = settingsPanel.transform.Find("Initial Keyframe Header").transform.GetComponent<RectTransform>().localScale;
        int numCollapsed = CountCollapsedHeaders();   // Number of keyframe headers that are currently collapsed in the UI
        int numUnCollapsed = newKeyframe.GetComponent<Keyframe>().keyframeID - numCollapsed;

        newKeyframe.transform.GetComponent<RectTransform>().localPosition = pos - new Vector3(0, (376 * numUnCollapsed) + (73.5f * numCollapsed) + (4f * newKeyframe.GetComponent<Keyframe>().keyframeID), 0);
        newKeyframe.transform.GetComponent<RectTransform>().localScale = scale;

        clip.KeyframeList.Add(newKeyframe);

        settingsPanel.transform.Find("Background").transform.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 378);
        settingsPanel.transform.Find("Background").transform.GetComponent<RectTransform>().localPosition -= new Vector3(0, 189, 0);
    }
    public void RemoveKeyframe(GameObject keyframe)
    {
        GameObject settingsPanel = keyframe.transform.parent.gameObject;
        int totalHeaders = clip.KeyframeList.Count + 2;     // +1 since the list is indexed at 0 then -1 because this header is being removed then +2 more for the two initial headers
        int numCollapsed = CountCollapsedHeaders();
        if (keyframe.GetComponent<Keyframe>().uiCollapsed) numCollapsed--;
        int numUnCollapsed = totalHeaders - numCollapsed;
        float uiHeight = (376 * numUnCollapsed) + (73.5f * numCollapsed) + (4f * totalHeaders);
        float uiWidth = settingsPanel.transform.Find("Background").transform.GetComponent<RectTransform>().sizeDelta.x;
        Vector3 uiPosition = settingsPanel.transform.Find("Background").transform.GetComponent<RectTransform>().localPosition;
        float oldUIYPos = -120;
        float oldUIHeight = 770;

        settingsPanel.transform.Find("Background").transform.GetComponent<RectTransform>().sizeDelta = new Vector2(uiWidth, uiHeight);
        settingsPanel.transform.Find("Background").transform.GetComponent<RectTransform>().localPosition = new Vector3(uiPosition.x, oldUIYPos - ((uiHeight - oldUIHeight) / 2), uiPosition.z);

        if (keyframe.GetComponent<Keyframe>().uiCollapsed) keyframe.GetComponent<CollapsingUI>().MoveHeaders(-1, 76);
        else keyframe.GetComponent<CollapsingUI>().MoveHeaders(-1, 378.5f);

        clip.KeyframeList.Remove(keyframe);
        Destroy(keyframe);

        // still trying to get the background to be positioned correctly
        // how it ends up adding and removing 1 keyframe with 1 collapsed header: 1220      how it should be: 467.5
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