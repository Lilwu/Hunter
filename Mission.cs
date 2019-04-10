using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType
{
    Collection,
    Destroy,
}

[CreateAssetMenu]
public class Mission : ScriptableObject
{
    public string missionName;
    public MissionType missionType;
    private bool isFinished;

    [Header("任務內容")]
    [Space]
    public string missionContent; //說明
    public string missionMonsterName;
    public Item missionItemName;
    public int missionContentAmount;

    [Header("任務限制")]
    [Space]
    public int missionLevel; //等級

    [Header("任務獎勵")]
    [Space]
    public string missionAwards; //獎勵說明
    public List<Item> _missionAwardsList;
    public int _missionAwardMoney;


    public void IsAccept()
    {
        isFinished = false;
    }

    public void IsFinished()
    {
        isFinished = true;
    }

    public bool ISFINISHED
    {
        get { return isFinished; }
    }
    
}
