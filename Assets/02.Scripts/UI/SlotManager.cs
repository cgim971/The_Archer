using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{

    ItemSave _itemSave;
    Button _useBtn;
    [SerializeField] Image _image;

    [SerializeField] private InventoryManager _inventory;
    public InventoryManager INVENTORY { set => _inventory = value; }

    public void SetItemSave(ItemSave itemSave)
    {
        _itemSave = itemSave;
        if (itemSave != null)
            _image.sprite = itemSave._ITEMSPRITE;
    }


    public void Start()
    {
        _useBtn = GetComponentInChildren<Button>();
        _useBtn.onClick.AddListener(() => Inform());
    }

    public void Inform()
    {
        _inventory.OnUI(_itemSave);
    }

}
