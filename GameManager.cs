using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public Transform bornPoint;
    public AudioClip clickClip;
    private AudioSource audioSource;

    //商品金額顯示 20190417
    public MoneyTooltip moneyTooltip;
    public ShopPaneal[] shopPaneals;

    private void Awake()
    {
        FindObjectOfType<Player>().gameObject.transform.localPosition = transform.position;
        audioSource = GetComponent<AudioSource>();

        //商品價錢顯示 20190417
        //Pointer Enter
        if (shopPaneals != null)
        {
            for (int i = 0; i < shopPaneals.Length; i++)
            {
                shopPaneals[i].OnPointerEnterEvent += ShowMoneyTooltip; //商品顯示價錢Tooltip
            }
            //Pointer Exit
            for (int i = 0; i < shopPaneals.Length; i++)
            {
                shopPaneals[i].OnPointerExitEvent += HideMoneyTooltip; //商品隱藏價錢Tooltip
            }
        }
    }

    private void Start()
    {
        GameObject.Find("light_FX").GetComponent<ParticleSystem>().Play();
        Invoke("CloseScenePanel", 3.0f);
        FindObjectOfType<CameraFollow>().ResetHUDFollow();
        FindObjectOfType<Minimap>().ResetIconFollow();
    }

    private void OnValidate()
    {
        if (moneyTooltip == null)
            moneyTooltip = FindObjectOfType<MoneyTooltip>();
    }

    public void ClickSound()
    {
        audioSource.PlayOneShot(clickClip);
    }

    public void CloseGameObject(GameObject gameObject)
    {
        ClickSound();
        gameObject.SetActive(false);
    }

    private void CloseScenePanel()
    {
        GameObject.Find("ScenePanel").SetActive(false);
    }

    //商品顯示價錢Tooltip 20190417
    private void ShowMoneyTooltip(ShopSlot shopSlot)
    {
        //裝備顯示
        EquippableItem equippableItem = shopSlot.item as EquippableItem;
        if (equippableItem != null)
        {
            moneyTooltip.transform.position = Input.mousePosition;
            moneyTooltip.ShowTooltip(equippableItem);
        }

        //補助道具顯示
        RestorableItem restorableItem = shopSlot.item as RestorableItem;
        if (restorableItem != null)
        {
            moneyTooltip.transform.position = Input.mousePosition;
            moneyTooltip.ShowTooltip(restorableItem);
        }

    }
    private void HideMoneyTooltip(ShopSlot shopSlot)
    {
        moneyTooltip.HideTooltip();
    }
}
