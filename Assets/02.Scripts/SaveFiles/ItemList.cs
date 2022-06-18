using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemList", menuName = "SaveFiles/Lists/ItemList")]
public class ItemList : ScriptableObject
{
    [SerializeField] List<ItemTypeList> _itemTypeSaves = new List<ItemTypeList>();

    public List<ItemTypeList> _ITEMTYPESAVES { get { return _itemTypeSaves; } }
}
