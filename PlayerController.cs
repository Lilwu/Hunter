using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player _player;
    private Animator _anim;
    private CharacterController _characterController;
    private InventoryInput inventoryInput;
    private Inventory inventory;
    private int speedID = Animator.StringToHash("Speed");
    private int speedYID = Animator.StringToHash("SpeedY");
    private int attack01ID = Animator.StringToHash("Base Layer.c_attack01");
    private int pickupID = Animator.StringToHash("Base Layer.c_pickup");
    private int gethitID = Animator.StringToHash("Base Layer.c_gethit");
    private int skill01ID = Animator.StringToHash("Base Layer.c_firedancing");

    private AnimatorStateInfo Bs;

    private bool WeaponInHand; //判斷武器是否在手上
    public GameObject Hand;


    private AudioSource _audioSource;
    public AudioClip attackVoiceClip;
    public AudioClip pickupSoundClip;
    public AudioClip hurtVoiceClip;
    public AudioClip restoreSound;
    public AudioClip fireDanceClip;
    private AudioClip itemAudio;

    //MOVEMENT
    private float Speed = 5.0f;
    private float RotationSpeed = 240.0f;
    private float Gravity = 20.0f;
    private Vector3 _moveDir = Vector3.zero;
    private bool actionSwitch;
    private bool mosInRange;
    private bool moneyInRange;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        inventoryInput = FindObjectOfType<InventoryInput>();
        inventory = FindObjectOfType<Inventory>();
    }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        actionSwitch = false;
        WeaponInHand = false;
        mosInRange = false;
    }

    private void WeaponInHandAction()
    {
        WeaponInHand = true;
        itemAudio = Hand.transform.GetChild(0).GetComponent<ItemBase>().itemClip;
        //同步武器攻擊力 20190318
        EquippableItem w_attack = Hand.transform.GetChild(0).GetComponent<ItemBase>().setting as EquippableItem;
        _player.AddWeaponAttack(w_attack.Damge);
    }

    private void OnEnable()
    {
        InventoryManager.WeaponInHandEvent += WeaponInHandAction;
    }

    private void OnDisable()
    {
        InventoryManager.WeaponInHandEvent -= WeaponInHandAction;
    }

    private void Update()
    {
        if (!actionSwitch)
        {
            //Movement TODO:Run
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            transform.Rotate(0, h * RotationSpeed * Time.deltaTime, 0);

            if (_characterController.isGrounded)
            {
                _anim.SetFloat(speedID, v * 1.5f);
                _anim.SetFloat(speedYID, h);
                _moveDir = Vector3.forward * v;
                _moveDir = transform.TransformDirection(_moveDir);
                _moveDir *= Speed;
            }
        }

        _moveDir.y -= Gravity * Time.deltaTime;
        _characterController.Move(_moveDir * Time.deltaTime);

        //Attack
        Bs = _anim.GetCurrentAnimatorStateInfo(0);

        if (Input.GetKeyDown(KeyCode.F))
        {
            _anim.SetBool("IsAttack", true);
        }

        if(Bs.fullPathHash == attack01ID || Bs.fullPathHash == pickupID || Bs.fullPathHash == gethitID || Bs.fullPathHash == skill01ID)
        {
            _anim.SetBool("IsAttack" , false);
            _anim.SetBool("IsPickup", false);
            _anim.SetBool("GetHit", false);

            if(Bs.fullPathHash == skill01ID)
            {
                _anim.SetBool("Skill01", false);
                AttackOverPositsion(1);
            }
        }

        //Pickup
        if(mItemToPickup != null && Input.GetKeyDown(KeyCode.V))
        {
            _anim.SetBool("IsPickup", true);
        }
        else if(mMoneyToPickup != null && Input.GetKeyDown(KeyCode.V))
        {
            _anim.SetBool("IsPickup", true);
        }

        //Skill_FireDancing
        if(Input.GetKeyDown(KeyCode.L))
        {
            _anim.SetBool("Skill01", true);
            _audioSource.PlayOneShot(fireDanceClip);

            GameObject FireDance = Resources.Load("FireDance") as GameObject;
            Instantiate(FireDance , GameObject.Find("WeaponHand").transform).transform.localPosition = transform.position;
        }
    }

    //使用魔法書 20190322
    public void UseMagicalCard(string animation , string effect)
    {
        _anim.SetBool(animation, true);
        _audioSource.PlayOneShot(fireDanceClip);
        
        GameObject Magicaleffect = Resources.Load(effect) as GameObject;

        if(GameObject.Find("MagicEffect").transform.FindChild(effect + "(Clone)") == null)
        {
            Instantiate(Magicaleffect, GameObject.Find("MagicEffect").transform).transform.localPosition = transform.position;
        }
        else
        {
            GameObject.Find(effect + "(Clone)").GetComponent<ParticleSystem>().Play();
        }


    }

    public void AttackMonster() //攻擊模式 event
    {
        _audioSource.PlayOneShot(attackVoiceClip , 0.5f);

        Ray ray = new Ray(transform.position + Vector3.up, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 2.0f, Color.cyan);


        if (WeaponInHand)
        {
            _audioSource.PlayOneShot(itemAudio);
        }

        if(Physics.Raycast(ray, out RaycastHit hit, 1.0f))
        {
            if (hit.collider.tag == "Monster")
            {
                int r_attack = Random.Range(_player.ATTACK - 15, _player.ATTACK + 20);

                FindObjectOfType<MonsterHealth>().TakeDamge(r_attack);
                FindObjectOfType<GenerateFont>().AttackPointFont(r_attack);
                FindObjectOfType<DamageFont>().playerAttackPoint = r_attack;

                print(r_attack);
            }
        }
    }
    public void AttackOverPositsion(int distance) //攻擊結束時位移 event
    {
        _moveDir = Vector3.forward * distance;
        _moveDir = transform.TransformDirection(_moveDir);
        _moveDir.y -= Gravity * Time.deltaTime;
        _characterController.Move(_moveDir * Time.deltaTime);

    }
    public void PickupItem() //玩家撿取物品&金幣 event
    {
        _audioSource.PlayOneShot(pickupSoundClip);

        if (mItemToPickup != null)
        {
            mItemToPickup.OnPickUp();
            mItemToPickup.PickupItem(mItemToPickup);
        }

        if(mMoneyToPickup != null)
        {
            mMoneyToPickup.OnPickUp();
            mMoneyToPickup.AddPlayerMoney();
        }

        inventoryInput.CloseMessagePanel();
    }

    public void actionStart() //events
    {
        actionSwitch = true;
    }
    public void actionEnd() //event
    {
        actionSwitch = false;
    }

    private ItemBase mItemToPickup = null;
    private Money mMoneyToPickup = null;

    private void OnTriggerEnter(Collider other)
    {
        ItemBase item = other.GetComponent<ItemBase>();
        if (item != null && other.tag == "Item")
        {
            //if (mLockPickup)
            // return;

            mItemToPickup = item;
            inventoryInput.OpenMessagePanel("");
        }

        Money money = other.GetComponent<Money>();
        if (money != null && other.tag == "Money")
        {
            mMoneyToPickup = money;
            inventoryInput.OpenMessagePanel("");
            moneyInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ItemBase item = other.GetComponent<ItemBase>();
        if (item != null && other.tag == "Item")
        {
            inventoryInput.CloseMessagePanel();
            mItemToPickup = null;
        }

        Money money = other.GetComponent<Money>();
        if (money != null && other.tag == "Money")
        {
            inventoryInput.CloseMessagePanel();
            mItemToPickup = null;
        }
    }
}
