using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public GameObject missionPanel;
    public GameObject missionMsgPanel;
    public ParticleSystem awardEffect;
    [SerializeField] GameObject inprogressText;
    [SerializeField] GameObject finishedText;
    [SerializeField] Text _missionName;
    [SerializeField] Text _destroyText;
    [SerializeField] Text _collectionText;

    private Mission mission;
    private int missionAmount;
    private InventoryInput inventoryInput;

    //任務事件 20190403
    protected bool isDestroyMission;
    protected bool isCollectionMission;
    protected string missionMosName;
    protected string missionItemName;

    private void DestroyMissionMos()
    {
        if (missionAmount < mission.missionContentAmount)
        {
            missionAmount++;
            _destroyText.text = "消滅" + missionMosName + "(" + missionAmount + "/" + mission.missionContentAmount + ")";

            CheckDestroyMosAmount();
        }
    }

    private void PickupMissionitem()
    {
        if(missionAmount < mission.missionContentAmount)
        {
            missionAmount++;
            _collectionText.text = "蒐集" + missionItemName + "(" + missionAmount + "/" + mission.missionContentAmount + ")";

            CheckPickupItemAmount();
        }
    }

    private void CheckDestroyMosAmount()
    {
        if (missionAmount >= mission.missionContentAmount)
        {
            inprogressText.gameObject.SetActive(false);
            finishedText.gameObject.SetActive(true);
            mission.IsFinished();
            awardEffect.Play();
            inventoryInput.GetComponent<AudioSource>().PlayOneShot(inventoryInput.awardClip);
        }
    }

    private void CheckPickupItemAmount()
    {
        if (missionAmount >= mission.missionContentAmount)
        {
            inprogressText.gameObject.SetActive(false);
            finishedText.gameObject.SetActive(true);
            mission.IsFinished();
            awardEffect.Play();
            inventoryInput.GetComponent<AudioSource>().PlayOneShot(inventoryInput.awardClip);
        }
    }

    private void OnEnable()
    {
        MonsterHealth.MissionActionEvent += DestroyMissionMos;
        PlayerController.MissionItemActionEvent += PickupMissionitem;
    }
    private void OnDisable()
    {
        MonsterHealth.MissionActionEvent -= DestroyMissionMos;
        PlayerController.MissionItemActionEvent -= PickupMissionitem;
    }
    //任務事件 20190403

    private void Start()
    {
        missionAmount = 0;
        isDestroyMission = false;
        isCollectionMission = false;
        inventoryInput = FindObjectOfType<InventoryInput>();
        missionMsgPanel.SetActive(false);
    }

    public void AcceptMission(Mission addMission)
    {
        if(mission == null)
        {
            inprogressText.SetActive(true);
            mission = addMission;
            _missionName.text = mission.missionName;

            if (mission.missionType == MissionType.Destroy)
            {
                _destroyText.gameObject.SetActive(true);
                missionMosName = mission.missionMonsterName;
                _destroyText.text = "消滅" + missionMosName + "(" + missionAmount + "/" + mission.missionContentAmount + ")";
                isDestroyMission = true;
            }
            else if (mission.missionType == MissionType.Collection)
            {
                _collectionText.gameObject.SetActive(true);
                _collectionText.text = "蒐集" + mission.missionItemName.ItemName + "(" + missionAmount + "/" + mission.missionContentAmount + ")";
                isCollectionMission = true;
                missionItemName = mission.missionItemName.ItemName;
            }

            missionMsgPanel.SetActive(true);
        }
        else
        {
            print("目前正在進行其他任務！");
            return;
        }
    }

    public void ResetMissionMsgPanel()
    {
        isDestroyMission = false;
        isCollectionMission = false;
        finishedText.SetActive(false);
        mission = null;
        missionAmount = 0;
        _destroyText.text = "";
        _collectionText.text = "";
        _missionName.text = "任務名稱：";
        gameObject.SetActive(false);
    }

    public bool ISDESTROYMISSION
    {
        get { return isDestroyMission; }
    }
    public bool ISCOLLECTIONMISSION
    {
        get { return isCollectionMission; }
    }
    public string MISSIONMOSNAME
    {
        get { return missionMosName; }
    }
    public string MISSIONITEMNAME
    {
        get { return missionItemName; }
    }

    public Mission ISMISSION
    {
        get { return mission; }
    }
}