using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : EquipObject<WeaponItem>
{
    [SerializeField] private WeaponItemData data;

    protected override void Awake()
    {
        base.Awake();
        dropObjectPrefab = data.DropObjectPrefab;
    }
    
    protected override void MakeItemInstance()
    {
        equipItem = new WeaponItem(data);
        itemId = data.ID;
    }
    
    
    protected override void OnEnable()
    {
        if (!isSpawned)  // 씬에 배치된 경우 == 최초 활성화 시 작업 방지
        {
            isSpawned = true;
            return;
        }

        Debug.Log("WeaponEnable 실행");
        equipItem = new WeaponItem(data);
        base.OnEnable();
    }
    
}
