using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _anim;
    private CharacterController _characterController;
    private InventoryInput inventoryInput;
    private int speedID = Animator.StringToHash("Speed");
    private int speedYID = Animator.StringToHash("SpeedY");
    private int attack01ID = Animator.StringToHash("Base Layer.c_attack01");
    private int pickupID = Animator.StringToHash("Base Layer.c_pickup");
    private AnimatorStateInfo Bs;
    private bool itemInRange;

    private AudioSource _audioSource;
    public AudioClip attackVoiceClip;
    public AudioClip pickupSoundClip;

    

    //MOVEMENT
    private float Speed = 30.0f;
    private float RotationSpeed = 240.0f;
    private float Gravity = 20.0f;
    private Vector3 _moveDir = Vector3.zero;
    private bool actionSwitch;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        inventoryInput = FindObjectOfType<InventoryInput>();
    }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        actionSwitch = false;
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

        if(Bs.fullPathHash == attack01ID || Bs.fullPathHash == pickupID)
        {
            _anim.SetBool("IsAttack" , false);
            _anim.SetBool("IsPickup", false);
        }

        //Pickup
        if(Input.GetKeyDown(KeyCode.V))
        {
            if(itemInRange)
            _anim.SetBool("IsPickup", true);
        }

    }

    public void AttackMonster() //攻擊模式 event
    {
        _audioSource.PlayOneShot(attackVoiceClip , 0.5f);


    }
    public void AttackOverPositsion() //攻擊結束時位移 event
    {
        _moveDir = Vector3.forward * 10;
        _moveDir = transform.TransformDirection(_moveDir);
        _moveDir.y -= Gravity * Time.deltaTime;
        _characterController.Move(_moveDir * Time.deltaTime);
    }
    public void actionStart() //event
    {
        actionSwitch = true;
    }
    public void actionEnd() //event
    {
        actionSwitch = false;
    }
    public void PickupItem()
    {
        _audioSource.PlayOneShot(pickupSoundClip);
        FindObjectOfType<ItemSetting>().PickUpItem();
        inventoryInput.CloseMessagePanel();
        itemInRange = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            itemInRange = true;
            inventoryInput.OpenMessagePanel("");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Item")
        {
            itemInRange = false;
            inventoryInput.CloseMessagePanel();
        }
    }
}
