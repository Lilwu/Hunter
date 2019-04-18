using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public string m_name;
    public int m_hp;
    public int m_maxHp;
    public int moneyMix;
    public int moneyMax;
    public int m_attack;
    public int m_exp;

    public Item[] m_fallItem;
    private Vector3 itemPos;

    private void Awake()
    {
        m_hp = m_maxHp;
        itemPos = new Vector3(1, 0, 0);
    }

    public void FallMoney(Transform transform)
    {
        GameObject coins = Resources.Load("Coins") as GameObject;
        Instantiate(coins).transform.position = transform.GetChild(0).transform.position;
        FindObjectOfType<Money>().SetMoney(moneyMix, moneyMax);
    }

    public void GetExp()
    {
        FindObjectOfType<Player>().AddPlayerExp(m_exp);
        //顯示StatePanel
        FindObjectOfType<StatePanel>().SetSateText("<color=yellow>" + "您獲得了 " + "</color>" + "<color=yellow>" + m_exp + "</color>" + "<color=yellow>" + " 的經驗值" + "</color>");
    }

    public void FallItem()
    {
        if(m_fallItem != null)
        {
            for (int i = 0; i < m_fallItem.Length; i++)
            {
                GameObject fallitems = Resources.Load(m_fallItem[i].ItemName) as GameObject;
                Instantiate(fallitems).transform.position = transform.GetChild(0).transform.position + itemPos;
            }
        }
    }
}
