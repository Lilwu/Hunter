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

    //血條追隨攝影機  20190417
    public GameObject itemHUD;
    private Camera main_camera;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        playerController = FindObjectOfType<PlayerController>();
        main_camera = GameObject.Find("Main Camera").GetComponent<Camera>();
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
        FindObjectOfType<StatePanel>().SetSateText("您獲得了金幣:" + money); //顯示StatePanel

        player.AddMoney(money);
        gameObject.SetActive(false);
        amonutText.text = "金幣(" + (money.ToString()) + ")";
    }

    private void Update()
    {
        //HUD追隨攝影機
        if (itemHUD != null)
        {
            itemHUD.transform.LookAt(itemHUD.transform.position + main_camera.transform.rotation * Vector3.back,
                                        main_camera.transform.rotation * Vector3.up);
        }
    }
}
