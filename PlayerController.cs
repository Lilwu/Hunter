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
    private AnimatorStateInfo Bs;

    private bool WeaponInHand; //判斷武器是否在手上
    public GameObject Hand;


    private AudioSource _audioSource;
    public AudioClip attackVoiceClip;
    public AudioClip pickupSoundClip;
    public AudioClip hurtVoiceClip;
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

        if(Bs.fullPathHash == attack01ID || Bs.fullPathHash == pickupID || Bs.fullPathHash == gethitID)
        {
            _anim.SetBool("IsAttack" , false);
            _anim.SetBool("IsPickup", false);
            _anim.SetBool("GetHit", false);
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
    }

    public void NewAttackPoint()
    {

    }

    public void AttackMonster() //攻擊模式 event
    {
        _audioSource.PlayOneShot(attackVoiceClip , 0.5f);

        if (WeaponInHand)
        {
            _audioSource.PlayOneShot(itemAudio);
        }

        if(mosInRange && mNearMonster != null)
        {
            int r_attack = Random.Range(_player.ATTACK - 15, _player.ATTACK + 20);

            FindObjectOfType<MonsterHealth>().TakeDamge(r_attack);
            FindObjectOfType<GenerateFont>().AttackPointFont(r_attack);
            FindObjectOfType<DamageFont>().playerAttackPoint = r_attack;

            print(r_attack);
        }
    }
    public void AttackOverPositsion() //攻擊結束時位移 event
    {
        _moveDir = Vector3.forward * 2;
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
    private MonsterHealth mNearMonster = null;

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

        MonsterHealth mos = other.GetComponent<MonsterHealth>();
        if (mos != null && other.tag == "Monster")
            mNearMonster = mos;
            mosInRange = true;

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

        MonsterHealth mos = other.GetComponent<MonsterHealth>();
        if (mos != null && other.tag == "Monster")
            mosInRange = false;

        Money money = other.GetComponent<Money>();
        if (money != null && other.tag == "Money")
        {
            inventoryInput.CloseMessagePanel();
            mItemToPickup = null;
        }
    }
}
