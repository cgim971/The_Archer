using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [SerializeField] Scrollbar _scrollbar;
    [SerializeField] private Transform _contenTr;

    [SerializeField] private Slider _tabSlider;
    [SerializeField] private RectTransform[] _btnRect, _btnImageRect;

    const int SIZE = 4;
    float[] _pos = new float[SIZE];
    float _distance, _curPos, _targetPos;
    bool _isDrag;
    int _targetIndex;

    private void Start()
    {
        _distance = 1f / (SIZE - 1);
        for (int i = 0; i < SIZE; i++) _pos[i] = _distance * i;

        _targetPos = _pos[2];
        _targetIndex = 2;
    }

    float SetPos()
    {
        for (int i = 0; i < SIZE; i++)
        {
            if (_scrollbar.value < _pos[i] + _distance * 0.5f && _scrollbar.value > _pos[i] - _distance * 0.5f)
            {
                _targetIndex = i;
                return _pos[i];
            }
        }
        return 0;
    }

    public void OnBeginDrag(PointerEventData eventData) => _curPos = SetPos();

    public void OnDrag(PointerEventData eventData) => _isDrag = true;

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDrag = false;

        _targetPos = SetPos();

        if (_curPos == _targetPos)
        {
            if (eventData.delta.x > 18 && _curPos - _distance >= 0)
            {
                --_targetIndex;
                _targetPos = _curPos - _distance;
            }
            else if (eventData.delta.x < -18 && _curPos + _distance <= 1.01f)
            {
                ++_targetIndex;
                _targetPos = _curPos + _distance;
            }
        }

        for (int i = 0; i < SIZE; i++)
        {
            if (_contenTr.GetChild(i).GetComponent<ScrollScript>() && _curPos != _pos[i] && _targetPos == _pos[i])
                _contenTr.GetChild(i).GetChild(1).GetComponent<Scrollbar>().value = 1;
        }
    }

    private void Update()
    {
        _tabSlider.value = _scrollbar.value;

        if (!_isDrag)
        {
            _scrollbar.value = Mathf.Lerp(_scrollbar.value, _targetPos, 0.1f);

            for (int i = 0; i < SIZE; i++) _btnRect[i].sizeDelta = new Vector2(i == _targetIndex ? 480 : 240, _btnRect[i].sizeDelta.y);
        }

        if (Time.time < 0.1f) return;

        for (int i = 0; i < SIZE; i++)
        {
            Vector3 btnTargetPos = _btnRect[i].anchoredPosition3D;
            Vector3 btnTargetScale = Vector3.one;
            bool textActive = false;

            if(i == _targetIndex)
            {
                btnTargetPos.y = -50f;
                btnTargetScale = new Vector3(1.2f, 1.2f, 1);
                textActive = true;
            }

            _btnImageRect[i].anchoredPosition3D = Vector3.Lerp(_btnImageRect[i].anchoredPosition3D, btnTargetPos, 0.25f);
            _btnImageRect[i].localScale = Vector3.Lerp(_btnImageRect[i].localScale, btnTargetScale, 0.25f);
            _btnImageRect[i].transform.GetChild(0).gameObject.SetActive(textActive);
        }
    }

    public void TabClick(int n)
    {
        _targetIndex = n;
        _targetPos = _pos[n];
    }
}
