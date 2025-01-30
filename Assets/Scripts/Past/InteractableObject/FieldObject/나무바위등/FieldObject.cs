using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public enum TileTag
{
    Land,
    Ocean
}

public class FieldObject : Interactable
{
    public FieldObjectData fieldObjectData;
    private float durability;
    private List<DropTable> dropTable = new();

    [SerializeField] private float interactionDuration;
    [Header("드랍아이템이 떨어질 수 있는 타일맵")]
    public Tilemap tilemap;
    public TileTag tileType;
    public Slider durabilityBar;
    private bool isInteracting = false;

    private void Awake()
    {
        durability = fieldObjectData.durability;
        dropTable = new List<DropTable>(fieldObjectData.dropTable);
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
            DestroyFieldObject();
        });
    }

    private void DropItems()
    {
        foreach (var drop in dropTable)
        {
            // 풀에 아이템이 등록되어 있는지 확인 후 등록
            if (!ObjectPoolManager.Instance.IsPoolRegistered(drop.DropItemId))
            {
                ObjectPoolManager.Instance.RegisterPrefab(drop.DropItemId, drop.DropItemPrefab);
            }

            for (int i = 0; i < Random.Range(drop.MinAmount, drop.MaxAmount + 1); i++)
            {
                Vector3 dropPosition = transform.position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0);

                GameObject dropObj = ObjectPoolManager.Instance.SpawnObject(drop.DropItemId, dropPosition, Quaternion.identity);

                if (dropObj != null)
                {
                    dropObj.transform.DOJump(dropPosition, 1f, 1, 0.8f).SetEase(Ease.OutBounce);
                }
                else
                {
                    Debug.LogError($"ID {drop.DropItemId}의 드랍 아이템을 생성할 수 없습니다!");
                }
            }
        }
    }

    private void DestroyFieldObject()
    {
        ObjectPoolManager.Instance.ReleaseObject(fieldObjectData.id, gameObject);
    }
}
