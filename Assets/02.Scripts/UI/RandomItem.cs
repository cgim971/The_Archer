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

    private void Start()
    {
        _purchaseBtn.onClick.AddListener(() => ItemPurchase());
        _nameText.text = "��� �̱�";
        GetCost();
        _costText.text = $"{_cost}��";
    }

    void GetCost()
    {
        int itemCount = GameManager.Instance._ITEMLIST.GetItemCount() + 1;
        _cost = (int)(5 + (itemCount - 1) * 10);
    }

    void ItemPurchase()
    {
        if (GameManager.Instance._PLAYERSAVE._MONEY < _cost) return;
        GameManager.Instance._ITEMLIST.HasItem();
        if (!GameManager.Instance._ITEMLIST.AbleItem()) return;

        GameManager.Instance._PLAYERSAVE._MONEY -= _cost;

        GameManager.Instance._ITEMLIST.GetItem();

        InventoryManager.instance.Refresh();

        GetCost();
        _costText.text = $"{_cost}��";
    }
}
