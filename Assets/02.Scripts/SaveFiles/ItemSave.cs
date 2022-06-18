using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSave", menuName = "SaveFiles/Items")]
public class ItemSave : ScriptableObject
{
    [SerializeField] private string _itemName;
    [SerializeField] ItemType _itemType;
    [SerializeField] private float _effect;
    [SerializeField] private bool _hasItem; // �������� ������ �ִ���
    public enum ItemType { NONE, HELMET, WEAPON, CHEST, PANTS, SHOULDER, BOOTS }
    public string _ITEMNAME { get => _itemName; }
    public ItemType _ITEMTYPE { get => _itemType; }
    public float _EFFECT { get => _effect; }
    public bool _HASITEM
    {
        get => _hasItem;
        set => _hasItem = value;
    }
}
