using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class DropTable
{
    [SerializeField] private GameObject dropItemPrefab;
    [SerializeField] private int minAmount;
    [SerializeField] private int maxAmount;
    
    public int dropItemId { get; private set; }
    public int MinAmount => minAmount;
    public int MaxAmount => maxAmount;

    public GameObject GetDropItemPrefab => dropItemPrefab;

    public void Initialize()
    {
        if (dropItemPrefab != null)
        {
            dropItemId = dropItemPrefab.GetComponent<DropObject>().GetItemId();
        }
        else
        {
            Debug.LogError("DropItemPrefab이 할당되지 않았습니다.");
        }
    }
}

[CreateAssetMenu(fileName = "FieldObject", menuName = "SO/Field Object", order = int.MaxValue)]
public class FieldObjectData : ScriptableObject
{
    [Header("필드오브젝트 ID")] 
    public int id;
    
    [Header("도구 내구도 감소량")] [Tooltip("float")] 
    public float toolWear;

    [Header("오브젝트의 체력")] [Tooltip("float")] 
    public float durability;

    [Header("드랍아이템")] [Tooltip("float")] 
    public List<DropTable> dropTable;
    
    
    private void OnEnable()
    {
        for(int i = 0; i<dropTable.Count; i++)
            dropTable[i].Initialize();
    }
}