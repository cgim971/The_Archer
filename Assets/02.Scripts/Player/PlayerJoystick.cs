using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform _rectBack;
    public RectTransform _rectJoystick;

    float _joystickRadius;
    bool _isTouch = false;
    Vector3 _vecMove = Vector3.zero;

    public Vector3 VecMove
    {
        get { return _vecMove; }
    }

    void Start() => _joystickRadius = _rectBack.rect.width * 0.5f;


    void OnTouch(Vector2 vecTouch)
    {
        Vector2 vec = new Vector2(vecTouch.x - _rectBack.position.x, vecTouch.y - _rectBack.position.y);

        vec = Vector2.ClampMagnitude(vec, _joystickRadius);
        _rectJoystick.localPosition = vec;

        float sqr = (_rectBack.position - _rectJoystick.position).sqrMagnitude / (_joystickRadius * _joystickRadius);

        Vector2 index = vec.normalized;
        _vecMove = new Vector3(index.x * sqr, 0, index.y * sqr);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        _isTouch = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        _isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _rectJoystick.localPosition = Vector2.zero;
        _vecMove = Vector3.zero;
        _isTouch = false;
    }
}
