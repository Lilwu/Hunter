using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RestoreItemType
{
    Potion,
    Scroll,
}

[CreateAssetMenu]
public class RestorableItem : Item
{
    public int add_HP;       //回復血量
    public int add_MP;       //回復魔力
    public int add_Speed;    //增加速度
    [Space]
    public bool BackToTown;   //傳送回村莊

    public RestoreItemType restoreItemType;

    //20190225
    public bool IsConsumable;
    private AudioClip restoreSound;

    public virtual void Use(InventoryManager inventoryManager)
    {
        //恢復體力藥水
        if(add_HP != 0)
        {
            Debug.Log("增加玩家:" + add_HP + "的血量");

            restoreSound = GameObject.Find("Character").GetComponent<PlayerController>().restoreSound;
            GameObject.Find("Character").GetComponent<AudioSource>().PlayOneShot(restoreSound);
            GameObject restoreFX_red = Resources.Load("RestoreFX_Red") as GameObject;
            Instantiate(restoreFX_red).transform.localPosition = GameObject.Find("Character").transform.position;

            FindObjectOfType<PlayerHealth>().AddHp(add_HP);
        }

        else if(add_MP != 0)
        {
            Debug.Log("增加玩家:" + add_MP + "的血量");

            restoreSound = GameObject.Find("Character").GetComponent<PlayerController>().restoreSound;
            GameObject.Find("Character").GetComponent<AudioSource>().PlayOneShot(restoreSound);
            GameObject restoreFX_red = Resources.Load("RestoreFX_Blue") as GameObject;
            Instantiate(restoreFX_red).transform.localPosition = GameObject.Find("Character").transform.position;

            FindObjectOfType<PlayerMagic>().AddMp(add_MP);
        }
    }

    public override Item GetCopy()
    {
        return Instantiate(this);
    }
}
