using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MoneyTooltip : MonoBehaviour
{
    [SerializeField] Text ItemNameText;
    [SerializeField] Text ItemSlotText;
    [SerializeField] Text ItemStatText;
    [SerializeField] Text ItemPriceText;

    private StringBuilder sb = new StringBuilder();
    private StringBuilder pb = new StringBuilder();

    public Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    //裝備顯示 20190417
    public void ShowTooltip(EquippableItem item)
    {
        ItemNameText.text = item.ItemName;
        ItemSlotText.text = item.equipmentType.ToString();

        sb.Length = 0;
        AddStat(item.Damge, "攻擊力");
        AddStat(item.Defense, "防禦力");

        ItemStatText.color = Color.white;
        ItemStatText.text = sb.ToString();

        //調整價錢顯示顏色(玩家金幣不足顯示紅色)
        pb.Length = 0;
        if(item.Price <= player.MONEY)
        {
            AddPrice(item.Price, "價錢");
            ItemPriceText.color = Color.green;
        }
        else if(item.Price > player.MONEY)
        {
            AddPrice(item.Price, "價錢");
            ItemPriceText.color = Color.red;
        }
        ItemPriceText.text = pb.ToString();

        gameObject.SetActive(true);
    }

    //補助道具顯示 20190221
    public void ShowTooltip(RestorableItem item)
    {
        ItemNameText.text = item.ItemName;
        ItemSlotText.text = item.restoreItemType.ToString();

        sb.Length = 0;
        AddStat(item.add_HP, "玩家體力");
        AddStat(item.add_MP, "玩家魔力");
        AddStat(item.add_Attack, "攻擊力");
        AddStateBool(item.BackToTown, "使用後傳送回村莊");

        ItemStatText.color = Color.white;
        ItemStatText.text = sb.ToString();

        //調整價錢顯示顏色(玩家金幣不足顯示紅色)
        pb.Length = 0;
        if (item.Price <= player.MONEY)
        {
            AddPrice(item.Price, "價錢");
            ItemPriceText.color = Color.green;
        }
        else if (item.Price > player.MONEY)
        {
            AddPrice(item.Price, "價錢");
            ItemPriceText.color = Color.red;
        }
        ItemPriceText.text = pb.ToString();

        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
        transform.position = Input.mousePosition;
    }

    private void AddStat(float value, string statName)
    {
        if (value != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();

            sb.Append(statName);
            sb.Append(" ");

            if (value > 0)
                sb.Append("+");
            sb.Append(value);
        }
    }

    private void AddPrice(float value, string statName)
    {
        if (value != 0)
        {
            if (pb.Length > 0)
                pb.AppendLine();

            pb.Append(statName);
            pb.Append(" ");

            if (value > 0)
                pb.Append(value);
        }
    }

    private void AddStateBool(bool value, string statName)
    {
        if (value != false)
        {
            if (sb.Length > 0)
                sb.AppendLine();

            sb.Append(statName);
        }
    }
}
