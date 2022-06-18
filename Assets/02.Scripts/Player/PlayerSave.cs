using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSave", menuName = "SaveFiles/Player")]

public class PlayerSave : ScriptableObject
{

    [SerializeField] private long _money;

    [SerializeField] private float _hp;
    [SerializeField] private float _maxHp = 30f;
    [SerializeField] private float _attack;
    [SerializeField] private float _defense = 0;
    [SerializeField] private bool[] _isItemEquip; // 아이템 장착 여부
    //HELMET, WEAPON, CHEST, PANTS, SHOULDER, BOOTS
    [SerializeField] private string[] _itemName; //장착한 아이템 이름

    public long _MONEY
    {
        get => _money;
        set => _money = value;
    }

    public float _HP
    {
        get => _hp;
        set => _hp = value;
    }
    public float _MAXHP { get => _maxHp; }
    public float _ATTACK { get => _attack; set => _attack = value; }
    public float _DEFENSE { get => _defense; set => _defense = value; }

    public void SetItemEquip(int index, bool isItemEquip)
    {
        if (index < 0 || index > _isItemEquip.Length) return;

        _isItemEquip[index] = isItemEquip;
    }
    public bool GetItemEquip(int index)
    {
        if (index < 0 || index > _isItemEquip.Length)
        {
            return false;
        }

        return _isItemEquip[index];
    }

    public void SetItemName(int index, string itemName)
    {
        if (index < 0 || index > _isItemEquip.Length) return;

        _itemName[index] = itemName;
    }
    public string GetItemName(int index)
    {
        if (index < 0 || index > _isItemEquip.Length)
        {
            return string.Empty;
        }

        return _itemName[index];
    }

    
}
