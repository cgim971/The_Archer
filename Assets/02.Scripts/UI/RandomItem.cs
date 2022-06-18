using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomItem : MonoBehaviour
{
    [SerializeField] Button _btn;

    [SerializeField] private long _cost;
    [SerializeField] private int _index;
    private ItemTypeList _list;

    private void Start()
    {
        if (_btn == null)
            _btn = GetComponentInChildren<Button>();

        _list = GameManager.Instance._ITEMLIST._ITEMTYPESAVES[_index];

        _btn.onClick.AddListener(() => Use());
    }

    void Use()
    {
        if (_list._HASITEMCOUNT >= _list._ITEMSAVES.Count) return;

        if (GameManager.Instance._PLAYERSAVE._MONEY >= _cost)
        {
            GameManager.Instance._PLAYERSAVE._MONEY -= _cost;
            Debug.Log("A");
            GetItem();
        }
    }

    void GetItem()
    {
        int index = Random.Range(0, _list._ITEMSAVES.Count - _list._HASITEMCOUNT);

        for (int i = 0; i < _list._ITEMSAVES.Count; i++)
        {
            if (!_list._ITEMSAVES[i]._HASITEM)
            {
                if (index == 0)
                {
                    _list._ITEMSAVES[i]._HASITEM = true;
                    _list._HASITEMCOUNT++;

                    return;
                }
                else index--;
            }
        }
    }


}
