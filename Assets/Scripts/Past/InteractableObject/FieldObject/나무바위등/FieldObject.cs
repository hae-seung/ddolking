using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Tilemaps;

public enum TileTag
{
    Land,
    Ocean
}

//나무 바위등 플레이어가 캘 수 있는 것들
public class FieldObject : Interactable 
{
    public FieldObjectData fieldObjectData;
    protected float toolWear;
    protected float durability;
    protected List<DropTable> dropTable = new();
    public Slider durabilityBar;

    private bool isInteracting = false;
    [SerializeField] private float interactionDuration;
    
    [Header("드랍아이템이 떨어질 수 있는 타일맵")]
    public Tilemap tilemap;
    public TileTag tileType;

    protected void Awake()
    {
        toolWear = fieldObjectData.toolWear;
        durability = fieldObjectData.durability;
        dropTable = new List<DropTable>(fieldObjectData.dropTable);

        // 필드 오브젝트를 풀에 등록 (ID 기반)
        if (!ObjectPoolManager.Instance.IsPoolRegistered(fieldObjectData.id))
        {
            ObjectPoolManager.Instance.CreatePool(fieldObjectData.id, gameObject);
            Debug.Log($"{gameObject.name} (ID: {fieldObjectData.id}) 풀 등록 완료");
        }

        // 드랍 아이템도 풀에 등록
        foreach (var drop in dropTable)
        {
            if (drop != null)
            {
                int id = drop.dropItemId;
                if (!ObjectPoolManager.Instance.IsPoolRegistered(id))
                    ObjectPoolManager.Instance.CreatePool(id, drop.GetDropItemPrefab);
            }
            else
            {
                Debug.Log("drop할 아이템이 없다");
            }
        }
    }

    public void OnEnable()
    {
        durabilityBar.maxValue = durability;
        durabilityBar.minValue = 0;
        durabilityBar.value = durability;
        durabilityBar.gameObject.SetActive(false);
    }

    public override void Interact(Interactor interactor)
    {
        if (!isInteracting)
        {
            isInteracting = true;
            durabilityBar.gameObject.SetActive(true);
            ReduceDurability(interactionDuration);
        }
    }

    private void ReduceDurability(float duration)
    {
        durabilityBar.DOValue(0, duration).SetEase(Ease.Linear).OnComplete(() =>
        {
            isInteracting = false;
            durabilityBar.gameObject.SetActive(false);
            DropItems();
            DestroyFieldObject();  // 풀에 반환
        });
    }

    private void DropItems()
    {
        foreach (var drop in dropTable)
        {
            int dropAmount = Random.Range(drop.MinAmount, drop.MaxAmount + 1);

            for (int i = 0; i < dropAmount; i++)
            {
                // 드랍 위치 설정 (사후 보정 방식 적용)
                Vector3 potentialDropTarget = transform.position + 
                                              new Vector3(Random.Range(-1.5f, 1.5f),
                                                  Random.Range(-1.5f, 1.5f), 
                                                  0);
                
                Vector3 dropTarget = IsLandTile(potentialDropTarget) ? potentialDropTarget : transform.position;

                float randomRotation = Random.Range(0f, 360f);

                // 드랍 아이템을 풀에서 가져옴
                DropObject droppedItem = ObjectPoolManager.Instance.SpawnObject(drop.dropItemId,
                    transform.position, 
                    Quaternion.identity).GetComponent<DropObject>();

                if (droppedItem != null)
                {
                    droppedItem.transform.rotation = Quaternion.Euler(0, 0, randomRotation);

                    // 아이템 포물선 궤적 이동 및 바운스 효과
                    droppedItem.transform.DOJump(dropTarget, 
                            Random.Range(0.5f, 1f), 
                            1, 
                            0.8f).SetEase(Ease.OutBounce);

                }
                else
                {
                    Debug.LogError("드랍 아이템을 생성할 수 없습니다!");
                }
            }
        }
    }

    // 특정 위치가 Land 타일인지 확인하는 함수
    private bool IsLandTile(Vector3 position)
    {
        Vector3Int tilePosition = tilemap.WorldToCell(position);
        TileBase tile = tilemap.GetTile(tilePosition);

        // 타일맵 오브젝트의 이름 또는 태그를 검사하여 'Land'인지 확인
        return tile != null && tilemap.tag.Equals(tileType.ToString());
    }

    
   

    public void DestroyFieldObject()
    {
        ObjectPoolManager.Instance.ReleaseObject(fieldObjectData.id, gameObject);
    }
}
