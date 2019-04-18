using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour
{
    public List<ItemSlot> itemSlots;

    //撿取物品 20190222

    public event Action<ItemSlot> OnRightClickEvent;

    //滑鼠游標移動物品 20190221
    public event Action<ItemSlot> OnPointerEnterEvent;
    public event Action<ItemSlot> OnPointerExitEvent;
    public event Action<ItemSlot> OnBeginDragEvent;
    public event Action<ItemSlot> OnEndDragEvent;
    public event Action<ItemSlot> OnDragEvent;
    public event Action<ItemSlot> OnDropEvent;


    //滑鼠游標移動物品 20190221
    private void Start()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            itemSlots[i].OnPointerEnterEvent += OnPointerEnterEvent;
            itemSlots[i].OnPointerExitEvent += OnPointerExitEvent;
            itemSlots[i].OnRightClickEvent += OnRightClickEvent;
            itemSlots[i].OnBeginDragEvent += OnBeginDragEvent;
            itemSlots[i].OnEndDragEvent += OnEndDragEvent;
            itemSlots[i].OnDragEvent += OnDragEvent;
            itemSlots[i].OnDropEvent += OnDropEvent;
        }
    }

    private void OnValidate()
    {
            GetComponentsInChildren(includeInactive: true, result: itemSlots);
           
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if(itemSlots[i].Item == null || itemSlots[i].CanAddStack(item))
            {
                itemSlots[i].Item = item;
                //20190222
                //startingItem.Add(item);
                //20190224
                itemSlots[i].Amount++;
                return true;
            }
        }
        return false;
    }

    public bool RemoveItem(Item item)
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].Item == item)
            {
                //20190224
                itemSlots[i].Amount--;
                //startingItem.Remove(item);
                return true;
            }
        }
        return false;
    }
    //20190224 新增物品ID功能 
    public Item RemoveItem(string itemID)
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            Item item = itemSlots[i].Item;
            if(item != null && item.ID == itemID)
            {
                itemSlots[i].Amount--;
                return item;
            }
        }
        return null;
    }

    //public bool IsFull()
    //{
    //    for (int i = 0; i < itemSlots.Count; i++)
    //    {
    //        if (itemSlots[i].Item == null)
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}

    //20190224 新增物品ID功能
    public int ItemCount(string itemID)
    {
        int number = 0;

        for (int i = 0; i < itemSlots.Count; i++)
        {
            if(itemSlots[i].Item.ID == itemID)
            {
                number++;
            }
        }
        return number;
    }

    //20190325 購買商品
    public void BuyToInventory(Item item , int amount)
    {
        for (int j = 0; j < amount; j++)
        {
            AddItem(item);
        }
    }
}
