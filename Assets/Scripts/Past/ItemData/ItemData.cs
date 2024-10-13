using UnityEngine;
public class ItemData : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [Multiline]
    [SerializeField] private string _description;
    [SerializeField] private Sprite _iconImage;
    [SerializeField] private GameObject dropObjectPrefab;
    
    public int ID => _id;
    public string Name => _name;
    public string Description => _description;
    public Sprite IconImage => _iconImage;
}
