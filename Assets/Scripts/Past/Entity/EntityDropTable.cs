using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityDropTable", menuName = "SO/Entity/DropTable")]
public class EntityDropTable : ScriptableObject
{
    public int experience;
    public List<EntityDropItemLogic> items;
}

[System.Serializable]
public class EntityDropItemLogic
{
    public ItemData dropItem;
    
    [SerializeField][Range(1,100)]
    private int probability;
    [SerializeField]
    private int dropItemAmount_Min;
    [SerializeField]
    private int dropItemAmount_Max;


    public int CalculateAndGetPrefab()
    {
        int amountRanNum = 0;
        int ranNum = Random.Range(0, 101);
        if (ranNum <= probability)
        {
            amountRanNum = Random.Range(dropItemAmount_Min, dropItemAmount_Max + 1);
        }

        return amountRanNum;
    }
}