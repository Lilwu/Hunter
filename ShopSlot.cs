using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    public Text itemAmount;
    public Text itemName;
    public Text itemPrice;
    public Inventory inventory;

    private ShopPaneal shopPaneal;
    private Image _image;
    private int _itemamount;
    private int _itemprice;
    private Color normalColor = Color.white;

    //商品顯示價錢Tooltip 20190417
    public event Action<ShopSlot> OnPointerEnterEvent;
    public event Action<ShopSlot> OnPointerExitEvent;

    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    private void Start()
    {
        shopPaneal = GetComponentInParent<ShopPaneal>();
        _image = GetComponent<Image>();
        _itemamount = 0;
        _itemprice = item.Price;
        itemAmount.text = _itemamount.ToString();
        _image.sprite = item.Icon;
        _image.color = normalColor;
        itemName.text = item.ItemName;
        itemPrice.text = (_itemamount * _itemprice).ToString();
    }

    public void AddItemAmonut()
    {
        _itemamount++;
        itemAmount.text = _itemamount.ToString();
        itemPrice.text = (_itemamount * _itemprice).ToString();

        shopPaneal.AddTotalPrice(_itemprice);
    }

    public void LessItemAmount()
    {
        if(_itemamount >0)
        {
            _itemamount--;
            itemAmount.text = _itemamount.ToString();
            itemPrice.text = (_itemamount * _itemprice).ToString();
            shopPaneal.LessTotalPrice(_itemprice);
        }
        else if (_itemamount <= 0)
        {
            print("_itemamount" + _itemamount);
        }
    }

    public void AddBuyItem()
    {
        inventory.BuyToInventory(item, _itemamount);
    }

    public void ResetItemAmount()
    {
        _itemamount = 0;
        itemAmount.text = _itemamount.ToString();
        itemPrice.text = (_itemamount * _itemprice).ToString();
    }

    //商品顯示價錢Tooltip 20190417
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (OnPointerEnterEvent != null)
            OnPointerEnterEvent(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //tooltip.HideTooltip();
        if (OnPointerExitEvent != null)
            OnPointerExitEvent(this);
    }
}
