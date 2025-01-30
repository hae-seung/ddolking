using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropTable
{
    [SerializeField] private GameObject dropItemPrefab;
    [SerializeField] private int minAmount;
    [SerializeField] private int maxAmount;

    public int DropItemId { get; private set; }
    public int MinAmount => minAmount;
    public int MaxAmount => maxAmount;
    public GameObject DropItemPrefab => dropItemPrefab;

    public void Initialize()
    {
        DropItemId = dropItemPrefab.GetComponent<DropObject>().GetItemId();
    }
}

[CreateAssetMenu(fileName = "FieldObject", menuName = "SO/Field Object", order = 1)]
public class FieldObjectData : ScriptableObject
{
    public int id;
    public float toolWear;
    public float durability;
    public List<DropTable> dropTable;

    private void OnEnable()
    {
        foreach (var drop in dropTable)
        {
            drop.Initialize();
        }
    }
}
