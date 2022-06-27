using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemList", menuName = "SaveFiles/Lists/ItemList")]
public class ItemList : ScriptableObject
{
    [SerializeField] List<ItemSave> _itemSaves = new List<ItemSave>();
    [SerializeField] private Material[] _materialsArmor;
    [SerializeField] private Material[] _materialsBow;
    public Material Materials(int type, int index)
    {
        if (type != 2)
            return _materialsArmor[index];
        else
            return _materialsBow[index];
    }

    public List<ItemSave> _ITEMSAVES { get => _itemSaves; }
    int _itemCount = 0;

    public int GetItemCount()
    {
        HasItem();
        return _itemCount;
    }
    public void HasItem()
    {
        _itemCount = 0;

        for (int index = 0; index < _itemSaves.Count; index++)
        {
            if (_itemSaves[index]._HASITEM)
            {
                _itemCount++;
            }
        }
    }

    public void GetItem()
    {
        List<ItemSave> items = new List<ItemSave>();
        int itemTier = Random.Range(0, 100);
        if (itemTier < 80) itemTier = 1;
        else if (itemTier < 92) itemTier = 2;
        else if (itemTier < 96) itemTier = 3;
        else if (itemTier < 99) itemTier = 4;
        else itemTier = 5;

        foreach (ItemSave item in _ITEMSAVES)
        {
            if ((int)item._ITEMTIER == itemTier)
            {
                if (!item._HASITEM)
                {
                    items.Add(item);

                }
            }
        }

        if (items.Count <= 0)
        {
            GetItem();
            return;
        }

        int index = Random.Range(0, items.Count);
        items[index]._HASITEM = true;

        ShopManager.instance.Purchase(items[index]);
        return;
    }

    public bool AbleItem()
    {
        if (_ITEMSAVES.Count > _itemCount) return true;

        return false;
    }
}
