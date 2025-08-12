using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEditor;
using UnityEngine;

public class PlayerBuild : MonoBehaviour
{
    private bool isBuilding = false;
    private PreviewObject previewObject;
    private EstablishItemData establishItemData;
    private EstablishItem establishItem;
    private Coroutine buildCoroutine;
    private bool buildComplete = false;

    private Vector2 mousePosition;
    
    private void Start() // UIManager가 싱글톤 초기화 후 실행
    {
        
        gameObject.SetActive(false);
        
        UIManager.Instance.ToggleBuildPanel(false);
        
        isBuilding = false;
        buildComplete = false;
    }

    private void Update()
    {
        if (establishItemData == null)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            CancelBuilding();
            return;
        }

        UpdatePreviewPosition();
        
        Collider2D[] hits = Physics2D.OverlapPointAll(mousePosition);
        bool isOnValidTarget = false;

        foreach (var c in hits)
        {
            if (((1 << c.gameObject.layer) & establishItemData.TargetLayer) != 0)
            {
                isOnValidTarget = true;
                break;
            }
        }

        // 설치 가능 여부 계산
        bool isValidPlacement = isOnValidTarget && previewObject.CanEstablish;
        
        previewObject.SetPlacementValidity(isValidPlacement);

        if (isValidPlacement && Input.GetMouseButtonDown(0))
        {
            buildComplete = true;
        }
    }
    

    public IEnumerator BuildItem(EstablishItem establishItem, Action<bool> callback)
    {
        if (isBuilding)
        {
            callback(false);
            yield break;
        }

        isBuilding = true;

        UIManager.Instance.ToggleBuildPanel(true);
        GameEventsManager.Instance.inputEvents.DisableInput();

        previewObject = Instantiate(establishItem.EstablishData.PreviewObject).GetComponent<PreviewObject>();
        establishItemData = establishItem.EstablishData;
        this.establishItem = establishItem;
        
        gameObject.SetActive(true);

        // 현재 실행 중인 코루틴 저장
        buildCoroutine = StartCoroutine(BuildProcess(callback));
    }

    
    
    private IEnumerator BuildProcess(Action<bool> callback)
    {
        // 설치가 완료될 때까지 대기
        yield return new WaitUntil(() => buildComplete);

        // 설치 완료 후 true 반환
        UIManager.Instance.ToggleBuildPanel(false);
        GameEventsManager.Instance.inputEvents.EnableInput();
        
        // 프리뷰 오브젝트 제거
        if (previewObject != null)
        {
            Destroy(previewObject.gameObject);
            previewObject = null;
        }

        int itemId = establishItemData.EstablishObjectData.id;
        //해당 마우스 위치에 오브젝트 설치
        if (!ObjectPoolManager.Instance.IsPoolRegistered(itemId))
        {
            ObjectPoolManager.Instance.RegisterPrefab(itemId, establishItemData.EstablishObjectData.ownObject);
        }
        
        IReBuild rebuildItem = ObjectPoolManager.Instance.SpawnObject(
            itemId,
            new Vector3(mousePosition.x, mousePosition.y, 0),
            Quaternion.identity).GetComponent<IReBuild>();

        if (rebuildItem != null)
        {
            rebuildItem.SetRebuildItem(establishItem);
        }
        
        
        //빌딩 시스템off
        callback(true);
        gameObject.SetActive(false);
        buildComplete = false;
        isBuilding = false;
        buildCoroutine = null;
    }

    private void CancelBuilding()
    {
        if (!isBuilding) return;
        isBuilding = false;
        buildComplete = false;

        // 코루틴 중단
        StopCoroutine(buildCoroutine);
        buildCoroutine = null;

        // UI 및 입력 초기화
        UIManager.Instance.ToggleBuildPanel(false);
        GameEventsManager.Instance.inputEvents.EnableInput();

        //프리뷰 오브젝트 제거
        if (previewObject != null)
        {
            Destroy(previewObject.gameObject);
            previewObject = null;
        }

        //건설시스템 off
        gameObject.SetActive(false);
    }

    private void UpdatePreviewPosition()
    {
        if (previewObject == null) return;

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector2(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y));
        previewObject.transform.position = mousePosition;
    }
}


