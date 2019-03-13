using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UsableItem : Item
{
    //20190225
    public bool IsConsumable;


    public virtual void Use(InventoryManager inventoryManager)
    {

    }

}
