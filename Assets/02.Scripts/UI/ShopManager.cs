using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [SerializeField] private Image _image;

    [SerializeField] private Text _titleText = null;
    [SerializeField] private Text _contentsText = null;

    [SerializeField] private Button _closeBtn;
    [SerializeField] private float _delay;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _closeBtn.onClick.AddListener(() => Close());
    }

    public void Purchase(ItemSave item)
    {
        _image.gameObject.SetActive(true);

        ShowPanel();

        _titleText.text = item._ITEMNAME;
        _contentsText.text = item._ITEMINFORM;
    }

    public void Close()
    {
        HidePanel();
    }

    public void ShowPanel()
    {
        _image.transform.gameObject.SetActive(true);
        _closeBtn.GetComponentInChildren<Text>().text = "Close";

        Sequence seq = DOTween.Sequence();
        seq.Append(_image.DOFade(1, _delay));
        seq.Join(_closeBtn.image.DOFade(1, _delay));

        seq.Play();
    }

    public void HidePanel()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_image.DOFade(0, _delay));
        seq.Join(_closeBtn.image.DOFade(0, _delay));

        _titleText.text = string.Empty;
        _contentsText.text = string.Empty;
        _closeBtn.GetComponentInChildren<Text>().text = string.Empty;

        seq.Play();

        StartCoroutine(SetActiveObject(_image.gameObject, false));
    }

    IEnumerator SetActiveObject(GameObject obj, bool isSetActive)
    {
        yield return new WaitForSeconds(_delay);
        obj.SetActive(isSetActive);
    }
}
