using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventoryManager : MonoBehaviour
{
    public ItemTooltip itemTooltip;
    public Image draggableItem;
    public GameObject Hand;

    private Inventory inventory;
    private EquipmentPanel equipmentPanel;
    private HotkeyBar hotkeyBar;
    private ItemSlot draggedSlot;

    //裝備武器事件
    public delegate void WeaponInHandAction();
    public static event WeaponInHandAction WeaponInHandEvent;


    private void OnValidate()
    {
        if (itemTooltip == null)
            itemTooltip = FindObjectOfType<ItemTooltip>();
    }

    private void Awake()
    {
        inventory = GetComponentInChildren<Inventory>();
        equipmentPanel = GetComponentInChildren<EquipmentPanel>();
        hotkeyBar = GetComponentInChildren<HotkeyBar>();
        //滑鼠游標移動物品 20190221

        //Setup Events:
        //Right Click
        inventory.OnRightClickEvent += InventoryRightClick;
        equipmentPanel.OnRightClickEvent += EquipmentPanelRightClick;
        //Pointer Enter
        inventory.OnPointerEnterEvent += ShowTooltip;
        equipmentPanel.OnPointerEnterEvent += ShowTooltip;
        //Pointer Exit
        inventory.OnPointerExitEvent += HideTooltip;
        equipmentPanel.OnPointerExitEvent += HideTooltip;
        //Begin Drag
        inventory.OnBeginDragEvent += BeginDrag;
        equipmentPanel.OnBeginDragEvent += BeginDrag;
        hotkeyBar.OnBeginDragEvent += BeginDrag;
        //End Drag
        inventory.OnEndDragEvent += EndDrag;
        equipmentPanel.OnEndDragEvent += EndDrag;
        hotkeyBar.OnEndDragEvent += EndDrag;
        //Drag
        inventory.OnDragEvent += Drag;
        equipmentPanel.OnDragEvent += Drag;
        hotkeyBar.OnDragEvent += Drag;
        //Drop
        inventory.OnDropEvent += Drop;
        equipmentPanel.OnDropEvent += Drop;
        //20190227
        hotkeyBar.OnDropEvent += Drop;
    }

    private void EquipmentPanelRightClick(ItemSlot itemSlot)
    {
        if (itemSlot.Item is EquippableItem)
        {
            Unequip((EquippableItem)itemSlot.Item);
        }

    }

    private void InventoryRightClick(ItemSlot itemSlot)
    {
        if (itemSlot.Item is EquippableItem)
        {
            Equip((EquippableItem)itemSlot.Item);
        }

        if (itemSlot.Item is EquippableItem)
        {
            Equip((EquippableItem)itemSlot.Item);
        }
        else if (itemSlot.Item is RestorableItem)
        {
            RestorableItem restorable = (RestorableItem)itemSlot.Item;
            restorable.Use(this);

            inventory.RemoveItem(restorable);
            restorable.Destroy();

            if (restorable.IsConsumable)
            {
                inventory.RemoveItem(restorable);
                restorable.Destroy();
            }
        }
    }

    //滑鼠游標移動物品 20190221
    private void Equip(ItemSlot itemSlot)
    {
        EquippableItem equippableItem = itemSlot.Item as EquippableItem;
        if(equippableItem != null)
        {
            Equip(equippableItem);
        }
    }

    private void Unequip(ItemSlot itemSlot)
    {
        EquippableItem equippableItem = itemSlot.Item as EquippableItem;
        if (equippableItem != null)
        {
            Unequip(equippableItem);
        }
    }

    private void ShowTooltip(ItemSlot itemSlot)
    {
        EquippableItem equippableItem = itemSlot.Item as EquippableItem;
        if (equippableItem != null)
        {
            itemTooltip.transform.position = Input.mousePosition;
            itemTooltip.ShowTooltip(equippableItem);
        }

        //新增補助道具顯示功能 20190222
        RestorableItem restorableItem = itemSlot.Item as RestorableItem;
        if (restorableItem != null)
        {
            itemTooltip.transform.position = Input.mousePosition;
            itemTooltip.ShowTooltip(restorableItem);
        }
    }

    private void HideTooltip(ItemSlot itemSlot)
    {
        itemTooltip.HideTooltip();
    }

    private void BeginDrag(ItemSlot itemSlot)
    {
        if(itemSlot.Item != null)
        {
            draggedSlot = itemSlot;
            draggableItem.sprite = itemSlot.Item.Icon;
            draggableItem.transform.position = Input.mousePosition;
            draggableItem.enabled = true;
        }
    }

    private void EndDrag(ItemSlot itemSlot)
    {
        draggedSlot = null;
        draggableItem.enabled = false;

        //將物品欄上物品移至地面後新增物品並刪除圖案 TODO:依物品數量更新文字 【20190303】
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.tag == "Floor" && !EventSystem.current.IsPointerOverGameObject()) 
            {
                print(hit.collider.tag);
                print(itemSlot.Item.name);
                Object obj = Resources.Load(itemSlot.Item.name);
                GameObject gobj = Instantiate(obj) as GameObject;
                gobj.transform.position = hit.point;

                itemSlot.Item = null;
            }
        }
    }

    private void Drag(ItemSlot itemSlot)
    {
       draggableItem.transform.position = Input.mousePosition;
    }

    private void Drop(ItemSlot dropItemSlot)
    {
        if (draggedSlot == null) return;

        if (dropItemSlot.CanAddStack(draggedSlot.Item))
        {
            AddStacks(dropItemSlot);
        }
        else if (dropItemSlot.CanReceiveItem(draggedSlot.Item) && draggedSlot.CanReceiveItem(dropItemSlot.Item))
        {
            SwapItems(dropItemSlot);
            DropToHotkeyBar(dropItemSlot);
        }
    }
    //20190225
    private void AddStacks(ItemSlot dropItemSlot)
    {
        int numAddableStacks = dropItemSlot.Item.MaximumStacks - dropItemSlot.Amount;
        int stacksToAdd = Mathf.Min(numAddableStacks, draggedSlot.Amount);

        dropItemSlot.Amount += stacksToAdd;
        draggedSlot.Amount -= stacksToAdd;
    }

    private void SwapItems(ItemSlot dropItemSlot)
    {
        EquippableItem dragEquipItem = draggedSlot.Item as EquippableItem;
        EquippableItem dropEquipItem = dropItemSlot.Item as EquippableItem;

        if (dropItemSlot is EquipmentSlot)
        {
            print("test1");
            if (dragEquipItem != null) dragEquipItem.Equip(this);
            if (dropEquipItem != null) dropEquipItem.Unequip(this);
        }
        if (draggedSlot is EquipmentSlot)
        {
            print("test2");
            if (dragEquipItem != null) dragEquipItem.Unequip(this);
            if (dropEquipItem != null) dropEquipItem.Equip(this);
        }
        Item draggedItem = draggedSlot.Item;
        int draggedItemAmount = draggedSlot.Amount;

        draggedSlot.Item = dropItemSlot.Item;
        draggedSlot.Amount = dropItemSlot.Amount;

        dropItemSlot.Item = draggedItem;
        dropItemSlot.Amount = draggedItemAmount;
    }

    private void EquipFormInventory(Item item)
    {
        if(item is EquippableItem)
        {
            Equip((EquippableItem)item);
        }
        else if(item is UsableItem)
        {
            UsableItem usableItem = (UsableItem)item;
            usableItem.Use(this);

            if(usableItem.IsConsumable)
            {
                inventory.RemoveItem(usableItem);
                usableItem.Destroy();
            }
        }
    }

    private void UnequipFromEquipPanel(Item item)
    {
        if (item is EquippableItem)
        {
            Unequip((EquippableItem)item);
        }
    }

    public void Equip(EquippableItem item)
    {
        if (inventory.RemoveItem(item))
        {
            EquippableItem previousItem;
            if (equipmentPanel.AddItem(item, out previousItem))
            {
                if (previousItem != null)
                {
                    inventory.AddItem(previousItem);
                    previousItem.Unequip(this);
                }
                item.Equip(this);


                //裝備武器時同步到玩家手上 20190313
                if (item.equipmentType == EquipmentType.Weapon)
                {
                    Object obj = Resources.Load(item.ItemName);
                    GameObject gobj = Instantiate(obj) as GameObject;

                    gobj.tag = "Player";
                    gobj.transform.parent = Hand.transform;
                    gobj.transform.position = Hand.transform.position;
                    gobj.transform.rotation = Hand.transform.rotation;

                    if(WeaponInHandEvent != null)
                    {
                        WeaponInHandEvent();
                    }
                }
            }
            else
            {
                inventory.AddItem(item);
            }
        }
    }

    public void Unequip(EquippableItem item)
    {
        if (equipmentPanel.RemoveItem(item))
        {
            item.Unequip(this);
            inventory.AddItem(item);

            if (item.equipmentType == EquipmentType.Weapon)
            {
                Destroy(Hand.transform.GetChild(0).gameObject);
            }
        }
    }

    //SetHotKey 20190227
    private void DropToHotkeyBar(ItemSlot dropItemSlot)
    {
        RestorableItem dragToHotkeyItem = draggedSlot.Item as RestorableItem;
        RestorableItem dropToHotkeyItem = dropItemSlot.Item as RestorableItem;

        if (dropItemSlot is HotkeySlot)
        {
            if (dragToHotkeyItem != null)
            {
                hotkeyBar.SetHotkeyItem(dropItemSlot);
                //Instantiate(draggedSlot).Item = (RestorableItem)draggedSlot.Item;
                print("test3");
            }
            if (dropToHotkeyItem != null)
            {
                dropItemSlot.Item = (RestorableItem)dropItemSlot.Item;
                print("test4");
            }
        }
        if (draggedSlot is HotkeySlot)
        {
            if (dragToHotkeyItem != null) dragToHotkeyItem = (RestorableItem)dropItemSlot.Item;
            if (dropToHotkeyItem != null) dropToHotkeyItem = (RestorableItem)dropItemSlot.Item;
        }

    }

    //DropItem 20190303
    private void DropItem(ItemSlot dropItemSlot)
    {

    }

    public void UseHotkeyItem(HotkeySlot hotkeySlot)
    {
        if (hotkeySlot.Item is RestorableItem)
        {
            RestorableItem restorable = (RestorableItem)hotkeySlot.Item;
            restorable.Use(this);

            hotkeyBar.RemoveItem(restorable);
            restorable.Destroy();

            if (restorable.IsConsumable)
            {
                hotkeyBar.RemoveItem(restorable);
                restorable.Destroy();
            }
        }
    }
}
