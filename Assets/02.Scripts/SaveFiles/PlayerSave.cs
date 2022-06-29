using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSave 
{
    [SerializeField] private long _money;

    [SerializeField] private float _hp;
    [SerializeField] private float _maxHp = 30f;

    [SerializeField] private float _attack;
    [SerializeField] private float _defense = 0;
    [SerializeField] private float _speed = 5;

    public ItemList _itemList;

    public float _bgmVolume;
    public float _effectVolume;

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

    public float _MAXHP
    {
        get => _maxHp; set => _maxHp = value;
    }
    public float _ATTACK
    {
        get => _attack; set => _attack = value;
    }
    public float _DEFENSE
    {
        get => _defense; set => _defense = value;
    }
    public float _SPEED
    {
        get => _speed; set => _speed = value;
    }


}
