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

    public virtual void Use(InventoryManager inventoryManager)
    {
        inventoryManager.Health += 50;
        Debug.Log("增加玩家50滴血量");
    }

    public override Item GetCopy()
    {
        return Instantiate(this);
    }
}
