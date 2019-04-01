using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public string _npcName;
    public string _npcStartMessage;
    public string _npcFinishedMessage;
    public string _npcCancelMessage;
    public string _npcNonMoneyMessage;

    public GameObject shopPanel;
    public Text NpcName;
    public Text NpcMessage;

    private Animator animator;
    private bool isRange;


    private void Awake()
    {
        NpcName.text = _npcName;
        NpcName.color = Color.green;
        NpcMessage.text = _npcStartMessage;
        animator = GetComponent<Animator>();
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
            animator.SetBool("isShop", false);
        }
    }

    public void OpenShopPanel()
    {
        if(isRange)
        {
            NpcMessage.text = _npcStartMessage;
            FindObjectOfType<InventoryInput>().CliclSound();
            shopPanel.SetActive(true);
            animator.SetBool("isShop", true);
        }
    }

    public void ChangeMessage(string message)
    {
        NpcMessage.text = message;
    }
}
