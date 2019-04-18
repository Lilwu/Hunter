using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum NPC_Type
{
    Shop,
    Mission,
    Message,
}

public class NPC : MonoBehaviour
{
    public NPC_Type npc_Type;
    [Space]
    public string _npcName;
    public string _npcStartMessage;
    public string _npcFinishedMessage;
    public string _npcCancelMessage;
    public string _npcNonMoneyMessage;

    [Header("Message")]
    public Text _npcMessageNameText;
    public Text _npcMessageContentText;
    [Space]
    public string _npcMessageOp;
    public string _npcMessageMosText;
    public string _npcMessageItemText;
    public string _npcMessageEnd;
    [Space]
    public Item[] _npcGiveItem;
    public Monster[] _npcAppearMos;
    private string npcAppearMosTotal;
    private string npcGiveItemTotal;

    [Space]
    public Inventory inventory;
    public GameObject shopPanel;
    public Text NpcName;
    public Text NpcMessage;

    private Animator animator;
    private bool isRange;

    public GameObject questMark;
    public GameObject exclamationMark;

    private void Awake()
    {
        NpcName.text = _npcName;
        NpcName.color = Color.green;
        NpcMessage.text = _npcStartMessage;
        animator = GetComponent<Animator>();
        inventory = FindObjectOfType<Inventory>();

        if (_npcMessageContentText != null)
        {
            SetMessagePanel();
        }
    }

    private void Start()
    {
        NpcMessage.enabled = false;
        shopPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            NpcMessage.text = _npcStartMessage;
            NpcMessage.enabled = true;
            isRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            NpcMessage.text = _npcStartMessage;
            NpcMessage.enabled = false;
            isRange = false;
            shopPanel.SetActive(false);
            animator.SetBool("IsQuest", false);
        }
    }

    public void OpenShopPanel()
    {
        if(isRange)
        {
            NpcMessage.text = _npcStartMessage;
            FindObjectOfType<InventoryInput>().CliclSound();
            shopPanel.SetActive(true);
            animator.SetBool("IsQuest", true);
        }
    }

    public void ChangeMessage(string message)
    {
        NpcMessage.text = message;
    }

    public void SetMessagePanel()
    {
        //NPC名稱
        _npcMessageNameText.color = Color.green;
        _npcMessageNameText.text = _npcName + "：";

        //對話內容
        _npcMessageContentText.text = _npcMessageOp + "\n";

        //NPC提供怪物資訊
        if (_npcAppearMos != null && _npcMessageMosText != null)
        {
            for (int i = 0; i < _npcAppearMos.Length; i++)
            {
                npcAppearMosTotal += _npcAppearMos[i].m_name + "\n";
            }

            _npcMessageContentText.text += _npcMessageMosText + "\n" + "<color=red>" + npcAppearMosTotal + "</color>";
        }

        //NPC給予物品
        if(_npcGiveItem != null && _npcMessageItemText != null)
        {
            for (int i = 0; i < _npcGiveItem.Length; i++)
            {
                npcGiveItemTotal += _npcGiveItem[i].ItemName + "\n";
            }

            _npcMessageContentText.text += _npcMessageItemText + "\n" + "<color=yellow>" + npcGiveItemTotal + "</color>" + "\n";
        }

        //對話結束
        if(_npcMessageEnd != null)
        {
            _npcMessageContentText.text += _npcMessageEnd;
        }
    }

    public void GoToBattleArea()
    {
        if (_npcGiveItem != null)
        {
            for (int i = 0; i < _npcGiveItem.Length; i++)
            {
                inventory.AddItem(_npcGiveItem[i]);
            }
        }

        isRange = false;
        FindObjectOfType<GameLoader>().LoadScene(2);
        GameObject.Find("Character").transform.position = GameObject.Find("BornPoint").transform.position;
    }
}
