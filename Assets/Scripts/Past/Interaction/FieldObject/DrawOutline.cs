using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class DrawOutline : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    

    private bool isPointerHovering = false;
    private bool isPlayerNear = false;

    public event Action onPointerExit;

    private void Start()
    {
        targetObject.SetActive(false);
    }

    public bool CanInteract => isPointerHovering && isPlayerNear;

    private void OnEnable()
    {
        GameEventsManager.Instance.inputEvents.onMouseMoved += OnMouseMoved;
    }

    private void OnDisable()
    {
        if(GameEventsManager.Instance != null)
            GameEventsManager.Instance.inputEvents.onMouseMoved -= OnMouseMoved;
    }

    private void OnMouseMoved(Vector3 mousePosition)
    {
        // 플레이어가 근처에 없으면 즉시 종료
        if (!isPlayerNear)
        {
            if (isPointerHovering)
            {
                isPointerHovering = false;
                UpdateOutline(false);
            }
            return;
        }

        //RaycastAll() 실행
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);
    
        // 충돌한 오브젝트가 없으면 `false`로 설정
        if (hits.Length == 0)
        {
            if (isPointerHovering)
            {
                isPointerHovering = false;
                UpdateOutline(false);
            }
            return;
        }

        bool hovering = false;
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider != null && hits[i].collider.gameObject == gameObject)
            {
                hovering = true;
                break;
            }
        }

        // 상태가 변경될 때만 `UpdateOutline()` 호출하여 성능 최적화
        if (isPointerHovering != hovering)
        {
            isPointerHovering = hovering;
            UpdateOutline(isPointerHovering);
        }
    }
    

    public void SetPlayerNear(bool state)
    {
        isPlayerNear = state;

        if (isPlayerNear && isPointerHovering)
        {
            UpdateOutline(true);
        }
        else
        {
            UpdateOutline(false);
        }
    }

    private void UpdateOutline(bool outline)
    {
        targetObject.SetActive(outline);
        if (!outline)
        {
            onPointerExit?.Invoke();
        }
    }
}