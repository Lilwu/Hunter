using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionItemType
{
    monsterItem
}

[CreateAssetMenu]
public class MissionItem : Item
{
    public string itemDescription;
    public MissionType missionType;


    public override Item GetCopy()
    {
        return Instantiate(this);
    }
}
