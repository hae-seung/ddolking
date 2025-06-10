
using UnityEngine;

public class ReinforceStructureItem : EstablishItem
{
    //etcItem과 etcItemData는 공유하되 객체만 다름.

    private int structureId;
    
    public int StructureId => structureId;
    
    
    protected override CountableItem CreateItem()
    {
        //굳이 클론으로 복제할 필요 없음. 오버헤드만 발생함.
        //어차피 갯수는 1개만 가지고 있을거고, Amount만 1씩 다시 복구 시키면 무한 사용 가능.
        //structureId만 달라지면서
        //실제로 한개의 실제 설치 구조물에 일대일 대응으로 한개의 객체만 담는셈임.
        //new로 객체 생성은 OpenCraftTabBehaviour에서 진행
        return this;
    }

    public void SetStructureId(int newId)
    {
        structureId = newId;
    }

    public ReinforceStructureItem(EstablishItemData data, int amount = 1) : base(data, amount)
    {
        structureId = -1;
    }
    
}

