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

    [SerializeField] private string _nextScene;
    [SerializeField] private float _delay;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        HidePanel();

        _nextBtn.onClick.AddListener(() => HidePanel(true));
        _lobbyBtn.onClick.AddListener(() => HidePanel(false));
    }

    public void Victory()
    {
        ShowPanel(true);

        _titleText.text = "VICTORY";
        _contentsText.text = "";

        //GameManager.Instance._PLAYERSAVE._MONEY += ;
    }

    public void Defeat()
    {
        ShowPanel(false);

        _titleText.text = "DEFEAT";
        _contentsText.text = "";

        //GameManager.Instance._PLAYERSAVE._MONEY += ;
    }

    public void ShowPanel(bool isVictory)
    {
        _image.transform.gameObject.SetActive(true);
        if (isVictory) _nextBtn.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();
        seq.Append(_image.DOFade(1, _delay));
        if (isVictory) seq.Join(_nextBtn.image.DOFade(1, _delay));
        seq.Join(_lobbyBtn.image.DOFade(1, _delay));

        seq.Play();
    }

    public void HidePanel()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_image.DOFade(0, 0));
        seq.Join(_nextBtn.image.DOFade(0, 0));
        seq.Join(_lobbyBtn.image.DOFade(0, 0));
        seq.Play();
        _titleText.text = string.Empty;
        _contentsText.text = string.Empty;

        _nextBtn.gameObject.SetActive(false);
        _image.gameObject.SetActive(false);
    }

    public void HidePanel(bool isNext)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_image.DOFade(0, _delay));
        seq.Join(_nextBtn.image.DOFade(0, _delay));
        seq.Join(_lobbyBtn.image.DOFade(0, _delay));

        seq.Play();

        _titleText.text = string.Empty;
        _contentsText.text = string.Empty;

        _nextBtn.gameObject.SetActive(false);
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
        SceneManager.LoadScene(_nextScene);
    }

    public void Lobby()
    {
        SceneManager.LoadScene("Lobby");
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

    //    // 다음 스테이지 창 뜨지만 현재 씬 다시
    //    _text.text = $"CLEAR\n\n{money}원 획득\n\n터치 시 다음 스테이지";

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

    //    // 플레이어 죽음
    //    _text.text = $"당신은 죽었습니다.\n\n{money / 2}원 획득\n\n터치 시 로비로";

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
