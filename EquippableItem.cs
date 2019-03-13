using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Helmet,
    Chest,
    Gloves,
    Boots,
    Weapon,
    Shield,
    Accessory1,
    Accessory2,
}

[CreateAssetMenu]
public class EquippableItem : Item
{
    public int StrengthBonus;
    public int AgilityBonus;
    [Space]

    public int Damge;      //傷害值
    public int Defense;    //防禦力
    public int Durability; //耐久度
    [Space]
    public int weight; //重量

    public EquipmentType equipmentType;


    public override Item GetCopy()
    {
        return Instantiate(this);
    }

    public override void Destroy()
    {
        Destroy(this);
    }


    //20190221
    public void Equip(InventoryManager inventoryManager)
    {

    }

    public void Unequip(InventoryManager inventoryManager)
    {

    }


}


