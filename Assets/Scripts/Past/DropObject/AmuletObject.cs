using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmuletObject : EquipObject<AmuletItem>
{
    [SerializeField] private AmuletItemData data;

    protected override void Awake()
    {
        base.Awake();
        dropObjectPrefab = data.DropObjectPrefab;
    }
    
    protected override void MakeItemInstance()
    {
        equipItem = new AmuletItem(data);
        itemId = data.ID;
    }
    
    
    protected override void OnEnable()
    {
        if (!isSpawned)  // 씬에 배치된 경우 == 최초 활성화 시 작업 방지
        {
            isSpawned = true;
            return;
        }

        Debug.Log("AmuletEnable 실행");
        equipItem = new AmuletItem(data);
        base.OnEnable();
    }
    
}
