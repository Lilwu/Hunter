using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //角色屬性
    static int _maxHp;
    static int _hp;
    static int _maxMp;
    static int _mp;
    static int _money;
    static int _attack;
    static int _lv;
    static int _exp;

    public GameObject playerUI;
    public Text moneyText;
    static Player instance;
    static GameObject instanceUI;

    //LevelUp 20190410
    public Text levelText;
    public ParticleSystem levelupEffect;
    public ParticleSystem playerLeveupFx;
    private InventoryInput inventoryInput;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);

            instanceUI = playerUI;
            DontDestroyOnLoad(playerUI);

            _maxHp = 500;
            _hp = _maxHp;

            _maxMp = 300;
            _mp = _maxMp;
            _money = 300;
            _attack = 20;

            _lv = 1;
            _exp = 0;
        }
        else if (this != instance)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            print("刪除" + sceneName + "的" + name);
            Destroy(gameObject);
            Destroy(playerUI);
        }
    }

    private void Start()
    {
        moneyText.text = _money.ToString();
        levelText.text = _lv.ToString();
        inventoryInput = FindObjectOfType<InventoryInput>();
    }

    public void AddMoney(int money)
    {
        _money += money;
        moneyText.text = _money.ToString();
    }

    public void LessMoney(int money)
    {
        _money -= money;
        moneyText.text = _money.ToString();
    }

    public void AddWeaponAttack(int attack)
    {
        _attack += attack;
        print("目前攻擊力為：" + _attack);
    }

    public void RemoveWeaponAttack()
    {
        _attack = 20;
    }

    public void AddPlayerExp(int exp)
    {
        _exp += exp;
        print("目前經驗值為：" + _exp);
        CheckLevelIsUp();
    }

    public void CheckLevelIsUp()
    {
        LevelUP();
        levelText.text = _lv.ToString();
    }

    //讀取各屬性
    public int HP
    {
        get { return _hp; }
    }
    public int MaxHP
    {
        get { return _maxHp; }
    }
    public int MP
    {
        get { return _mp; }
    }
    public int MaxMP
    {
        get { return _maxMp; }
    }
    public int MONEY
    {
        get { return _money;}
    }
    public int ATTACK
    {
        get { return _attack; }
    }
    public int LV
    {
        get { return _lv; }
    }
    public int EXP
    {
        get { return _exp; }
    }

    private void LevelUP()
    {

        if (_exp >= 30 && _exp < 80 && _lv != 2)
        {
            _lv = 2;
            LevelUpEffect();
            HpMpUp(50);
        }
        else if (_exp >= 80 && _exp < 150 && _lv != 3)
        {
            _lv = 3;
            LevelUpEffect();
            HpMpUp(60);
        }
        else if (_exp >= 150 && _exp < 250 && _lv != 4)
        {
            _lv = 4;
            LevelUpEffect();
            HpMpUp(70);
        }
        else if (_lv >= 250 && _lv != 5)
        {
            _lv = 5;
            LevelUpEffect();
            HpMpUp(80);
        }
    }

    private void LevelUpEffect()
    {
        playerLeveupFx.Play();
        levelupEffect.Play();
        inventoryInput.GetComponent<AudioSource>().PlayOneShot(inventoryInput.awardClip);
        FindObjectOfType<StatePanel>().SetSateText("<color=green>" + "恭喜！等級提升為： " + "</color>" + "<color=green>" + _lv + "</color>"); //顯示StatePanel
    }

    private void HpMpUp(int value)
    {
        _maxHp += value;
        _maxMp += value;
        GetComponent<PlayerHealth>().ResetHealth();
        GetComponent<PlayerMagic>().ResetMagic();
    }
}