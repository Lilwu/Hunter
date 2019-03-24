using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotkeySlot : ItemSlot
{
    public KeyCode _key;
    public Button _button;

    //冷卻系統
    public Image coldtimeImage;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public override bool CanReceiveItem(Item item)
    {
        if (item == null)
            return true;

        RestorableItem restorableItem = item as RestorableItem;
        MagicalcardItem magicalcard = item as MagicalcardItem;
        return restorableItem || magicalcard != null;
    }


}
