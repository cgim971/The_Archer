using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheeetManager : MonoBehaviour
{

    const string url = "https://script.google.com/macros/s/AKfycbwX5cuf2etsF_7iiygUnnrfdzx7SfVrg13KX1FjsqwD8syqyNgiB28l9kZLFE2PUMZe9A/exec";

    [SerializeField] private PlayerSave _playerSave;
    [SerializeField] private ItemList _itemList;

    private void Start()
    {
        _playerSave = GameManager.Instance._PLAYERSAVE;
        _itemList = GameManager.Instance._ITEMLIST;

        for (int i = 0; i < _itemList._ITEMSAVES.Count; i++)
        {
            ItemSaveFile(i);
        }

        MoneySave();
    }

    public void MoneySave()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "player");
        form.AddField("money", _playerSave._MONEY.ToString());

        StartCoroutine(Post(form));
    }

    public void ItemSaveFile(int index)
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "item");
        form.AddField("index", index);
        form.AddField("hasItem", _itemList._ITEMSAVES[index]._HASITEM.ToString());
        form.AddField("equipmentItem", _itemList._ITEMSAVES[index]._EQUIPMENTITEM.ToString());

        StartCoroutine(Post(form));
    }

    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.isDone) print(www.downloadHandler.text);
            else print("웹의 응답이 없습니다.");
        }

    }

}
