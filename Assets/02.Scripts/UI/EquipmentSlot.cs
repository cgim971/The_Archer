using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] Image _itemImage;
    [SerializeField] Button _selectBtn;
    ItemSave _itemSave;

    private void Start()
    {
        _selectBtn.onClick.AddListener(() =>
        {
            InventoryManager.instance.OnUI(ITEMSAVE);
        });
    }

    public ItemSave ITEMSAVE
    {
        get => _itemSave;
    }
    public void SetItem(ItemSave itemSave)
    {
        _itemSave = itemSave;
        if (itemSave != null)
        {
            _itemImage.sprite = GameManager.Instance._PLAYERSAVE._itemList._itemSaves[_itemSave._ITEMNUMBER]._itemSprite;
            _itemSave._EQUIPMENTITEM = true;
        }
        else
        {
            _itemImage.sprite = null;
        }
    }
}
