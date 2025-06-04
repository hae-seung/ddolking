using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CraftTableLevel
{
    public CraftManualType type;
    public int level;
    public bool isActive = false;
}


public class ReinforceManager : MonoBehaviour
{
    public static ReinforceManager Instance;
    
    //구조물 강화
    [SerializeField] private CraftReinforceSO craftReinforceSo;
    private Dictionary<int, CraftTableLevel> craftTables = new(); //id, 구조물
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

    public int RegisterStructure(CraftManualType type) 
    {
        //강화된 구조물이 파괴되고 다시 설치되면 데이터 이전 필요
        for (int i = 0; i < craftTables.Count; i++)
        {
            if (!craftTables[i].isActive && craftTables[i].type == type)
            {
                craftTables[i].isActive = true;
                return i;
            }
        }
        
        //완전히 새로운 구조물
        int lastNum = craftTables.Count;
        
        CraftTableLevel craftTableLevel = new CraftTableLevel
        {
            //초기화
            type = type,
            level = 0,
            isActive = true
        };

        //딕셔너리 등록
        craftTables[lastNum] = craftTableLevel;
        
        return lastNum;
    }

    public void UnRegisterStructure(int id)
    {
        craftTables[id].isActive = false;
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
        return craftTables[id].level;
    }

    public ItemData GetReinforceNeedItemData(int level)
    {
        return reinforceData[level];
    }

    public void LevelUp(int id)
    {
        craftTables[id].level++;
    }
}
