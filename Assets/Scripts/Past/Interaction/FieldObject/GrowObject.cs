using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BreakableObject))]
public class GrowObject : MonoBehaviour
{
    private BreakableObject _breakableObject;
    
    [Header("자라는 시간")]
    [SerializeField] public float growTime;
    [Space(20)]
    [Header("소환시킬 필드오브젝트")]
    [SerializeField] public FieldObjectData spawnFieldObject;

    private void Awake()
    {
        _breakableObject = GetComponent<BreakableObject>();
    }

    private void OnEnable()
    {
        Invoke(nameof(GrowUp), growTime);
    }
    

    private void GrowUp()
    {
        if (!gameObject.activeSelf)
            return;
        
        ObjectPoolManager.Instance.SpawnObject(
            spawnFieldObject.id,
            transform.position,
            Quaternion.identity);
        _breakableObject.DestroyFieldObject();
    }

}
