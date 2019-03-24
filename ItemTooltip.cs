using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] Text ItemNameText;
    [SerializeField] Text ItemSlotText;
    [SerializeField] Text ItemStatText;

    private StringBuilder sb = new StringBuilder();

    //裝備顯示
    public void ShowTooltip(EquippableItem item)
    {
        ItemNameText.text = item.ItemName;
        ItemSlotText.text = item.equipmentType.ToString();

        sb.Length = 0;
        AddStat(item.Damge, "攻擊力");
        AddStat(item.Defense, "防禦力");
        AddStat(item.weight, "重量");

        //調整裝備顯示顏色(綠色)
        ItemStatText.color = Color.green;
        ItemStatText.text = sb.ToString();

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
        AddStat(item.add_Speed, "攻擊速度");

        //調整補助道具顯示顏色(紅色) 20190221
        ItemStatText.color = Color.red;
        ItemStatText.text = sb.ToString();

        gameObject.SetActive(true);
    }

    //魔法卡顯示 20190324
    public void ShowTooltip(MagicalcardItem item)
    {
        ItemNameText.text = item.ItemName;
        ItemSlotText.text = item.magicalcardType.ToString();

        sb.Length = 0;
        AddStat(item.damage, "魔法攻擊力");
        AddStat(item.coldtime, "冷卻時間");

        //調整補助道具顯示顏色(紅色) 20190324
        ItemStatText.color = Color.red;
        ItemStatText.text = sb.ToString();

        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
        transform.position = Input.mousePosition;
    }

    private void AddStat(float value , string statName)
    {
        if(value != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();


            if (value > 0)
                sb.Append("+");
            sb.Append(value);
            sb.Append(" ");
            sb.Append(statName);
        }

    }
}
