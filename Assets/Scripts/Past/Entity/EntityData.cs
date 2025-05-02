using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityData : ScriptableObject
{
    [SerializeField] protected int entityId;
    [SerializeField] protected float hp;
    [SerializeField] protected float toolWear;
    [SerializeField] protected float moveSpeed;
    [Range(0f,1f)]
    [SerializeField] protected float defense;

    //사망시 드랍
    [SerializeField] protected EntityDropTable dropTable;
    public int EntityId => entityId;
    public float Hp => hp;
    public float ToolWear => toolWear;
    public float MoveSpeed => moveSpeed;
    
    public float Defense => defense;

    public EntityDropTable DropTable => dropTable;

}
