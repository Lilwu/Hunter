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

    //HUD追隨攝影機
    public GameObject itemHUD;
    private Camera main_camera;

    private void OnValidate()
    {
        inventory = FindObjectOfType<Inventory>();
        inventoryInput = FindObjectOfType<InventoryInput>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        main_camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    public void OnPickUp()
    {
        Destroy(gameObject);
    }

    public void PickupItem(ItemBase item)
    {
        inventory.AddItem(setting);
    }

    private void Update()
    {
        //HUD追隨攝影機
        if (itemHUD != null)
        {
            itemHUD.transform.LookAt(itemHUD.transform.position + main_camera.transform.rotation * Vector3.back,
                                        main_camera.transform.rotation * Vector3.up);
        }
    }
}
