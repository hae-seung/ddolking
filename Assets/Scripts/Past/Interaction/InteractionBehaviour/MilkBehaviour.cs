using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkBehaviour : FeedingInteraction
{
    [Header("빈병 아이템")]
    [SerializeField] private CountableItemData emptyBottle;

    [Header("우유 아이템")] 
    [SerializeField] private CountableItemData milk;
    
    
    
    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
       base.Interact(interactor, currentGripItem);

       if (currentGripItem.itemData == emptyBottle)
       {
           int total = Inventory.Instance.GetItemTotalAmount(milk);
           
           //우유가 인벤에 없음 => 새로운 칸 탐색
           if (total == 0)
           {
                CheckInventoryAndGiveMilk();
           }
           //우유가 인벤에 1칸이라도 들어있음
           else
           {
               if (total % milk.MaxAmount > 0)
               {
                   //우유가 인벤토리에 있고 갯수가 MAX까지 안찍혓을때
                   GiveMilk();
               }
               else
               {
                   //우유가 모든 칸에 맥스치로 찍혀있음 => 새로운 칸 탐색
                   CheckInventoryAndGiveMilk();
               }
           }
       }
    }

    private void CheckInventoryAndGiveMilk()
    {
        //인벤토리 남는 칸이 없을 때
        if (Inventory.Instance.GetEmptySlotAmount() == 0)
        {
            return;
        }
               
        //한칸이라도 남는칸이 있다면
        GiveMilk();
    }
    
    private void GiveMilk()
    {
        Item milkItem = milk.CreateItem();
        Inventory.Instance.RemoveItem(emptyBottle, 1);
        Inventory.Instance.Add(milkItem, 1);
    }
}
