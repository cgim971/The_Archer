using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SMonoBehaviour<GameManager>
{

    [SerializeField] PlayerSave _playerSave;
    [SerializeField] ItemList _itemList;

    protected override void Awake()
    {
        base.Awake();

        _playerSave = Resources.Load<PlayerSave>("Saves/Player/PlayerSave");
        _itemList = Resources.Load<ItemList>("Saves/Lists/ItemList");

        for (int index = 0; index < _itemList._ITEMTYPESAVES.Count; index++)
        {
            SetHasItemCount(index);
        }
    }

    void SetHasItemCount(int index)
    {
        int count = 0;
        for (int i = 0; i < _itemList._ITEMTYPESAVES[index]._ITEMSAVES.Count; i++)
        {
            if (_itemList._ITEMTYPESAVES[index]._ITEMSAVES[i]._HASITEM)
            {
                count++;
            }
        }

        _itemList._ITEMTYPESAVES[index]._HASITEMCOUNT = count;
        return;
    }

    public PlayerSave _PLAYERSAVE
    {
        get => _playerSave;
    }

    public ItemList _ITEMLIST
    {
        get => _itemList;
    }
}
