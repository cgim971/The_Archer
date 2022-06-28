using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [SerializeField] private Transform _player;
    [SerializeField] private Transform[] _parts;
    [SerializeField] private Renderer[] _skinParts;
    [SerializeField] private Transform _inventory;

    [SerializeField] private GameObject _slots;
    [SerializeField] private List<GameObject> _slotList;

    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private Image _panel;
    [SerializeField] private Text _titleText;
    [SerializeField] private Text _contentText;
    [SerializeField] private Image _itemImage;
    [SerializeField] private GameObject _btns;
    [SerializeField] private Button _useBtn;
    [SerializeField] private Button _unquipBtn;
    [SerializeField] private float _delay = 0.4f;

    [SerializeField] private ItemList _itemList;

    [SerializeField] ItemSave _itemSave;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private Text _atkText;
    [SerializeField] private Text _defText;
    [SerializeField] private Text _hpText;
    [SerializeField] private Text _spdText;

    private void Start()
    {
        _itemList = GameManager.Instance._ITEMLIST;


        for (int index = 0; index < 30; index++)
        {
            GameObject newSlot = Instantiate(_slots, _inventory);
            newSlot.GetComponent<SlotManager>().INVENTORY = this;
            newSlot.GetComponent<SlotManager>().SetItemSave(GetItemSave());
            _slotList.Add(newSlot);
        }

        _btns.transform.Find("useBtn").GetComponent<Button>().onClick.AddListener(() => Use());
        _btns.transform.Find("unquipBtn").GetComponent<Button>().onClick.AddListener(() => Unquip());
        _btns.transform.Find("cancelBtn").GetComponent<Button>().onClick.AddListener(() => Cancel());

        EquipmentItem();
        GameManager.Instance.ItemEffect();
        OffUI();
    }

    public void Refresh()
    {
        count = 0;
        for (int i = 0; i < 30; i++)
        {
            _slotList[i].GetComponent<SlotManager>().SetItemSave(GetItemSave());
        }
    }

    void EquipmentItem()
    {
        foreach (ItemSave itemSave in _itemList._ITEMSAVES)
        {
            if (itemSave._EQUIPMENTITEM)
            {
                _skinParts[(int)itemSave._ITEMTYPE - 1].gameObject.SetActive(true);

                _parts[(int)itemSave._ITEMTYPE - 1].GetComponent<EquipmentSlot>().SetItem(itemSave);

                _skinParts[(int)itemSave._ITEMTYPE - 1].GetComponent<Renderer>().material = _itemList.Materials((int)itemSave._ITEMTYPE, (int)itemSave._ITEMTIER - 1);
            }
        }
    }

    int count = 0;
    public ItemSave GetItemSave()
    {
        for (int i = count; i < _itemList._ITEMSAVES.Count; i++, count++)
        {
            if (_itemList._ITEMSAVES[i]._HASITEM)
            {
                count++;
                return _itemList._ITEMSAVES[i];
            }
        }
        return null;
    }

    public void OnUI()
    {
        _menuPanel.SetActive(true);
        _btns.SetActive(true);

        _player.gameObject.SetActive(false);
        Sequence seq = DOTween.Sequence();
        seq.Append(_panel.DOFade(1, _delay));
        seq.Join(_itemImage.DOFade(1, _delay));
        seq.Join(_menuPanel.GetComponent<Image>().DOFade(1, _delay));

        seq.Play();

        _titleText.text = "title";
        _contentText.text = "content";

        UpdateStats();
    }


    [SerializeField] private Text _atkStatText;
    [SerializeField] private Text _defStatText;
    [SerializeField] private Text _spdStatText;
    [SerializeField] private Text _hpStatText;

    public void OnUI(ItemSave itemSave)
    {
        if (itemSave == null) return;

        _itemSave = itemSave;

        _menuPanel.SetActive(true);
        _btns.SetActive(true);

        if (itemSave._EQUIPMENTITEM)
        {
            _unquipBtn.gameObject.SetActive(true);
        }
        else
        {
            _atkStatText.text = $"ATK {GameManager.Instance._PLAYERSAVE._ATTACK} ¡æ {GetItem(0) + itemSave._EFFECT}";
            _defStatText.text = $"DEF {GameManager.Instance._PLAYERSAVE._DEFENSE} ¡æ {GetItem(1) + itemSave._EFFECT}";
            _spdStatText.text = $"SPD {GameManager.Instance._PLAYERSAVE._SPEED} ¡æ {GetItem(2) + itemSave._EFFECT}";
            _hpStatText.text = $"HP {GameManager.Instance._PLAYERSAVE._HP} ¡æ {GetItem(3) + itemSave._EFFECT}";

            _useBtn.gameObject.SetActive(true);
        }

        _player.gameObject.SetActive(false);
        Sequence seq = DOTween.Sequence();
        seq.Append(_panel.DOFade(1, _delay));
        seq.Join(_itemImage.DOFade(1, _delay));
        seq.Join(_menuPanel.GetComponent<Image>().DOFade(1, _delay));

        seq.Play();

        _titleText.text = itemSave._ITEMNAME;
        _contentText.text = itemSave._ITEMINFORM;
        _itemImage.sprite = itemSave._ITEMSPRITE;

        

        UpdateStats();
    }

    public int GetItem(int index)
    {
        float returnIndex = 0;
        switch (index)
        {
            case 0:
                foreach (ItemSave itemSave in _itemList._ITEMSAVES)
                {
                    if (itemSave._ITEMTYPE == _itemSave._ITEMTYPE) continue;
                    if (!itemSave._EQUIPMENTITEM) continue;
                    if (itemSave._ITEMTYPE == ItemSave.ItemType.WEAPON)
                    {
                        returnIndex += itemSave._EFFECT;
                    }
                }
                return (int)returnIndex;
            case 1:
                foreach (ItemSave itemSave in _itemList._ITEMSAVES)
                {
                    if (itemSave._ITEMTYPE == _itemSave._ITEMTYPE) continue;
                    if (!itemSave._EQUIPMENTITEM) continue;
                    if (itemSave._ITEMTYPE == ItemSave.ItemType.HELMET || itemSave._ITEMTYPE == ItemSave.ItemType.SHOULDER)
                    {
                        returnIndex += itemSave._EFFECT;
                    }
                }
                return (int)returnIndex;

            case 2:
                foreach (ItemSave itemSave in _itemList._ITEMSAVES)
                {
                    if (itemSave._ITEMTYPE == _itemSave._ITEMTYPE) continue;
                    if (!itemSave._EQUIPMENTITEM) continue;
                    if (itemSave._ITEMTYPE == ItemSave.ItemType.BOOTS)
                    {
                        returnIndex += itemSave._EFFECT;
                    }
                }
                return (int)returnIndex;

            case 3:
                foreach (ItemSave itemSave in _itemList._ITEMSAVES)
                {
                    if (itemSave._ITEMTYPE == _itemSave._ITEMTYPE) continue;
                    if (!itemSave._EQUIPMENTITEM) continue;
                    if (itemSave._ITEMTYPE == ItemSave.ItemType.CHEST || itemSave._ITEMTYPE == ItemSave.ItemType.PANTS)
                    {
                        returnIndex += itemSave._EFFECT;
                    }
                }
                return (int)returnIndex;

        }

        return 0;
    }

    public void OffUI()
    {
        _btns.SetActive(false);
        _unquipBtn.gameObject.SetActive(false);
        _useBtn.gameObject.SetActive(false);

        Sequence seq = DOTween.Sequence();
        seq.Append(_panel.DOFade(0, _delay));
        seq.Join(_itemImage.DOFade(0, _delay));
        seq.Join(_menuPanel.GetComponent<Image>().DOFade(0, _delay));

        seq.Play();

        _titleText.text = string.Empty;
        _contentText.text = string.Empty;

        StartCoroutine(SetActiveObject(_menuPanel, false));
        StartCoroutine(SetActiveObject(_player.gameObject, true));

        UpdateStats();
    }

    IEnumerator SetActiveObject(GameObject obj, bool isSetActive)
    {
        yield return new WaitForSeconds(_delay);
        obj.SetActive(isSetActive);
    }

    public void Use()
    {
        EquipmentSlot equipmentSlot = _parts[(int)_itemSave._ITEMTYPE - 1].GetComponent<EquipmentSlot>();

        foreach (ItemSave itemSave in _itemList._ITEMSAVES)
        {
            if (_itemSave._ITEMTYPE == itemSave._ITEMTYPE)
            {
                Unquip(itemSave);
            }
        }

        equipmentSlot.SetItem(_itemSave);

        GameManager.Instance.ItemEffect();
        OffUI();

        _skinParts[(int)_itemSave._ITEMTYPE - 1].gameObject.SetActive(true);
        if (equipmentSlot.ITEMSAVE != null)
        {
            equipmentSlot.ITEMSAVE._EQUIPMENTITEM = true;
            _skinParts[(int)_itemSave._ITEMTYPE - 1].GetComponent<Renderer>().material = _itemList.Materials((int)_itemSave._ITEMTYPE, (int)_itemSave._ITEMTIER - 1);
        }
    }

    public void UpdateStats()
    {
        _atkText.text = $"ATK {GameManager.Instance._PLAYERSAVE._ATTACK}";
        _defText.text = $"DEF {GameManager.Instance._PLAYERSAVE._DEFENSE}";
        _spdText.text = $"SPD {GameManager.Instance._PLAYERSAVE._SPEED}";
        _hpText.text = $"HP {GameManager.Instance._PLAYERSAVE._MAXHP}";
    }

    public void Unquip()
    {
        _skinParts[(int)_itemSave._ITEMTYPE - 1].gameObject.SetActive(false);
        _parts[(int)_itemSave._ITEMTYPE - 1].GetComponent<EquipmentSlot>().SetItem(null);
        _itemSave._EQUIPMENTITEM = false;

        GameManager.Instance.ItemEffect();
        OffUI();
    }

    public void Unquip(ItemSave itemSave)
    {
        _skinParts[(int)itemSave._ITEMTYPE - 1].gameObject.SetActive(false);
        _parts[(int)itemSave._ITEMTYPE - 1].GetComponent<EquipmentSlot>().SetItem(null);
        itemSave._EQUIPMENTITEM = false;

        GameManager.Instance.ItemEffect();
        UpdateStats();
    }

    public void Cancel()
    {
        OffUI();
    }

}
