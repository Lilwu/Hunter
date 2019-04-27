using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionItemType
{
    任務道具,
}

[CreateAssetMenu]
public class MissionItem : Item
{
    public string itemDescription;
    public MissionItemType missionType;

    public override Item GetCopy()
    {
        return Instantiate(this);
    }
}
