using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image playerHealth;
    public Text healthText;
    private float maxHealth;
    private int currentHealth;
    private AudioSource _audiosource;
    private Animator _animator;

    private void Awake()
    {
        _audiosource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        maxHealth = GetComponent<Player>().MaxHP;
        currentHealth = GetComponent<Player>().HP;
        healthText.text = currentHealth + "/" + maxHealth;

        print("最大血量為：" + maxHealth);
        print("目前血量為：" + currentHealth);
    }

    public void TakeDamge(int damage)
    {
        _animator.SetBool("GetHit", true);
        _audiosource.PlayOneShot(GetComponent<PlayerController>().hurtVoiceClip);
        currentHealth -= damage;
        playerHealth.fillAmount = currentHealth / maxHealth;
        healthText.text = currentHealth + "/" + maxHealth;

        print("你被扣了" + damage + "滴血");
        print("目前血量為：" + currentHealth);

        //被攻擊特效 20190318
        GameObject hitEffect = Resources.Load("hitEffect") as GameObject;
        Instantiate(hitEffect).transform.localPosition = transform.GetChild(0).transform.position;

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        healthText.text = 0 + "/" + maxHealth;
        print("你已經死亡！");
        print("目前血量為：" + currentHealth);
    }
}