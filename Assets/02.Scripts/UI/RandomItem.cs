using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomItem : MonoBehaviour
{
    [SerializeField] Button _purchaseBtn;
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _costText;
    [SerializeField] private int _cost;
    [SerializeField] private Text _text;
    [SerializeField] private GameObject _panel;

    private void Start()
    {
        _purchaseBtn.onClick.AddListener(() => ItemPurchase());
        _nameText.text = "��� �̱�";
        GetCost();
        _costText.text = $"{_cost}��";
        _panel.SetActive(false);
        _panel.GetComponent<Button>().onClick.AddListener(() => OffPanel());
    }

    void GetCost()
    {
        int itemCount = GameManager.Instance._PLAYERSAVE._itemList.GetItemCount() + 1;
        _cost = (int)(5 + (itemCount - 1) * 10);
    }

    void ItemPurchase()
    {
        if (GameManager.Instance._PLAYERSAVE._MONEY < _cost)
        {
            _panel.SetActive(true);
            _text.text = "���� �����մϴ�.";
            return;
        }
        GameManager.Instance._PLAYERSAVE._itemList.HasItem();
        if (!GameManager.Instance._PLAYERSAVE._itemList.AbleItem())
        {
            _panel.SetActive(true);
            _text.text = "������ �������� �������� �ʽ��ϴ�.";
            return;
        }
        GameManager.Instance._PLAYERSAVE._MONEY -= _cost;

        GameManager.Instance._PLAYERSAVE._itemList.GetItem();

        InventoryManager.instance.Refresh();

        GetCost();
        _costText.text = $"{_cost}��";
    }

    public void OffPanel()
    {
        _panel.SetActive(false);
    }
}
