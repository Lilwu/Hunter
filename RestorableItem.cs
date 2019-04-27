using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum RestoreItemType
{
    藥水,
    卷軸,
    修復,
    禮物,
}

[CreateAssetMenu]
public class RestorableItem : Item
{
    public int add_HP;       //回復血量
    public int add_MP;       //回復魔力
    public int add_Attack;    //增加攻擊力
    public int add_Grind;   //恢復磨損度
    [Space]
    public bool BackToTown;   //傳送回村莊

    [Header("禮物")] //新增新手禮盒 20190418
    public bool IsPresent;
    public Item[] presents; //禮物
    private Inventory inventory;

    public RestoreItemType restoreItemType;

    //20190225
    public bool IsConsumable;
    private AudioClip restoreSound;

    public virtual void Use(InventoryManager inventoryManager)
    {
        FindObjectOfType<StatePanel>().SetSateText("使用 " + ItemName); //顯示StatePanel

        //恢復體力藥水
        if (add_HP != 0)
        {
            restoreSound = GameObject.Find("Character").GetComponent<PlayerController>().restoreSound;
            GameObject.Find("Character").GetComponent<AudioSource>().PlayOneShot(restoreSound);
            GameObject restoreFX_red = Resources.Load("RestoreFX_Red") as GameObject;
            Instantiate(restoreFX_red).transform.localPosition = GameObject.Find("Character").transform.position;

            FindObjectOfType<PlayerHealth>().AddHp(add_HP);
        }

        else if (add_MP != 0)
        {
            restoreSound = GameObject.Find("Character").GetComponent<PlayerController>().restoreSound;
            GameObject.Find("Character").GetComponent<AudioSource>().PlayOneShot(restoreSound);
            GameObject restoreFX_red = Resources.Load("RestoreFX_Blue") as GameObject;
            Instantiate(restoreFX_red).transform.localPosition = GameObject.Find("Character").transform.position;

            FindObjectOfType<PlayerMagic>().AddMp(add_MP);
        }

        else if (add_Attack != 0)
        {
            restoreSound = GameObject.Find("Character").GetComponent<PlayerController>().restoreSound;
            GameObject.Find("Character").GetComponent<AudioSource>().PlayOneShot(restoreSound);
            GameObject restoreFX_green = Resources.Load("RestoreFX_Green") as GameObject;
            Instantiate(restoreFX_green).transform.localPosition = GameObject.Find("Character").transform.position;

            FindObjectOfType<Player>().AddWeaponAttack(add_Attack);
        }

        else if (BackToTown)
        {
            FindObjectOfType<GameLoader>().LoadScene(1);
        }

        else if (IsPresent)
        {
            inventory = FindObjectOfType<Inventory>();

            for (int i = 0; i < presents.Length; i++)
            {
                inventory.AddItem(presents[i]);
                FindObjectOfType<StatePanel>().SetSateText("<color=yellow>" + "獲得" + "</color>" + "<color=yellow>" + presents[i].ItemName + "</color>");
                FindObjectOfType<InventoryInput>().GetComponent<AudioSource>().PlayOneShot(FindObjectOfType<InventoryInput>().awardClip);
            }
        }
    }

    public override Item GetCopy()
    {
        return Instantiate(this);
    }
}
