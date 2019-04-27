using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image playerHealth;
    public Text healthText;
    public ParticleSystem hitEffect;
    public GameObject restartPanel;
    private float maxHealth;
    private int currentHealth;
    private AudioSource _audiosource;
    private Animator _animator;
    protected bool isDeath;

    private void Awake()
    {
        _audiosource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        ResetHealth();
    }

    public void AddHp(int amount)
    {
        if(currentHealth >= maxHealth)
        {
            FindObjectOfType<StatePanel>().SetSateText("沒發生任何事"); //顯示StatePanel

            return;
        }
        else if(currentHealth >= maxHealth -amount && currentHealth <= maxHealth)
        {
            currentHealth = (int)maxHealth;
            playerHealth.fillAmount = currentHealth / maxHealth;
            healthText.text = currentHealth + "/" + maxHealth;

            FindObjectOfType<StatePanel>().SetSateText("您的血量已經恢復滿了"); //顯示StatePanel
        }
        else
        {
            currentHealth += amount;
            playerHealth.fillAmount = currentHealth / maxHealth;
            healthText.text = currentHealth + "/" + maxHealth;

            FindObjectOfType<StatePanel>().SetSateText("您增加了" + amount + "滴血"); //顯示StatePanel
        }
    }

    public void TakeDamge(int damage)
    {
        if(!isDeath)
        {
            _animator.SetBool("GetHit", true);
            _audiosource.PlayOneShot(GetComponent<PlayerController>().hurtVoiceClip);
            currentHealth -= damage;
            playerHealth.fillAmount = currentHealth / maxHealth;
            healthText.text = currentHealth + "/" + maxHealth;

            FindObjectOfType<StatePanel>().SetSateText("您被扣了" + damage + "滴血"); //顯示StatePanel

            hitEffect.Play();

            if (currentHealth <= 0)
            {
                Death();
            }
        }
    }

    void Death()
    {
        isDeath = true;

        _animator.SetTrigger("IsDeath");
        healthText.text = 0 + "/" + maxHealth;
        GetComponent<PlayerController>().enabled = false;
        FindObjectOfType<CameraFollow>().enabled = false;
        _audiosource.PlayOneShot(GetComponent<PlayerController>().deathSoundClip);
        FindObjectOfType<StatePanel>().SetSateText("您已經死亡！"); //顯示StatePanel

        Invoke("OpenRestartPanel", 2.0f);
    }

    public bool ISDEATH
    {
        get { return isDeath; }
    }

    private void OpenRestartPanel()
    {
        restartPanel.SetActive(true);
    }

    public void RestartGame()
    {
        _animator.ResetTrigger("IsDeath");
        _animator.SetTrigger("IsLife");
        GetComponent<PlayerController>().enabled = true;
        FindObjectOfType<CameraFollow>().enabled = true;
        AddHp(500);
        restartPanel.SetActive(false);
        FindObjectOfType<GameLoader>().LoadScene(1);
    }

    public void ResetHealth()
    {
        maxHealth = GetComponent<Player>().MaxHP;
        currentHealth = GetComponent<Player>().HP;
        healthText.text = currentHealth + "/" + maxHealth;

        print("最大血量為：" + maxHealth);
        print("目前血量為：" + currentHealth);
    }
}