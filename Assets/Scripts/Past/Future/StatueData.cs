using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Statue", menuName = "SO/Statue")]
public class StatueData : ScriptableObject
{
    [SerializeField] private Sprite statueImage;
    [TextArea] [SerializeField] private string name;
    [TextArea] [SerializeField] private string explain;
    [SerializeField] private string day;

    
    public Sprite StatueImage => statueImage;
    public string Name => name;
    public string Explain => explain;
    public string Day => day;
}
