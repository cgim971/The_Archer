using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "item_", menuName = "SaveFiles/Items")]
public class ItemSave : ScriptableObject
{
    [SerializeField] private int _itemNumber;
    // 아이템 이름
    [SerializeField] private string _itemName;
    // 아이템 정보
    [SerializeField] private string _itemInform;
    // 아이템 등급
    [SerializeField] private ItemTier _itemTier;
    // 아이템 타입
    [SerializeField] ItemType _itemType;
    // 아이템을 가지고 있는지
    [SerializeField] private bool _hasItem;
    public enum ItemTier { NONE = 0, BASE = 1, LUXURY = 2, RARE = 3, LEGEND = 4 }
    public enum ItemType { NONE, HELMET, WEAPON, CHEST, PANTS, SHOULDER, BOOTS }

    public int _ITEMNUMBER { get => _itemNumber; }
    public string _ITEMNAME { get => _itemName; }
    public string _ITEMINFORM { get => _itemInform; }
    public ItemTier _ITEMTIER { get => _itemTier; }
    public ItemType _ITEMTYPE { get => _itemType; }
    public bool _HASITEM
    {
        get => _hasItem;
        set => _hasItem = value;
    }
}
