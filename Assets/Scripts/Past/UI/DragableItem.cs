using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler

{
    private Transform canvas;
    private Transform previousParent;
    private RectTransform rect;
    private CanvasGroup canvasGroup;

    public Transform PreviousParent => previousParent;
    
    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>().transform;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        previousParent = transform.parent; // 현재 부모 저장
        transform.SetParent(canvas); // 아이템을 최상단 canvas로 이동
        transform.SetAsLastSibling(); // 드래그 중 다른 UI 요소 위에 표시

        canvasGroup.alpha = 0.6f; // 드래그 중 아이템을 반투명하게
        canvasGroup.blocksRaycasts = false; // 드래그 중에는 아이템 자체는 Raycast를 차단
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position; // 마우스 위치에 따라 아이템 이동

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        // 아이템이 드롭된 슬롯이 없으면 원래 부모로 복귀
        if (transform.parent == canvas)
        {
            transform.SetParent(previousParent); // 원래 부모로 복귀
            rect.position = previousParent.GetComponent<RectTransform>().position;
        }
    }

   
    
}
