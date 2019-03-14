using System;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] InventoryInput inventoryInput;
    public Item setting;
    public AudioClip itemClip;
    private AudioSource audioSource;

    public event Action<ItemSlot> OnDropEvent;

    private void OnValidate()
    {
        inventory = FindObjectOfType<Inventory>();
        inventoryInput = FindObjectOfType<InventoryInput>();
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPickUp()
    {
        Destroy(gameObject);
    }

    public void PickupItem(ItemBase item)
    {
        inventory.AddItem(setting);
    }
}
