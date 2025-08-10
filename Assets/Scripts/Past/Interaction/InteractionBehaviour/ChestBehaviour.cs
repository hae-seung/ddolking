using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Chest
{
    //상자 안에 담기는 실제 객체 리스트
    private List<Item> items;
    
    public Chest(List<Item> datas)
    {
        items = new List<Item>(datas);
    }

    public Chest()
    {
        items = new List<Item>();
    }


    public int Add(Item item, int amount)
    {
        return -1;
    }
    
}







public class ChestBehaviour : InteractionBehaviour
{
    
    
    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
        
    }
}
