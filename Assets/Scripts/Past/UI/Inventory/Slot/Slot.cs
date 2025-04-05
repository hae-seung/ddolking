using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;

public class Slot : MonoBehaviour
{
    [SerializeField] private RectTransform slotRect;
    [SerializeField] private Image iconImage;
    [SerializeField] private RectTransform iconRect;
    [SerializeField] private TextMeshProUGUI amountText;

    [SerializeField] private Image highlightImage;
    [SerializeField] private float highlightAlpha = 0.3f;
    [SerializeField] private float highlightFadeDuration = 0.2f;
    private float currentAlpha = 0f;
    
    public int SlotIdx { get; private set; }
    public RectTransform SlotRect => slotRect;
    public bool IsUsing { get; private set; } = false;
    
    private void ShowIcon() => iconImage.gameObject.SetActive(true);
    private void HideIcon() => iconImage.gameObject.SetActive(false);
    private void ShowText() => amountText.gameObject.SetActive(true);
    private void HideText() => amountText.gameObject.SetActive(false);
    
    public RectTransform IconRect => iconRect;

    public void SetIndex(int idx)
    {
        SlotIdx = idx;
    }
    

    public void UpdateItemAmount(int amount = 0)
    {
        if(amount == 0)
            HideText();
        else
        {
            amountText.text = amount.ToString();
            ShowText();
        }
    }

    public void SwapOrMoveIcon(Slot other)
    {
        if (other == null) return;

        Sprite tempSprite = iconImage.sprite;

        SetItemIcon(other.iconImage.sprite);
        other.SetItemIcon(tempSprite);
    }

    public void SetItemIcon(Sprite otherSprite)
    {
        if (otherSprite != null)
        {
            iconImage.sprite =  otherSprite;
            IsUsing = true;                         //중요
            ShowIcon();
        }
        else//다른 슬롯엔 아이템이 없었음을 의미
        {
            RemoveItem();
        }
    }

    public void RemoveItem()
    {
        HideIcon();
        HideText();
        IsUsing = false;                            //중요
        iconImage.sprite = null;
    }
    
    
    public void HideAmountText()
    {
        HideText();
    }

    public void Highlight(bool show)
    {
        if (show)
            StartCoroutine(nameof(HighlightFadeInRoutine));
        else
            StartCoroutine(nameof(HighlightFadeOutRoutine));
    }

    //하이라이트 알파값 서서히 증가
    private IEnumerator HighlightFadeInRoutine()
    {
        StopCoroutine(nameof(HighlightFadeOutRoutine));
        highlightImage.gameObject.SetActive(true);
        float unit = highlightAlpha / highlightFadeDuration;

        for (; currentAlpha <= highlightAlpha; currentAlpha += unit * Time.deltaTime)
        {
            highlightImage.color = new Color(
                highlightImage.color.r,
                highlightImage.color.g,
                highlightImage.color.b,
                currentAlpha
            );

            yield return null;
        }
    }

    //하이라이트 알파값 0까지 서서히 감소
    private IEnumerator HighlightFadeOutRoutine()
    {
        StopCoroutine(nameof(HighlightFadeInRoutine));

        float unit = highlightAlpha / highlightFadeDuration;

        for (; currentAlpha >= 0f; currentAlpha -= unit * Time.deltaTime)
        {
            highlightImage.color = new Color(
                highlightImage.color.r,
                highlightImage.color.g,
                highlightImage.color.b,
                currentAlpha
            );

            yield return null;
        }

        highlightImage.gameObject.SetActive(false);
    }
}