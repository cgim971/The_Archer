using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    [SerializeField] private Button _matchBtn;
    [SerializeField] private Image _image;
    Button _imageBtn;
    [SerializeField] private Text _text;

    Sequence startSeq = null;
    Sequence endSeq = null;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _matchBtn.onClick.AddListener(() => MatchScene());
        _imageBtn = _image.GetComponent<Button>();
        _imageBtn.interactable = false;
        _text.raycastTarget = false;
        _imageBtn.onClick.AddListener(() => EndText());
    }

    void MatchScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void SetText(string text)
    {

        startSeq = DOTween.Sequence();
        startSeq.Append(_image.DOColor(new Color(1, 1, 1, 1), 0.4f));
        startSeq.Join(_text.DOColor(new Color(0, 0, 0, 1), 0.4f));

        _text.text = text + "È¹µæ";
        startSeq.Play();

        _text.raycastTarget = true;
        _imageBtn.interactable = true;
    }

    public void EndText()
    {
        endSeq = DOTween.Sequence();
        endSeq.Append(_image.DOColor(new Color(1, 1, 1, 0), 0.4f));
        endSeq.Join(_text.DOColor(new Color(0, 0, 0, 0), 0.4f));

        _text.text = string.Empty;
        endSeq.Play();

        _text.raycastTarget = false;
        _imageBtn.interactable = false;
    }

    private void OnGUI()
    {
        var labelStyle = new GUIStyle();
        labelStyle.fontSize = 50;
        labelStyle.normal.textColor = Color.white;
        GUILayout.Label("MONEY : " + GameManager.Instance._PLAYERSAVE._MONEY.ToString(), labelStyle);
        GUILayout.Label("HELMETS : " + GameManager.Instance._ITEMLIST._ITEMTYPESAVES[0]._HASITEMCOUNT.ToString() + " / " + GameManager.Instance._ITEMLIST._ITEMTYPESAVES[0]._ITEMSAVES.Count.ToString(), labelStyle);
        GUILayout.Label("WEAPONS : " + GameManager.Instance._ITEMLIST._ITEMTYPESAVES[1]._HASITEMCOUNT.ToString() + " / " + GameManager.Instance._ITEMLIST._ITEMTYPESAVES[1]._ITEMSAVES.Count.ToString(), labelStyle);
        GUILayout.Label("CHESTS : " + GameManager.Instance._ITEMLIST._ITEMTYPESAVES[2]._HASITEMCOUNT.ToString() + " / " + GameManager.Instance._ITEMLIST._ITEMTYPESAVES[2]._ITEMSAVES.Count.ToString(), labelStyle);
        GUILayout.Label("PANTSS : " + GameManager.Instance._ITEMLIST._ITEMTYPESAVES[3]._HASITEMCOUNT.ToString() + " / " + GameManager.Instance._ITEMLIST._ITEMTYPESAVES[3]._ITEMSAVES.Count.ToString(), labelStyle);
        GUILayout.Label("SHOULDERS : " + GameManager.Instance._ITEMLIST._ITEMTYPESAVES[4]._HASITEMCOUNT.ToString() + " / " + GameManager.Instance._ITEMLIST._ITEMTYPESAVES[4]._ITEMSAVES.Count.ToString(), labelStyle);
        GUILayout.Label("BOOTS : " + GameManager.Instance._ITEMLIST._ITEMTYPESAVES[5]._HASITEMCOUNT.ToString() + " / " + GameManager.Instance._ITEMLIST._ITEMTYPESAVES[5]._ITEMSAVES.Count.ToString(), labelStyle);
        var labelStyle2 = new GUIStyle();
        labelStyle2.fontSize = 100;
        if (GUI.Button(new Rect(1000, 100, 500, 200), "µ· 10 »ó½Â", labelStyle2))
        {
            GameManager.Instance._PLAYERSAVE._MONEY += 10;
        }
    }
}
