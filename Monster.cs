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

    private void Awake()
    {
        m_hp = m_maxHp;
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
        print("你獲得了" + m_exp + "的經驗值");
    }
}
