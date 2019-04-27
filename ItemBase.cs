using System;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    //public Inventory inventory;
    public Item setting;
    public AudioClip itemClip;
    private AudioSource audioSource;

    public event Action<ItemSlot> OnDropEvent;

    //HUD追隨攝影機
    public GameObject itemHUD;
    private Camera main_camera;

    private void OnValidate()
    {
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
        FindObjectOfType<Inventory>().AddItem(setting);
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
