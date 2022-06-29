using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SMonoBehaviour<GameManager>
{

    [SerializeField] PlayerSave _playerSave;

    [SerializeField] private int _maxHp = 10;
    [SerializeField] private int _attack = 1;
    [SerializeField] private int _defense = 0;
    [SerializeField] private int _speed = 5;

    public int MAXHP { get => _maxHp; }
    public int ATTACK { get => _attack; }
    public int DEFENSE { get => _defense; }
    public int SPEED { get => _speed; }

    public PlayerSave _PLAYERSAVE { get => _playerSave; }

    private int _stageNumber;
    public int STAGENUMBER
    {
        get => _stageNumber;
        set => _stageNumber = value;
    }

    [SerializeField] private Material[] _materialsArmor;
    [SerializeField] private Material[] _materialsBow;
    public Material Materials(int type, int index)
    {
        if (type != 2)
            return _materialsArmor[index];
        else
            return _materialsBow[index];
    }

    private string SAVE_PATH = "";
    private readonly string SAVE_FILENAME = "/SaveFile.txt";

    protected override void Awake()
    {
        base.Awake();

        SAVE_PATH = Application.dataPath + "/Save";
        STAGENUMBER = 0;

        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);


        }

        LoadFronJson();
        InvokeRepeating("SaveToJson", 1f, 60f);
    }

    private void Start()
    {
        SetResolution();
    }

    void SetResolution()
    {
        int setWidth = 1440;
        int setHeight = 2960;

        Screen.SetResolution(setWidth / 3, setHeight / 3, true);
    }
    private void LoadFronJson()
    {
        string json = "";
        if (File.Exists(SAVE_PATH + SAVE_FILENAME))
        {
            json = File.ReadAllText(SAVE_PATH + SAVE_FILENAME);
            _playerSave = JsonUtility.FromJson<PlayerSave>(json);
        }
        else
        {


            SaveToJson();
            LoadFronJson();
        }
    }

    public void SaveToJson()
    {
        SAVE_PATH = Application.dataPath + "/Save";
        if (_playerSave == null) return;

        string json = JsonUtility.ToJson(_playerSave, true);
        File.WriteAllText(SAVE_PATH + SAVE_FILENAME, json, System.Text.Encoding.UTF8);
    }

    public void ItemEffect()
    {
        _playerSave._MAXHP = _maxHp;
        _playerSave._ATTACK = _attack;
        _playerSave._DEFENSE = _defense;
        _playerSave._SPEED = _speed;


        foreach (ItemSave itemSave in _playerSave._itemList._itemSaves)
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

    private void OnApplicationQuit()
    {
        SaveToJson();
    }
}
