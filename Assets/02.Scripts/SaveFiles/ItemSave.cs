using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "item_", menuName = "SaveFiles/Items")]
public class ItemSave : ScriptableObject
{
    [SerializeField] private int _itemNumber;
    // ������ �̸�
    [SerializeField] private string _itemName;
    // ������ ����
    [SerializeField] private string _itemInform;
    // ������ ���
    [SerializeField] private ItemTier _itemTier;
    // ������ Ÿ��
    [SerializeField] ItemType _itemType;
    // �������� ������ �ִ���
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
