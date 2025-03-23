using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;
    public Sprite normalImage;
    public Sprite mouseEnterImage;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = mouseEnterImage;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = normalImage;
    }
}
