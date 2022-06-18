using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollScript : ScrollRect
{
    bool _forParent;
    ScrollManager _scrollManager;
    ScrollRect _parentScrollRect;

    protected override void Start()
    {
        _scrollManager = GameObject.FindWithTag("ScrollManager").GetComponent<ScrollManager>();
        _parentScrollRect = GameObject.FindWithTag("ScrollManager").GetComponent<ScrollRect>();
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        _forParent = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y);

        if (_forParent)
        {
            _scrollManager.OnBeginDrag(eventData);
            _parentScrollRect.OnBeginDrag(eventData);
        }
        else base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (_forParent)
        {
            _scrollManager.OnDrag(eventData);
            _parentScrollRect.OnDrag(eventData);
        }
        else base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (_forParent)
        {
            _scrollManager.OnEndDrag(eventData);
            _parentScrollRect.OnEndDrag(eventData);
        }
        else base.OnEndDrag(eventData);
    }
}
