using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] Image _itemImage;
    ItemSave _itemSave;
    public ItemSave ITEMSAVE
    {
        get => _itemSave;
    }
    public void SetItem(ItemSave itemSave)
    {
        _itemSave = itemSave;
        if (itemSave != null)
        {
            _itemImage.sprite = itemSave._ITEMSPRITE;
            _itemSave._EQUIPMENTITEM = true;
        }
    }
}
