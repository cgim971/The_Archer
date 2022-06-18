using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemTypeList", menuName = "SaveFiles/Lists/ItemTypeList")]
public class ItemTypeList : ScriptableObject
{
    [SerializeField] List<ItemSave> _itemSave = new List<ItemSave>();
    public List<ItemSave> _ITEMSAVES { get { return _itemSave; } }
    [SerializeField] private int _hasItemCount;
    public int _HASITEMCOUNT
    {
        get => _hasItemCount;
        set => _hasItemCount = value;
    }
}
