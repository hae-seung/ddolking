using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ReinforceManager : MonoBehaviour
{
    public static ReinforceManager Instance;
    
    //구조물 강화
    [SerializeField] private CraftReinforceSO craftReinforceSo;
    private Dictionary<int, int> craftTables = new(); //id, 레벨
    private Dictionary<int, Sprite> toolImages = new();//level, sprite
    private Dictionary<int, float> efficient = new();//level, float
    private Dictionary<int, ItemData> reinforceData = new();//level itemData

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }

        InitStructureData();
    }
    

    private void InitStructureData()
    {
        for (int i = 0; i <= craftReinforceSo.MaxLevel; i++)
        {
            toolImages[i] = craftReinforceSo.ToolImages[i];
            efficient[i] = craftReinforceSo.Efficient[i];
            reinforceData[i] = craftReinforceSo.ReinforceDatas[i]; 
        }
    }

    public int RegisterStructure(int id)
    {
        Debug.Log($"{id}를 등록할려 합니다");
        
        if (craftTables.ContainsKey(id))
            return id;
        
        
        int lastNum = craftTables.Count;
        craftTables[lastNum] = 0; //레벨 0으로 초기화
        
        return lastNum;
    }
    
    public Sprite GetToolImage(int level)
    {
        return toolImages[level];
    }

    public float GetEfficient(int level)
    {
        return efficient[level];
    }

    public int GetCraftLevel(int id)
    {
        return craftTables[id];
    }

    public ItemData GetReinforceNeedItemData(int level)
    {
        return reinforceData[level];
    }

    public void LevelUp(int id)
    {
        craftTables[id]++;
    }
}
