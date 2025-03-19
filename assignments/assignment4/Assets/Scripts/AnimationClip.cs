using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimationClip : MonoBehaviour
{
    public float duration;
    public List<GameObject> KeyframeList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Vector3[] Interpolate(GameObject currKeyframe, GameObject nextKeyframe, float t)
    {
        Keyframe next = nextKeyframe.gameObject.GetComponent<Keyframe>();

        Vector3[] result = new Vector3[3];
        Vector3 pos, rot, scale = Vector3.zero;

        if (currKeyframe.name == "Initial Keyframe Header")
        {
            Transform setting2 = currKeyframe.transform.Find("Initial Keyframe Setting 2 (TMPro)");
            pos = new Vector3 
                (
                    float.Parse(setting2.Find("InputField (TMP)").gameObject.GetComponent<TMP_InputField>().text),
                    float.Parse(setting2.Find("InputField (TMP) (1)").gameObject.GetComponent<TMP_InputField>().text),
                    float.Parse(setting2.Find("InputField (TMP) (2)").gameObject.GetComponent<TMP_InputField>().text)
                );

            Transform setting3 = currKeyframe.transform.Find("Initial Keyframe Setting 3 (TMPro)");
            rot = new Vector3
                (
                    float.Parse(setting3.Find("InputField (TMP)").gameObject.GetComponent<TMP_InputField>().text),
                    float.Parse(setting3.Find("InputField (TMP) (1)").gameObject.GetComponent<TMP_InputField>().text),
                    float.Parse(setting3.Find("InputField (TMP) (2)").gameObject.GetComponent<TMP_InputField>().text)
                );

            Transform setting4 = currKeyframe.transform.Find("Initial Keyframe Setting 4 (TMPro)");
            scale = new Vector3
                (
                    float.Parse(setting4.Find("InputField (TMP)").gameObject.GetComponent<TMP_InputField>().text),
                    float.Parse(setting4.Find("InputField (TMP) (1)").gameObject.GetComponent<TMP_InputField>().text),
                    float.Parse(setting4.Find("InputField (TMP) (2)").gameObject.GetComponent<TMP_InputField>().text)
                );
        }
        else
        {
            pos = currKeyframe.GetComponent<Keyframe>().positionKey;
            rot = currKeyframe.GetComponent<Keyframe>().rotationKey;
            scale = currKeyframe.GetComponent<Keyframe>().scaleKey;
        }

        result[0] = Vector3.Lerp(pos, next.positionKey, t);
        result[1] = Vector3.Lerp(rot, next.rotationKey, t);
        result[2] = Vector3.Lerp(scale, next.scaleKey, t);

        return result;
    }
}
