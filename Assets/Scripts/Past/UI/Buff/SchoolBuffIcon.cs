using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SchoolBuffIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rt;
    private Action<RectTransform> OnOpen;
    private Action OnClose;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    public void Init(Action<RectTransform> OpenToolTip, Action CloseToolTip)
    {
        OnOpen = OpenToolTip;
        OnClose = CloseToolTip;
    }
    
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnOpen?.Invoke(rt);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnClose?.Invoke();
    }
}
