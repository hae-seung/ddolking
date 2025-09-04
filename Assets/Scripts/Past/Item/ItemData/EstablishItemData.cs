using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EstablishItemData", menuName = "SO/CountableItemData/Establish")]
public class EstablishItemData : CountableItemData
{
    //대표 예시 : 씨앗
    [Header("실제 설치할 필드오브젝트의 데이터")]
    [SerializeField] private FieldObjectData establishObjectData; 
    
    [Header("설치하기 전 나타날 프리뷰")] 
    [SerializeField] private GameObject establishObjectPreview;

    [Header("설치할 레이어")] 
    [SerializeField] private LayerMask targetLayer;
    
    
    public FieldObjectData EstablishObjectData => establishObjectData;
    public GameObject PreviewObject => establishObjectPreview;
    public LayerMask TargetLayer => targetLayer;
    
    
    public override Item CreateItem()
    {
        return new EstablishItem(this);
    }
}
