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
    [SerializeField] private bool _equipmentItem;
    [SerializeField] private Sprite _itemSprite;
    [SerializeField] private float _effect;

    public enum ItemTier { NONE = 0, BASE = 1, RARE = 2, EPIC = 3, LUXURY = 4, LEGEND = 5 }
    public enum ItemType { NONE = 0, HELMET = 1, WEAPON = 2, CHEST = 3, PANTS = 4, SHOULDER = 5, BOOTS = 6 }

    public int _ITEMNUMBER
    {
        get => _itemNumber;
    }
    public string _ITEMNAME { get => _itemName; }
    public string _ITEMINFORM { get => _itemInform; }
    public ItemTier _ITEMTIER { get => _itemTier; }
    public ItemType _ITEMTYPE { get => _itemType; }
    public bool _HASITEM
    {
        get => _hasItem;
        set
        {
            _hasItem = value;
        }
    }
    public bool _EQUIPMENTITEM
    {
        get => _equipmentItem;
        set
        {
            _equipmentItem = value;
        }
    }
    public Sprite _ITEMSPRITE { get => _itemSprite; }
    public float _EFFECT { get => _effect; }
}
