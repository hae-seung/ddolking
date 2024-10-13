using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Tilemaps;
using UnityEngine.Pool; // 풀링을 사용하기 위한 네임스페이스 추가

public class FieldObject : Interactable
{
    public FieldObjectData fieldObjectData;
    protected float toolWear; // 감소시킬 내구도
    protected float durability; // 오브젝트의 상호작용 체력
    protected List<DropTable> dropTable = new(); // 드랍할 아이템 목록
    public GameObject durabilityPanel;
    public Slider durabilityBar;

    private bool isInteracting = false;
    [SerializeField] private float interactionDuration; // 내구도 감소 시간
    public Tilemap tilemap;

    // 드랍 아이템 풀을 관리할 딕셔너리
    private Dictionary<GameObject, ObjectPool<DropObject>> dropObjectPools = new();

    protected void Awake()
    {
        toolWear = fieldObjectData.toolWear;
        durability = fieldObjectData.durability;
        dropTable = new List<DropTable>(fieldObjectData.dropTable);

        // 각 드랍 아이템마다 DropObjectManager에서 풀을 가져옴
        foreach (var drop in dropTable)
        {
            GameObject dropPrefab = drop.dropItem;
            dropObjectPools[dropPrefab] = DropObjectManager.Instance.GetOrCreatePool(dropPrefab);
        }
    }

    public void Start()
    {
        // 내구도 바 초기화
        durabilityBar.maxValue = durability; 
        durabilityBar.minValue = 0;          
        durabilityBar.value = durability;    
        durabilityPanel.SetActive(false);    
    }

    public override void Interact(Interactor interactor)
    {
        if (!isInteracting)
        {
            isInteracting = true;
            durabilityPanel.SetActive(true); 
            ReduceDurability(interactionDuration); 
        }
    }

    // 내구도 감소 (DOTween 사용)
    private void ReduceDurability(float duration)
    {
        Debug.Log("Duration: " + duration); 

        durabilityBar.DOValue(0, duration).SetEase(Ease.Linear).OnComplete(() =>
        {
            isInteracting = false;
            durabilityPanel.SetActive(false); 
            DropItems(); 
            Destroy(gameObject); // 오브젝트 파괴
        });
    }

    // 나무가 쓰러질 때 아이템을 드랍하는 메서드
    private void DropItems()
    {
        foreach (var drop in dropTable)
        {
            // 드랍할 아이템 수량을 무작위로 결정
            int dropAmount = UnityEngine.Random.Range(drop.minAmount, drop.maxAmount + 1);

            for (int i = 0; i < dropAmount; i++)
            {
                // 드랍할 아이템 프리팹을 드랍 테이블에서 가져옴
                GameObject dropPrefab = drop.dropItem;

                // 드랍 위치 설정
                Vector3 dropTarget = GetValidDropPosition();
                float randomRotation = UnityEngine.Random.Range(0f, 360f);
              
                // 해당 프리팹의 풀에서 DropObject를 가져옴
                DropObject droppedItem = dropObjectPools[dropPrefab].Get();
                droppedItem.transform.position = transform.position; // 드랍 위치 설정
                droppedItem.transform.rotation = Quaternion.Euler(0, 0, randomRotation);

                // 아이템 포물선 궤적 이동 및 바운스 효과
                droppedItem.transform.DOJump(dropTarget, 1f, 1, 1f).SetEase(Ease.OutBounce);
            }
        }
    }

    // 드랍 가능한 위치를 반환하는 함수
    private Vector3 GetValidDropPosition()
    {
        Vector3 dropTarget;
        dropTarget = transform.position + new Vector3(UnityEngine.Random.Range(-1.5f, 1.5f), UnityEngine.Random.Range(-1.5f, 1.5f), 0);
        if (IsLandTile(dropTarget))
            return dropTarget;
        return transform.position;
    }

    // 특정 위치가 Land 타일인지 확인하는 함수
    private bool IsLandTile(Vector3 position)
    {
        Vector3Int tilePosition = tilemap.WorldToCell(position);
        TileBase tile = tilemap.GetTile(tilePosition);

        if (tile != null)
        {
            // 타일맵 오브젝트의 이름이 'Land'이면 Land 타일로 간주
            if (tilemap.gameObject.name == "Land")
                return true;
        }
        return false;
    }
    

    // DropObject 생성 (프리팹을 인자로 받음)
    private DropObject CreateDropObject(GameObject dropPrefab)
    {
        DropObject newItem = Instantiate(dropPrefab).GetComponent<DropObject>();
        newItem.SetManagedPool(dropObjectPools[dropPrefab]); // 풀을 설정해줌
        return newItem;
    }

    // DropObject를 풀에서 가져올 때 호출되는 메서드
    private void OnGetDropObject(DropObject item)
    {
        item.gameObject.SetActive(true);
    }

    // DropObject를 풀에 반환할 때 호출되는 메서드
    private void OnReleaseDropObject(DropObject item)
    {
        item.gameObject.SetActive(false);
    }

    // DropObject를 파괴할 때 호출되는 메서드
    private void OnDestroyDropObject(DropObject item)
    {
        Destroy(item.gameObject);
    }
}