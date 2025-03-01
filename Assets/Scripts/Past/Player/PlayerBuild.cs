using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuild : MonoBehaviour
{
    private bool isBuilding = false;
    private PreviewObject previewObject;
    private EstablishItemData establishItemData;
    private Coroutine buildCoroutine;
    private bool buildComplete = false;

    private Vector2 mousePosition;
    private Ray ray;
    private Camera mainCamera;

    private void Start() // UIManager가 싱글톤 초기화 후 실행
    {
        mainCamera = Camera.main;
        
        gameObject.SetActive(false);
        
        UIManager.Instance.ToggleBuildPanel(false);
        
        isBuilding = false;
        buildComplete = false;
    }

    private void Update()
    {
        // 우클릭(취소) 시 코루틴 종료 및 초기화
        if (Input.GetMouseButtonDown(1))
        {
            CancelBuilding();
            return;
        }

        UpdatePreviewPosition();

        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Vector2 rayOrigin = ray.origin;

        // Raycast를 사용하여 Land 감지
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.zero, Mathf.Infinity);

        if (hit.collider == null)
            return;

        if (hit.transform.CompareTag("Land") && previewObject.CanEstablish)
        {
            if (Input.GetMouseButtonDown(0)) // 좌클릭(설치)
            {
                buildComplete = true; // 설치 완료 후 `Coroutine` 종료됨
            }
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

        ObjectPoolManager.Instance.SpawnObject(
            itemId,
            new Vector3(mousePosition.x, mousePosition.y, 0),
            Quaternion.identity);
        
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
        previewObject.transform.position = mousePosition;
    }
}


