using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionPanel : MonoBehaviour
{
    public Transform missionslotParent;
    public List<Mission> missions;
    public GameObject missionPanel;

    public MissionSlot[] missionSlots;

    private void Awake()
    {
        ResetMissionPanel();
        ResetMissionCondition();
        GetComponentInParent<Canvas>().worldCamera = GameObject.Find("UICamera").GetComponent<Camera>();
    }

    private void OnValidate()
    {
        missionSlots = missionslotParent.GetComponentsInChildren<MissionSlot>();
    }

    public void ResetMissionPanel()
    {
        for (int i = 0; i < missionSlots.Length; i++)
        {
            missionSlots[i].mission = missions[i];
        }
    }

    public void ResetMissionCondition()
    {
        for (int i = 0; i < missionSlots.Length; i++)
        {
            missionSlots[i].CheckMissionCondition();
        }
    }

    public void ResetOtherMission()
    {
        for (int i = 0; i < missionSlots.Length; i++)
        {
            missionSlots[i].CheckOtherMission();
        }
    }

    public void ClosePanel(GameObject warningPanel)
    {
        if(warningPanel.activeSelf == true)
        {
            warningPanel.SetActive(false);
        }
    }
}
