using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    public Text amonutText;
    private int money;
    private Player player;
    private PlayerController playerController;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        playerController = FindObjectOfType<PlayerController>();
    }

    public void OnPickUp()
    {
        Destroy(gameObject);
    }

    public void SetMoney(int min, int max)
    {
        money = Random.Range(min, max);
        amonutText.text = "金幣(" + (money.ToString()) + ")";
    }

    public void AddPlayerMoney()
    {
        Debug.Log("你獲得了金幣:" + money);
        player.AddMoney(money);
        gameObject.SetActive(false);
        amonutText.text = "金幣(" + (money.ToString()) + ")";
    }
}
