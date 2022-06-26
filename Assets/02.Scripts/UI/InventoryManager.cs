using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] private Transform _player;
    [SerializeField] private Transform[] _parts;
    [SerializeField] private Transform _inventory;

    [SerializeField] private GameObject _slots;
    [SerializeField] private List<GameObject> _slotList;

    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private Image _panel;
    [SerializeField] private Text _titleText;
    [SerializeField] private Text _contentText;
    [SerializeField] private GameObject _btns;
    [SerializeField] private float _delay = 0.4f;

    [SerializeField] private ItemList _itemList;

    [SerializeField] ItemSave _itemSave;

    private void Start()
    {
        _itemList = GameManager.Instance._ITEMLIST;

        for (int index = 0; index < 30; index++)
        {
            GameObject newSlot = Instantiate(_slots, _inventory);
            newSlot.GetComponent<SlotManager>().INVENTORY = this;
            foreach (ItemSave itemSave in _itemList._ITEMSAVES)
            {
                if (_itemList._ITEMSAVES[index]._HASITEM)
                    newSlot.GetComponent<SlotManager>().SetItemSave(_itemList._ITEMSAVES[index]);
            }

            _slotList.Add(newSlot);
        }

        _btns.transform.Find("useBtn").GetComponent<Button>().onClick.AddListener(() => Use());
        _btns.transform.Find("cancelBtn").GetComponent<Button>().onClick.AddListener(() => Cancel());

        OffUI();
    }

    private void Update()
    {

    }

    public void OnUI()
    {
        _menuPanel.SetActive(true);

        _player.gameObject.SetActive(false);
        _btns.SetActive(true);
        Sequence seq = DOTween.Sequence();
        seq.Append(_panel.DOFade(1, _delay));

        seq.Play();

        _titleText.text = "title";
        _contentText.text = "content";
    }

    public void OnUI(ItemSave itemSave)
    {
        _itemSave = itemSave;

        _menuPanel.SetActive(true);

        _player.gameObject.SetActive(false);
        _btns.SetActive(true);
        Sequence seq = DOTween.Sequence();
        seq.Append(_panel.DOFade(1, _delay));

        seq.Play();

        _titleText.text = itemSave._ITEMNAME;
        _contentText.text = itemSave._ITEMINFORM;
    }

    public void OffUI()
    {
        _player.gameObject.SetActive(true);
        _btns.SetActive(false);

        Sequence seq = DOTween.Sequence();
        seq.Append(_panel.DOFade(0, _delay));

        seq.Play();

        _titleText.text = string.Empty;
        _contentText.text = string.Empty;

        StartCoroutine(SetActiveObject(_menuPanel, false));
    }

    IEnumerator SetActiveObject(GameObject obj, bool isSetActive)
    {
        yield return new WaitForSeconds(_delay);
        obj.SetActive(isSetActive);
    }

    public void Use()
    {


        OffUI();
    }

    public void Cancel()
    {


        OffUI();
    }

}
