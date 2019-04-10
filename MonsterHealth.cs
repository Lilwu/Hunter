using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class MonsterHealth : MonoBehaviour
{
    private Animator _animator;
    private AudioSource m_audioSource;
    public AudioClip hurtClip;
    public AudioClip hitClip;
    public AudioClip deathClip;

    public Material m_material;

    //HUD
    public Image m_health;
    public Text m_nameText;
    private string _name;
    private float maxHealth;
    private int currentHealth;
    private bool isDead;
    private bool isSinking;

    //血條追隨攝影機
    public GameObject m_healthBar;
    public Camera main_camera;

    //動畫回朔系統
    static int takeDamged = Animator.StringToHash("Base Layer.block_hit");
    public AnimatorStateInfo BS;

    //任務事件
    private MissionManager missionManager;
    public delegate void MissionAction();
    public static event MissionAction MissionActionEvent;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        m_audioSource = GetComponent<AudioSource>();
        missionManager = FindObjectOfType<MissionManager>();
    }

    private void Start()
    {
        //HUD
        maxHealth = GetComponent<Monster>().m_maxHp;
        currentHealth = GetComponent<Monster>().m_hp;
        _name = GetComponent<Monster>().m_name;
        m_nameText.text = _name;
    }

    public void TakeDamge(int damage)
    {
        if (isDead) return;

        _animator.SetBool("Damaged", true);
        m_audioSource.PlayOneShot(hitClip);
        m_audioSource.PlayOneShot(hurtClip);

        //怪物噴血 20190308
        GameObject bloody = Resources.Load("Bloody_Effect") as GameObject;
        GameObject attckEffect = Resources.Load("attckEffect") as GameObject;
        Instantiate(bloody, transform.GetChild(0)).transform.position = transform.position;
        Instantiate(attckEffect, transform.GetChild(0)).transform.position = transform.position;

        gameObject.transform.position -= Vector3.back * 2;

        //HUD
        currentHealth -= damage;
        m_health.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        print("消滅了" + _name);
        _animator.SetTrigger("IsDead");
        m_audioSource.PlayOneShot(deathClip);
        isDead = true;

        tag = "Untagged";
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Collider>().enabled = false;  //防止金幣彈走
        GetComponent<Rigidbody>().isKinematic = true; //防止死亡時移動
        GetComponent<MonsterController>().enabled = false;
        GetComponent<GenerateFont>().enabled = false;
        GetComponent<AudioSource>().clip = null;
        GetComponent<Monster>().FallMoney(this.transform);
        GetComponent<Monster>().GetExp();

        if(missionManager.ISDESTROYMISSION == true && missionManager.MISSIONMOSNAME == _name)
        {
            Debug.Log("目前進行任務中" + _name);
            if(MissionActionEvent != null)
            {
                MissionActionEvent();
            }
        }
    }

    public void StartSinking()
    {
        isSinking = true;
        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        //血條追隨攝影機
        m_healthBar.transform.LookAt(m_healthBar.transform.position + main_camera.transform.rotation * Vector3.back,
                                        main_camera.transform.rotation * Vector3.up);

        //動畫完成機制 20190310
        BS = _animator.GetCurrentAnimatorStateInfo(0);

        if (BS.nameHash == takeDamged)
        {
            _animator.SetBool("Damaged", false);
        }
    }
}
