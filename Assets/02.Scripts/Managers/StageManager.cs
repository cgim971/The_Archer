using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    [SerializeField] private List<WarriorMovement> warriorMovement;
    [SerializeField] private int _warriorCount;
    bool _isClear = false;
    [SerializeField] private Image _image;
    Button _imageBtn;
    [SerializeField] private Text _text;
    string nextScene = string.Empty;
    Sequence seq = null;

    private void Awake()
    {
        instance = this;

        _warriorCount = warriorMovement.Count;
    }

    private void Start()
    {
        _imageBtn = _image.GetComponent<Button>();
        _imageBtn.interactable = false;
        _text.raycastTarget = false;
        _imageBtn.onClick.AddListener(() => Next());
    }

    public int _WARRIORCOUNT
    {
        get => _warriorCount;
        set
        {
            if (_isClear) return;

            _warriorCount = value;
            if (_warriorCount <= 0)
            {
                _isClear = true;
                Clear();
            }
        }
    }

    public void Clear()
    {
        seq = DOTween.Sequence();
        seq.Append(_image.DOColor(new Color(1, 1, 1, 1), 0.4f));
        seq.Join(_text.DOColor(new Color(0, 0, 0, 1), 0.4f));

        // 다음 스테이지 창 뜨지만 현재 씬 다시
        _text.text = "CLEAR\n\n터치 시 다음 스테이지";

        _text.raycastTarget = true;
        _imageBtn.interactable = true;

        nextScene = "Main";
    }

    public void Death()
    {
        seq = DOTween.Sequence();
        seq.Append(_image.DOColor(new Color(1, 1, 1, 1), 0.4f));
        seq.Join(_text.DOColor(new Color(0, 0, 0, 1), 0.4f));

        // 플레이어 죽음
        _text.text = "당신은 죽었습니다.\n\n터치 시 로비로";

        _text.raycastTarget = true;
        _imageBtn.interactable = true;

        nextScene = "Lobby";
    }

    public void Next()
    {
        SceneManager.LoadScene(nextScene);
    }

}
