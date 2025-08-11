
using System;
using UnityEngine;

public abstract class RebuildItem
{
    
}


public class ReinforceStructureItem : RebuildItem
{
    private ReinforceStructureData data;

    private int level;


    public ReinforceStructureItem(ReinforceStructureData data)
    {
        this.data = data;
        level = 0;
    }

    public void LevelUp()
    {
        level = Math.Min(level + 1, 3);
    }

    public int GetLevel()
    {
        return level;
    }
    
    public float GetEfficient(int level)
    {
        return data.Efficient[level];
    }

    public Sprite GetSprite(int level)
    {
        return data.ToolImages[level];
    }

    public ItemData GetNextLevelNeedItem(int level)
    {
        return data.ReinforceDatas[level];
    }
    
}

