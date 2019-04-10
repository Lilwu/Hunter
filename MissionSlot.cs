using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionSlot : MonoBehaviour
{
    public Mission mission;
    public MissionManager missionManager;
    public Inventory inventory;
    public GameObject warningPanel;
    public ParticleSystem awardEffect;
    private Text warningText;
    private Player _player;
    private Color normalColor = Color.white;
    private Color nonenoughColor = Color.red;
    private InventoryInput inventoryInput;
    private GameObject questMarkOpen;
    private GameObject exclamationMarkOpen;

    [SerializeField] Text _missionName;
    [SerializeField] Text _missionCondition;
    [SerializeField] Text _missionAwards;
    [SerializeField] Image _missionButton;

    [Space]
    [SerializeField] Sprite btn_Accept;
    [SerializeField] Sprite btn_Finished;
    [SerializeField] Sprite btn_Inprogress;
    [SerializeField] Sprite btn_Insufficient;
    [SerializeField] Sprite btn_Receive;

    //任務事件 20190403
    private void IsFinishedMission()
    {
        print("test04");
        CheckMissionCondition();
    }

    private void Awake()
    {
        warningText = warningPanel.GetComponentInChildren<Text>();
    }

    private void Start()
    {
        inventoryInput = FindObjectOfType<InventoryInput>();
        _missionName.text = "任務：" + mission.missionName;
        _missionCondition.text = "等級限制：" + mission.missionLevel;
        _missionAwards.text = "任務獎勵：" + mission.missionAwards;
        mission.IsAccept();
    }

    public void CheckMissionCondition()
    {
        _player = GameObject.Find("Character").GetComponent<Player>();
        questMarkOpen = GetComponentInParent<NPC>().questMark;
        exclamationMarkOpen = GetComponentInParent<NPC>().exclamationMark;

        if (_player.LV < mission.missionLevel)
        {
            _missionCondition.color = nonenoughColor;
            _missionButton.sprite = btn_Insufficient;
            _missionButton.GetComponent<Button>().enabled = false;
        }
        else if(_player.LV >= mission.missionLevel && mission.ISFINISHED == false && missionManager.ISCOLLECTIONMISSION == false && missionManager.ISDESTROYMISSION == false)
        {
            _missionCondition.color = normalColor;
            _missionButton.sprite = btn_Accept;
            _missionButton.GetComponent<Button>().enabled = true;
            questMarkOpen.SetActive(true);
        }
        else if (_player.LV >= mission.missionLevel && mission.ISFINISHED == true && _missionButton.sprite != btn_Finished)
        {
            _missionButton.sprite = btn_Receive;
            exclamationMarkOpen.SetActive(true);
        }
    }

    public void ReceiveAwards()
    {
        if (_missionButton.sprite == btn_Receive)
        {
            if(mission._missionAwardMoney != 0)
            {
                _player.AddMoney(mission._missionAwardMoney);
                print("您得到了" + mission._missionAwardMoney + "個金幣");
            }

            if(mission._missionAwardsList != null)
            {
                for (int i = 0; i < mission.missionContentAmount; i++)
                {
                    inventory.RemoveItem(mission.missionItemName);
                }

                for (int i = 0; i < mission._missionAwardsList.Count; i++)
                {
                    inventory.AddItem(mission._missionAwardsList[i]);
                    print("您得到了" + mission._missionAwardsList[i]);
                }
            }
            exclamationMarkOpen.SetActive(false);
            _missionButton.sprite = btn_Finished;
            _missionButton.GetComponent<Button>().enabled = false;
            missionManager.ResetMissionMsgPanel();
            awardEffect.Play();
            inventoryInput.GetComponent<AudioSource>().PlayOneShot(inventoryInput.awardClip);

            GetComponentInParent<MissionPanel>().ResetOtherMission();
        }
        else if(_missionButton.sprite == btn_Accept && missionManager.ISCOLLECTIONMISSION == false && missionManager.ISDESTROYMISSION == false)
        {
            print("您接受了任務：" + mission.missionName);
            missionManager.AcceptMission(mission);

            _missionButton.sprite = btn_Inprogress;
            questMarkOpen.SetActive(false);
        }

        else if(missionManager.ISCOLLECTIONMISSION == true || missionManager.ISDESTROYMISSION == true)
        {
            print("目前正在執行其他任務！");
            warningText.text = "- " + "目前正在執行其他任務" + " -";
            warningPanel.SetActive(true);
        }

        else if (_missionButton.sprite == btn_Finished)
        {
            print("您已完成此任務了！");
        }
    }

    public void CheckOtherMission()
    {
        if (_missionButton.sprite == btn_Accept && missionManager.ISCOLLECTIONMISSION == false && missionManager.ISDESTROYMISSION == false)
        {
            questMarkOpen.SetActive(true);
        }
    }
}
