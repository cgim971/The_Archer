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
        _nameText.text = "장비 뽑기";
        _costText.text = $"{_cost}원";
    }

    void ItemPurchase()
    {
        if (GameManager.Instance._PLAYERSAVE._MONEY < _cost) return;
        GameManager.Instance._ITEMLIST.HasItem();
        if (!GameManager.Instance._ITEMLIST.AbleItem()) return;

        GameManager.Instance._PLAYERSAVE._MONEY -= _cost;

        GameManager.Instance._ITEMLIST.GetItem();
    }
}
