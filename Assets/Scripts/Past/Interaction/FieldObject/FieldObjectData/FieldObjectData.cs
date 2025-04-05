using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "FieldObject", menuName = "SO/Field Object", order = 1)]
public class FieldObjectData : ScriptableObject
{
    public int id;
    public float toolWear;
    public float durability;
    public List<DropTable> dropTable;
    
    
    [Header("필드 오브젝트 자신의 프리팹")]
    [SerializeField] public GameObject ownObject;
}
