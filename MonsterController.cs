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

    public void TakeDamageToPlayer(float value) 
    {
        _audiosource.PlayOneShot(GetComponent<MonsterHealth>().hitClip , value);
        playerHealth.TakeDamge(attackDamage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            InAttackRange = true;
            if(m_material != null)
            m_material.SetFloat("_OutlineWidth", 1.05f);
        }

        if (other.tag == "TrackRange")
        {
            FindObjectOfType<StatePanel>().SetSateText("被敵人發現了！"); //顯示StatePanel

            playerInRange = true;
            _audiosource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            InAttackRange = false;
            if(m_material != null)
            m_material.SetFloat("_OutlineWidth", 1.0f);
        }

        if (other.tag == "TrackRange")
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (gameObject.tag == "Dragon")
        {
            if (dis > 1.7f)
            {
                GoToPlayer();
            }
            else if (dis <= 1.7f && dis > 0f && !playerHealth.ISDEATH)
            {
                _animator.SetBool("IsRun", false);
                _animator.SetBool("IsAttack", true);
            }
            else if (playerHealth.ISDEATH)
            {
                _animator.SetBool("IsAttack", false);
            }

            dis = Vector3.Distance(player.position, transform.position);
        }

        if(gameObject.tag == "Monster")
        {
            if (dis > 1f)
            {
                GoToPlayer();
            }
            else if (dis <= 1f && dis > 0f && !playerHealth.ISDEATH)
            {
                _animator.SetBool("IsRun", false);
                _animator.SetBool("IsAttack", true);
            }
            else if (playerHealth.ISDEATH)
            {
                _animator.SetBool("IsAttack", false);
            }

            dis = Vector3.Distance(player.position, transform.position);
        }
    }
}
