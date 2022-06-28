using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    [SerializeField] private Image _image;

    [SerializeField] private Text _titleText = null;
    [SerializeField] private Text _contentsText = null;

    [SerializeField] private Button _nextBtn;
    [SerializeField] private Button _lobbyBtn;

    [SerializeField] private Button _pauseBtn;
    [SerializeField] private Image _pausePanel;
    [SerializeField] private Button _continueGameBtn;
    [SerializeField] private Button _endGameBtn;
    [SerializeField] private Button _lobbyBtn2;

    [SerializeField] private float _delay;

    [SerializeField] private int _money;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        HidePanel();

        _nextBtn.onClick.AddListener(() => HidePanel(true));
        _lobbyBtn.onClick.AddListener(() => HidePanel(false));
        _pauseBtn.onClick.AddListener(() => OnSetting());
        _continueGameBtn.onClick.AddListener(() => OffSetting());
        _endGameBtn.onClick.AddListener(() => Application.Quit());
        _lobbyBtn2.onClick.AddListener(() => Lobby());

        OffSetting();
    }

    public void OnSetting()
    {
        Time.timeScale = 0;
        _pausePanel.gameObject.SetActive(true);
    }
    public void OffSetting()
    {
        Time.timeScale = 1;
        _pausePanel.gameObject.SetActive(false);
    }

    public void Victory()
    {
        ShowPanel(true);

        _titleText.text = "VICTORY";
        _contentsText.text = $"{_money}¿ø È¹µæ";

        GameManager.Instance._PLAYERSAVE._MONEY += _money;
    }

    public void Defeat()
    {
        ShowPanel(false);

        _titleText.text = "DEFEAT";
        _contentsText.text = $"{_money / 3}¿øÀ» È¹µæÇÏ¼Ì½À´Ï´Ù.";
        GameManager.Instance._PLAYERSAVE._MONEY += _money / 3;
    }

    public void ShowPanel(bool isVictory)
    {
        _image.transform.gameObject.SetActive(true);
        _lobbyBtn.gameObject.SetActive(true);
        if (isVictory) _nextBtn.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();
        seq.Append(_image.DOFade(1, _delay));

        if (GameManager.Instance.STAGENUMBER == 4)
        {
            _nextBtn.gameObject.SetActive(false);
        }

        if (isVictory) seq.Join(_nextBtn.image.DOFade(1, _delay));

        seq.Join(_lobbyBtn.image.DOFade(1, _delay));

        _nextBtn.GetComponentInChildren<Text>().text = "Next Stage";
        _lobbyBtn.GetComponentInChildren<Text>().text = "Lobby";

        seq.Play();
    }

    public void HidePanel()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_image.DOFade(0, 0));
        seq.Join(_nextBtn.image.DOFade(0, 0));
        seq.Join(_lobbyBtn.image.DOFade(0, 0));

        _titleText.text = string.Empty;
        _contentsText.text = string.Empty;
        _nextBtn.GetComponentInChildren<Text>().text = string.Empty;
        _lobbyBtn.GetComponentInChildren<Text>().text = string.Empty;

        seq.Play();

        _nextBtn.gameObject.SetActive(false);
        _lobbyBtn.gameObject.SetActive(false);
        _image.gameObject.SetActive(false);
    }

    public void HidePanel(bool isNext)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_image.DOFade(0, _delay));
        seq.Join(_nextBtn.image.DOFade(0, _delay));
        seq.Join(_lobbyBtn.image.DOFade(0, _delay));

        _titleText.text = string.Empty;
        _contentsText.text = string.Empty;
        _nextBtn.GetComponentInChildren<Text>().text = string.Empty;
        _lobbyBtn.GetComponentInChildren<Text>().text = string.Empty;

        seq.Play();

        _nextBtn.gameObject.SetActive(false);
        _lobbyBtn.gameObject.SetActive(false);
        StartCoroutine(SetActiveObject(_image.gameObject, false, isNext));
    }

    IEnumerator SetActiveObject(GameObject obj, bool isSetActive, bool isNext)
    {
        yield return new WaitForSeconds(_delay);
        obj.SetActive(isSetActive);

        if (isNext) NextStage();
        else Lobby();
    }

    public void NextStage()
    {
        GameManager.Instance.STAGENUMBER += 1;
        SceneManager.LoadScene(GameManager.Instance.STAGENUMBER);
    }

    public void Lobby()
    {
        GameManager.Instance.STAGENUMBER = 0;
        SceneManager.LoadScene(GameManager.Instance.STAGENUMBER);
    }

    //[SerializeField] private List<GameObject> warriorMovement;
    //[SerializeField] private int _warriorCount;
    //bool _isClear = false;
    //[SerializeField] private Image _image;
    //Button _imageBtn;
    //[SerializeField] private Text _text;
    //string nextScene = string.Empty;
    //Sequence seq = null;
    //[SerializeField] int money = 100;

    //private void Awake()
    //{
    //    instance = this;

    //    _warriorCount = warriorMovement.Count;
    //}

    //private void Start()
    //{
    //    _imageBtn = _image.GetComponent<Button>();
    //    _imageBtn.interactable = false;
    //    _text.raycastTarget = false;
    //    _imageBtn.onClick.AddListener(() => Next());
    //}

    //public int _WARRIORCOUNT
    //{
    //    get => _warriorCount;
    //    set
    //    {
    //        if (_isClear) return;

    //        _warriorCount = value;
    //        if (_warriorCount <= 0)
    //        {
    //            _isClear = true;
    //            Clear();
    //        }
    //    }
    //}

    //public void Clear()
    //{
    //    seq = DOTween.Sequence();
    //    seq.Append(_image.DOColor(new Color(1, 1, 1, 1), 0.4f));
    //    seq.Join(_text.DOColor(new Color(0, 0, 0, 1), 0.4f));

    //    // ´ÙÀ½ ½ºÅ×ÀÌÁö Ã¢ ¶ßÁö¸¸ ÇöÀç ¾À ´Ù½Ã
    //    _text.text = $"CLEAR\n\n{money}¿ø È¹µæ\n\nÅÍÄ¡ ½Ã ´ÙÀ½ ½ºÅ×ÀÌÁö";

    //    _text.raycastTarget = true;
    //    _imageBtn.interactable = true;
    //    GameManager.Instance._PLAYERSAVE._MONEY += money;

    //    nextScene = "Main";
    //}

    //public void Death()
    //{
    //    seq = DOTween.Sequence();
    //    seq.Append(_image.DOColor(new Color(1, 1, 1, 1), 0.4f));
    //    seq.Join(_text.DOColor(new Color(0, 0, 0, 1), 0.4f));

    //    // ÇÃ·¹ÀÌ¾î Á×À½
    //    _text.text = $"´ç½ÅÀº Á×¾ú½À´Ï´Ù.\n\n{money / 2}¿ø È¹µæ\n\nÅÍÄ¡ ½Ã ·Îºñ·Î";

    //    _text.raycastTarget = true;
    //    _imageBtn.interactable = true;

    //    GameManager.Instance._PLAYERSAVE._MONEY += money / 2;

    //    nextScene = "Lobby";
    //}

    //public void Next()
    //{
    //    SceneManager.LoadScene(nextScene);
    //}
}
