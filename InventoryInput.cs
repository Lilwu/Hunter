using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryInput : MonoBehaviour
{
    [SerializeField] GameObject equipmentPanelGameObject;
    [SerializeField] GameObject inventoryPanelGameObject;
    [SerializeField] GameObject messagePanelGameObject;
    [SerializeField] ItemTooltip itemTooltip;

    [SerializeField] InventoryManager inventoryManager;

    //20190227 SetHotkey
    [SerializeField] Transform hotkeySlotParent;
    [SerializeField] HotkeySlot[] hotkeySlot;

    [SerializeField] KeyCode[] toggleEquipmentKeys;
    [SerializeField] KeyCode[] toggleInventoryKeys;

    private void OnValidate()
    {
        hotkeySlot = hotkeySlotParent.GetComponentsInChildren<HotkeySlot>();
    }

    void Update()
    {
        for (int i = 0; i < toggleInventoryKeys.Length; i++)
        {
            if(Input.GetKeyDown(toggleInventoryKeys[i]))
            {
                itemTooltip.HideTooltip(); //不論開啟關閉物品欄，tooltip始終保持關閉，直到游標指定物品才顯示
                inventoryPanelGameObject.SetActive(!inventoryPanelGameObject.activeSelf);
                break;
            }
        }

        for (int i = 0; i < toggleEquipmentKeys.Length; i++)
        {
            if (Input.GetKeyDown(toggleEquipmentKeys[i]))
            {
                itemTooltip.HideTooltip(); //不論開啟關閉裝備欄，tooltip始終保持關閉，直到游標指定物品才顯示
                equipmentPanelGameObject.SetActive(!equipmentPanelGameObject.activeSelf);
                break;
            }
        }

        for (int i = 0; i < hotkeySlot.Length; i++)
        {
            if (Input.GetKeyDown(hotkeySlot[i]._key))
            {
                hotkeySlot[i]._button.onClick.Invoke();
            }
        }
    } 

    public void CloseGameObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public void OpenMessagePanel(string text)
    {
        messagePanelGameObject.SetActive(true);
    }

    public void CloseMessagePanel()
    {
        messagePanelGameObject.SetActive(false);
    }
}