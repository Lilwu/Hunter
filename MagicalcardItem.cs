using UnityEngine;

public enum MagicalcardType
{
    攻擊型魔法,
    輔助型魔法,
}

[CreateAssetMenu]
public class MagicalcardItem : Item
{
    public int damage;
    public int coldtime;
    public int takeMagic;
    public string animation;
    public string effectName;
    [Space]
    public MagicalcardType magicalcardType;

    public virtual void Use(InventoryManager inventoryManager)
    {
        FindObjectOfType<StatePanel>().SetSateText("使用魔法書： " + ItemName); //顯示StatePanel

        Player _player = FindObjectOfType<Player>();
        //攻擊型魔法
        if (magicalcardType == MagicalcardType.攻擊型魔法)
        {
            FindObjectOfType<PlayerController>().UseMagicalCard(animation, effectName);
            FindObjectOfType<PlayerMagic>().UseMagic(takeMagic);

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
