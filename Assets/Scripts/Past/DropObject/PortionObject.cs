using UnityEngine;

public class PortionObject : CountableObject<PortionItem>
{
    [SerializeField] private PortionItemData data;

    protected override void MakeItemInstance()//풀이 아닌 최초 생성된 경우
    {
        countableItem = new PortionItem(data);
        itemId = data.ID;
    }

    protected override void Awake() //풀이 아닌 최초 생성된 경우
    {
        base.Awake();
        dropObjectPrefab = data.DropObjectPrefab;  // DropObject 프리팹 설정
    }

    protected override void OnEnable()
    {
        if (!isSpawned)  // 씬에 배치된 경우 최초 활성화 시 작업 방지
        {
            isSpawned = true;
            return;
        }

        Debug.Log("PortionEnable 실행");
        countableItem = new PortionItem(data);
        base.OnEnable();
    }
    
}