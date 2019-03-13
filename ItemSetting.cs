using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//給GameObject抓取Item.asset數值;
public class ItemSetting : MonoBehaviour
{
    [SerializeField] Inventory inventory;

    public Item setting;

    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public void PickUpItem()
    {
        GetComponent<ItemBase>().OnPickUp();
        inventory.AddItem(setting);
        Debug.Log("消除物件" + setting.ItemName);
    }
}
