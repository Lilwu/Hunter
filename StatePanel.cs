using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatePanel : MonoBehaviour
{
    public Transform StateTextParent;
    public Text[] stateTexts;
    public Text stateTextClone;

    public void SetSateText(string newText)
    {
        stateTexts = StateTextParent.GetComponentsInChildren<Text>();

        if (stateTexts.Length < 5)
        {
            stateTextClone.text = newText;
            Instantiate(stateTextClone.transform, StateTextParent.transform);
            stateTexts = StateTextParent.GetComponentsInChildren<Text>();
        }

        for (int i = 0; i < stateTexts.Length; i++)
        {
            if (stateTexts.Length == 1 || stateTexts.Length == 2 || stateTexts.Length == 3 || stateTexts.Length == 4 || stateTexts.Length == 5)
            {
                Destroy(stateTexts[i].gameObject , 1.5f);
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            SetSateText("測試中01");
        }
    }
}


