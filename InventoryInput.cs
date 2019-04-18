using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryInput : MonoBehaviour
{
    [SerializeField] GameObject equipmentPanelGameObject;
    [SerializeField] GameObject inventoryPanelGameObject;
    [SerializeField] GameObject messagePanelGameObject;
    [SerializeField] GameObject npcMessagePanelGameObject;
    [SerializeField] GameObject minimenuPanelGameObject;
    [SerializeField] GameObject settingPanelGameObject;
    [SerializeField] ItemTooltip itemTooltip;
    [SerializeField] InventoryManager inventoryManager;
    [SerializeField] Camera minimap;
    [SerializeField] AudioSource musicAudio;
    [SerializeField] Slider musicAdjValue;

    //20190227 SetHotkey
    [SerializeField] Transform hotkeySlotParent;
    [SerializeField] HotkeySlot[] hotkeySlot;

    [SerializeField] KeyCode[] toggleEquipmentKeys;
    [SerializeField] KeyCode[] toggleInventoryKeys;
    [SerializeField] KeyCode[] toggleMinimenuKeys;
    [SerializeField] KeyCode[] toggleSettingKeys;

    public AudioClip clickClip;
    public AudioClip awardClip;
    private AudioSource audioSource;

    private void OnValidate()
    {
        hotkeySlot = hotkeySlotParent.GetComponentsInChildren<HotkeySlot>();
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        equipmentPanelGameObject.SetActive(false);
        inventoryPanelGameObject.SetActive(false);
        settingPanelGameObject.SetActive(false);
        minimap.orthographicSize = 20.0f;
    }

    void Update()
    {
        //背景音樂調整
        musicAudio.volume = musicAdjValue.value;

        for (int i = 0; i < toggleInventoryKeys.Length; i++)
        {
            if (Input.GetKeyDown(toggleInventoryKeys[i]))
            {
                itemTooltip.HideTooltip(); //不論開啟關閉物品欄，tooltip始終保持關閉，直到游標指定物品才顯示
                inventoryPanelGameObject.SetActive(!inventoryPanelGameObject.activeSelf);
                break;
            }
        }

        for (int i = 0; i < toggleEquipmentKeys.Length; i++)
        {
            if (Input.GetKeyDown(toggleEquipmentKeys[i]))
            {
                itemTooltip.HideTooltip(); //不論開啟關閉裝備欄，tooltip始終保持關閉，直到游標指定物品才顯示
                equipmentPanelGameObject.SetActive(!equipmentPanelGameObject.activeSelf);
                break;
            }
        }

        for (int i = 0; i < toggleMinimenuKeys.Length; i++)
        {
            if (Input.GetKeyDown(toggleMinimenuKeys[i]))
            {
                minimenuPanelGameObject.SetActive(!minimenuPanelGameObject.activeSelf);
                break;
            }
        }

        for (int i = 0; i < toggleSettingKeys.Length; i++)
        {
            if (Input.GetKeyDown(toggleSettingKeys[i]))
            {
                settingPanelGameObject.SetActive(!settingPanelGameObject.activeSelf);
                break;
            }
        }

        for (int i = 0; i < hotkeySlot.Length; i++)
        {
            if (Input.GetKeyDown(hotkeySlot[i]._key))
            {
                hotkeySlot[i]._button.onClick.Invoke();
            }
        }
    }

    public void CloseGameObject(GameObject gameObject)
    {
        CliclSound();
        gameObject.SetActive(false);
    }

    public void OpenMessagePanel(string text)
    {
        messagePanelGameObject.SetActive(true);
    }

    public void CloseMessagePanel()
    {
        messagePanelGameObject.SetActive(false);
    }

    public void OpenNpcMessagePanel(string text)
    {
        npcMessagePanelGameObject.SetActive(true);
    }

    public void CloseNpcMessagePanel()
    {
        npcMessagePanelGameObject.SetActive(false);
    }

    public void CliclSound()
    {
        audioSource.PlayOneShot(clickClip);
    }

    public void ToogleMinimanuPanel()
    {
        CliclSound();
        minimenuPanelGameObject.SetActive(!minimenuPanelGameObject.activeSelf);
    }

    public void OpenMinimanu(GameObject panel)
    {
        CliclSound();
        panel.SetActive(!panel.activeSelf);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetQuiality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    //小地圖
    public void PlusMinimap()
    {
        if (minimap.orthographicSize <= 20 && minimap.orthographicSize > 15)
        {
            CliclSound();
            minimap.orthographicSize--;
        }
    }
    public void MinusMinimap()
    {
        if (minimap.orthographicSize >= 20 && minimap.orthographicSize < 30)
        {
            CliclSound();
            minimap.orthographicSize++;
        }
    }
}