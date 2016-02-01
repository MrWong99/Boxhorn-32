using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This class is used to find the maximum font size that can be displayed in each
/// textfield for predefined groups of text.
/// To use it you have to attach the script to a text GUI object and set the same
/// tag for each text that should be seen as being in the same group.
/// </summary>
public class FontSizeMatcher : MonoBehaviour
{
    public void Update()
    {
        GUIText Text = GetComponent<GUIText>();
        if (Text != null)
        {
            Text.fontSize = GetMaxSize(gameObject.tag);
        }
    }

    private static int GetMaxSize(string tag)
    {
        int res = -1;
        GameObject[] Objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject Obj in Objects)
        {
            GUIText Text = Obj.GetComponent<GUIText>();
            if (Text != null)
            {
                if (res == -1)
                {
                    res = Text.fontSize;
                } else if (res > Text.fontSize)
                {
                    res = Text.fontSize;
                }
            }
        }
        return res == -1 ? 0 : res;
    }
}
