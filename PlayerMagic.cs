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
            print("沒發生任何事");
            return;
        }
        else if (currentMagic >= maxMagic - amount && currentMagic <= maxMagic)
        {
            currentMagic = (int)maxMagic;
            playerMagic.fillAmount = currentMagic / maxMagic;
            magicText.text = currentMagic + "/" + maxMagic;

            print("你的魔力已經恢復滿了");
        }
        else
        {
            currentMagic += amount;
            playerMagic.fillAmount = currentMagic / maxMagic;
            magicText.text = currentMagic + "/" + maxMagic;

            print("你增加了" + amount + "滴魔");
        }
    }

    public void UseMagic(int consume)
    {
        _animator.SetBool("GetHit", true);
        _audiosource.PlayOneShot(GetComponent<PlayerController>().hurtVoiceClip);
        currentMagic -= consume;
        playerMagic.fillAmount = currentMagic / maxMagic;
        magicText.text = currentMagic + "/" + maxMagic;

        print("你消耗了" + consume + "滴魔");
        print("目前魔力為：" + currentMagic);

        if (currentMagic <= 0)
        {
            print("你已經沒有魔力了！");
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
