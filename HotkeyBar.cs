using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotkeyBar : MonoBehaviour
{
    [SerializeField] Transform hotkeySlotParent;
    [SerializeField] HotkeySlot[] hotkeySlots;
    [SerializeField] Inventory inventory;

    public event Action<ItemSlot> OnPointerEnterEvent;
    public event Action<ItemSlot> OnPointerExitEvent;
    public event Action<ItemSlot> OnBeginDragEvent;
    public event Action<ItemSlot> OnEndDragEvent;
    public event Action<ItemSlot> OnDragEvent;
    public event Action<ItemSlot> OnDropEvent;

    private void Start()
    {
        for (int i = 0; i < hotkeySlots.Length; i++)
        {
            hotkeySlots[i].OnPointerEnterEvent += OnPointerEnterEvent;
            hotkeySlots[i].OnPointerExitEvent += OnPointerExitEvent;
            hotkeySlots[i].OnBeginDragEvent += OnBeginDragEvent;
            hotkeySlots[i].OnEndDragEvent += OnEndDragEvent;
            hotkeySlots[i].OnDragEvent += OnDragEvent;
            hotkeySlots[i].OnDropEvent += OnDropEvent;
        }
    }

    private void OnValidate()
    {
        hotkeySlots = hotkeySlotParent.GetComponentsInChildren<HotkeySlot>();
        inventory = FindObjectOfType<Inventory>();
    }

    public void SetHotkeyItem(ItemSlot item)
    {
        for (int i = 0; i < hotkeySlots.Length; i++)
        {
            hotkeySlots[i].Item = item.Item;
        }
    }

    public bool RemoveItem(Item item)
    {
        for (int i = 0; i < hotkeySlots.Length; i++)
        {
            if (hotkeySlots[i].Item == item)
            {
                //20190224
                hotkeySlots[i].Amount--;
                //startingItem.Remove(item);
                return true;
            }
        }
        return false;
    }
}
