using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropTable
{
    public GameObject dropItem;
    public int minAmount;
    public int maxAmount;
}

[CreateAssetMenu(fileName = "FieldObject", menuName = "Scriptable Object/Field Object", order = int.MaxValue)]
public class FieldObjectData : ScriptableObject
{
    [Header("도구 내구도 감소량")] [Tooltip("float")]
    public float toolWear;

    [Header("오브젝트의 체력")] [Tooltip("float")]
    public float durability;

    [Header("드랍아이템")] [Tooltip("float")] 
    public List<DropTable> dropTable;
}
