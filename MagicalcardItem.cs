using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MagicalcardType
{
    Attack,
    Restore
}

[CreateAssetMenu]
public class MagicalcardItem : Item
{
    public int damage;
    public int coldtime;
    public string animation;
    public string effectName;
    [Space]
    public MagicalcardType magicalcardType;

    public virtual void Use(InventoryManager inventoryManager)
    {
        Debug.Log("使用魔法書");
        Player _player = FindObjectOfType<Player>();

        //攻擊型魔法
        if (magicalcardType == MagicalcardType.Attack)
        {
            Debug.Log("使用了:" + ItemName);
            FindObjectOfType<PlayerController>().UseMagicalCard(animation, effectName);

            Collider[] objs = Physics.OverlapSphere(GameObject.Find("Character").transform.position + Vector3.up, 4.0f, 1 << LayerMask.NameToLayer("Enemy"));

            if(objs.Length <= 0)
            {
                return;
            }

            for (int i = 0; i < objs.Length; i++)
            {
                Debug.Log(objs[i].gameObject.name);
                int r_attack = Random.Range(damage - 15, damage + 30);
                objs[i].GetComponent<MonsterHealth>().TakeDamge(r_attack);
                objs[i].GetComponent<GenerateFont>().AttackPointFont(r_attack);
                FindObjectOfType<DamageFont>().playerAttackPoint = r_attack;
            }
        }
    }
}
