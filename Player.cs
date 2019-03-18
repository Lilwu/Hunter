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
    static int _lv;
    static int _money;
    static int _attack;

    public Text moneyText;
    static Player instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);

            _maxHp = 999;
            _hp = _maxHp;

            _maxMp = 499;
            _mp = _maxMp;
            _money = 1;
            _attack = 20;

            _lv = 1;
        }
        else if (this != instance)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            print("刪除" + sceneName + "的" + name);
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        moneyText.text = _money.ToString();
    }

    public void AddMoney(int money)
    {
        _money += money;
        moneyText.text = _money.ToString();
    }

    public void AddWeaponAttack(int attack)
    {
        _attack += attack;
        print("目前攻擊力為：" + _attack);
    }

    //讀取HP及MP
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
    public int LV
    {
        get { return _lv; }
    }
    public int MONEY
    {
        get { return _money;}
    }

    public int ATTACK
    {
        get { return _attack; }
    }

}