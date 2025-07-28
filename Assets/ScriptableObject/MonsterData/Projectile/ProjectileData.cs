using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ProjectileData", menuName = "SO/Entity/Projectile")]
public class ProjectileData : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private GameObject projectilePrefab;


    public int Id => id;
    public GameObject Prefab => projectilePrefab;
}
