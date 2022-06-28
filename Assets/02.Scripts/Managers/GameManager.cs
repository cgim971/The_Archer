using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : SMonoBehaviour<GameManager>
{

    [SerializeField] PlayerSave _playerSave;
    [SerializeField] ItemList _itemList;

    [SerializeField] private int _maxHp = 10;
    [SerializeField] private int _attack = 1;
    [SerializeField] private int _defense = 0;
    [SerializeField] private int _speed = 5;

    public int MAXHP { get => _maxHp; }
    public int ATTACK { get => _attack; }
    public int DEFENSE { get => _defense; }
    public int SPEED { get => _speed; }

    public PlayerSave _PLAYERSAVE { get => _playerSave; }
    public ItemList _ITEMLIST { get => _itemList; }

    private int _stageNumber;
    public int STAGENUMBER
    {
        get => _stageNumber;
        set => _stageNumber = value;
    }

    protected override void Awake()
    {
        base.Awake();

        _playerSave = Resources.Load<PlayerSave>("Saves/Player/PlayerSave");
        _itemList = Resources.Load<ItemList>("Saves/Lists/ItemList");

        STAGENUMBER = 0;
    }

    public void ItemEffect()
    {
        _playerSave._MAXHP = _maxHp;
        _playerSave._ATTACK = _attack;
        _playerSave._DEFENSE = _defense;
        _playerSave._SPEED = _speed;


        foreach (ItemSave itemSave in _itemList._ITEMSAVES)
        {
            if (!itemSave._EQUIPMENTITEM) continue;

            switch (itemSave._ITEMTYPE)
            {
                case ItemSave.ItemType.HELMET:
                    _playerSave._DEFENSE += itemSave._EFFECT;
                    break;
                case ItemSave.ItemType.WEAPON:
                    _playerSave._ATTACK += itemSave._EFFECT;
                    break;
                case ItemSave.ItemType.CHEST:
                    _playerSave._MAXHP += itemSave._EFFECT;
                    break;
                case ItemSave.ItemType.PANTS:
                    _playerSave._MAXHP += itemSave._EFFECT;
                    break;
                case ItemSave.ItemType.SHOULDER:
                    _playerSave._DEFENSE += itemSave._EFFECT;
                    break;
                case ItemSave.ItemType.BOOTS:
                    _playerSave._SPEED += itemSave._EFFECT;
                    break;
            }
        }
    }

    //void SetHasItemCount(int index)
    //{
    //    int count = 0;
    //    for (int i = 0; i < _itemList._ITEMTYPESAVES[index]._ITEMSAVES.Count; i++)
    //    {
    //        if (_itemList._ITEMTYPESAVES[index]._ITEMSAVES[i]._HASITEM)
    //        {
    //            count++;
    //        }
    //    }

    //    _itemList._ITEMTYPESAVES[index]._HASITEMCOUNT = count;
    //    return;
    //}


    //public void SetDefense(ItemTypeList list)
    //{
    //    float index = 1;

    //    foreach (ItemSave item in list._ITEMSAVES)
    //    {
    //        if (item._HASITEM)
    //            index += item._EFFECT;
    //    }

    //    _PLAYERSAVE._DEFENSE = index;
    //}

    //public void SetDefense()
    //{
    //    float index = 1;

    //    foreach (ItemTypeList list in _itemList._ITEMTYPESAVES)
    //    {
    //        foreach (ItemSave item in list._ITEMSAVES)
    //        {
    //            if (item._HASITEM)
    //                index += item._EFFECT;
    //        }
    //    }

    //    _PLAYERSAVE._DEFENSE = index;
    //}

    //public void SetAttack(ItemTypeList list)
    //{
    //    float index = 2;

    //    foreach (ItemSave item in list._ITEMSAVES)
    //    {
    //        if (item._HASITEM)
    //            index += item._EFFECT;
    //    }

    //    _PLAYERSAVE._ATTACK = index;
    //}
    //public void SetAttack()
    //{
    //    float index = 2;

    //    foreach (ItemSave item in _itemList._ITEMTYPESAVES[1]._ITEMSAVES)
    //    {
    //        if (item._HASITEM)
    //            index += item._EFFECT;
    //    }

    //    _PLAYERSAVE._ATTACK = index;
    //}
}
