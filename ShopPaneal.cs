using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPaneal : MonoBehaviour
{
    public Transform shopslotParent;
    public Inventory inventory;
    public GameObject shopPanel;

    public ShopSlot[] shopSlots;
    public List<Item> items;

    private Player _player;
    private int _totalPrice;
    private NPC _npc;
    private Color normalColor = Color.white;
    private Color nonMoneyColor = Color.red;

    private void OnValidate()
    {
        shopSlots = shopslotParent.GetComponentsInChildren<ShopSlot>();
    }

    private void Awake()
    {
        _player = GameObject.Find("Character").GetComponent<Player>();
        _npc = GetComponentInParent<NPC>();
        ResetShopPanel();
    }

    public void ClosePanel()
    {
        shopPanel.SetActive(false);
        _npc.ChangeMessage(_npc._npcCancelMessage);
        ResetShopPanel();
    }

    //結帳
    public void BuyItems()
    {
        if (_player.MONEY >= _totalPrice)
        {
            print("購買完成！");
            _npc.ChangeMessage(_npc._npcFinishedMessage);

            for (int i = 0; i < shopSlots.Length; i++)
            {
                shopSlots[i].AddBuyItem();
            }

            _player.LessMoney(_totalPrice);
            ResetShopPanel();
            shopPanel.SetActive(false);
        }
        else
        {
            print("購買失敗，您的金幣不足！");
            _npc.ChangeMessage(_npc._npcNonMoneyMessage);
            ResetShopPanel();
            shopPanel.SetActive(false);
        }
    }

    //增加總金額(物品價錢)
    public void AddTotalPrice(int price)
    {
        _totalPrice += price;
        CheckTextColor();
    }
    //減少總金額(物品價錢)
    public void LessTotalPrice(int price)
    {
        _totalPrice -= price;
        CheckTextColor();
    }
    //檢查玩家金幣是否足夠，不足將商品金額改成紅色字體
    private void _CheckTextColor()
    {
        if (_player.MONEY < _totalPrice)
        {
            for (int i = 0; i < shopSlots.Length; i++)
            {
                shopSlots[i].itemPrice.color = nonMoneyColor;
            }
        }
        else if (_player.MONEY >= _totalPrice)
        {
            for (int i = 0; i < shopSlots.Length; i++)
            {
                shopSlots[i].itemPrice.color = normalColor;
            }
        }
    }

    private void CheckTextColor()
    {
        for (int i = 0; i < shopSlots.Length; i++)
        {
            if (_player.MONEY < _totalPrice)
            {
                shopSlots[i].itemPrice.color = nonMoneyColor;
            }
            else if (_player.MONEY >= _totalPrice)
            {
                shopSlots[i].itemPrice.color = normalColor;
            }
        }
    }

    public void ResetShopPanel()
    {
        _totalPrice = 0;

        for (int i = 0; i < shopSlots.Length; i++)
        {
            shopSlots[i].item = items[i];
            shopSlots[i].ResetItemAmount();
        }
    }
}
