using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    [SerializeField] Inventory inventory;

    public event Action<ItemSlot> OnDropEvent;

    private void OnValidate()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public void OnPickUp()
    {
        Destroy(gameObject);
    }
}
