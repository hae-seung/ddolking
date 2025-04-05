using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragableUI : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] private Transform _targetTr;
    private Vector2 _beginPoint;
    private Vector2 _moveBegin;

    private void Awake()
    {
        if (_targetTr == null)
            _targetTr = transform.parent;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        _beginPoint = _targetTr.position;
        _moveBegin = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _targetTr.position = _beginPoint + (eventData.position - _moveBegin);
    }
}
