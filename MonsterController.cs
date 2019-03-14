using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    public Material m_material;
    private Animator _animator;
    private AudioSource _audiosource;

    //追蹤玩家
    PlayerController isAttack;
    private Transform player;
    private NavMeshAgent nav;
    private bool playerInRange;
    private bool InAttackRange = false; //玩家可攻擊範圍

    //攻擊玩家
    private int attackDamage;
    //private PlayerHealth playerHealth;

    private float timer;
    private bool playerisDeath = false;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        _audiosource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void GoToPlayer()
    {
        //追蹤玩家位置
        nav.destination = player.position;
        _animator.SetBool("IsRun", true);
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
        if (playerInRange)
        {
            GoToPlayer();
        }
    }
}
