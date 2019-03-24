using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    public Material m_material;
    private Animator _animator;
    private AudioSource _audiosource;
    public AudioClip attackClip;

    //追蹤玩家
    private Transform player;
    private NavMeshAgent nav;
    private bool playerInRange;
    private bool InAttackRange = false; //玩家可攻擊範圍
    private float dis;

    //攻擊玩家
    private int attackDamage;
    private PlayerHealth playerHealth;

    private float timer;
    private bool playerisDeath = false;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        _audiosource = GetComponent<AudioSource>();
        attackDamage = GetComponent<Monster>().m_attack;
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void Start()
    {
        player = GameObject.Find("Character").transform;
    }

    private void GoToPlayer()
    {
        //追蹤玩家位置
        nav.destination = player.position;
        _animator.SetBool("IsAttack", false);
        _animator.SetBool("IsRun", true);
    }

    public void TakeDamageToPlayer() 
    {
        _audiosource.PlayOneShot(GetComponent<MonsterHealth>().hitClip , 0.2f);
        playerHealth.TakeDamge(attackDamage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            InAttackRange = true;
            m_material.SetFloat("_OutlineWidth", 1.05f);
        }

        if (other.tag == "TrackRange")
        {
            print("你被發現了！");
            playerInRange = true;
            _audiosource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            InAttackRange = false;
            m_material.SetFloat("_OutlineWidth", 1.0f);
        }

        if (other.tag == "TrackRange")
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (dis > 1.5f)
        {
            GoToPlayer();
        }
        else if(dis <= 1f && dis > 0f)
        {
            _animator.SetBool("IsRun", false);
            _animator.SetBool("IsAttack", true);
        }

        dis = Vector3.Distance(player.position, transform.position);
        //print(dis);
    }
}
