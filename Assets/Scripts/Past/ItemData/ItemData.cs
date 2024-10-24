using UnityEngine;
public class ItemData : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [Multiline]
    [SerializeField] private string _description;
    [SerializeField] private Sprite _iconImage;
    [Tooltip("인벤토리에서 버릴때 생성시킬 오브젝트")]
    [SerializeField] private GameObject dropObjectPrefab;
    
    public int ID => _id;
    public string Name => _name;
    public string Description => _description;
    public Sprite IconImage => _iconImage;
    public GameObject DropObjectPrefab => dropObjectPrefab;
}
