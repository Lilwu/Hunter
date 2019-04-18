using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMagic : MonoBehaviour
{
    public Image playerMagic;
    public Text magicText;
    private float maxMagic;
    private int currentMagic;
    private AudioSource _audiosource;
    private Animator _animator;

    private void Awake()
    {
        _audiosource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        maxMagic = GetComponent<Player>().MaxMP;
        currentMagic = GetComponent<Player>().MP;
        magicText.text = currentMagic + "/" + maxMagic;

        print("最大魔力為：" + maxMagic);
        print("目前魔力為：" + currentMagic);
    }

    public void AddMp(int amount)
    {
        if (currentMagic >= maxMagic)
        {
            FindObjectOfType<StatePanel>().SetSateText("沒發生任何事"); //顯示StatePanel

            return;
        }
        else if (currentMagic >= maxMagic - amount && currentMagic <= maxMagic)
        {
            currentMagic = (int)maxMagic;
            playerMagic.fillAmount = currentMagic / maxMagic;
            magicText.text = currentMagic + "/" + maxMagic;

            FindObjectOfType<StatePanel>().SetSateText("您的魔力已經恢復滿了"); //顯示StatePanel
        }
        else
        {
            currentMagic += amount;
            playerMagic.fillAmount = currentMagic / maxMagic;
            magicText.text = currentMagic + "/" + maxMagic;

            FindObjectOfType<StatePanel>().SetSateText("您增加了" + amount + "滴魔力"); //顯示StatePanel
        }
    }

    public void UseMagic(int consume)
    {
        currentMagic -= consume;
        playerMagic.fillAmount = currentMagic / maxMagic;
        magicText.text = currentMagic + "/" + maxMagic;

        FindObjectOfType<StatePanel>().SetSateText("您消耗了" + consume + "滴魔力"); //顯示StatePanel

        if (currentMagic <= 0)
        {
            FindObjectOfType<StatePanel>().SetSateText("您已經沒有魔力了"); //顯示StatePanel
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentMagic -= 50;
            playerMagic.fillAmount = currentMagic / maxMagic;
            magicText.text = currentMagic + "/" + maxMagic;
        }
    }
}
