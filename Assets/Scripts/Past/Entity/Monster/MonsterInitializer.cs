using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterInitializer : MonoBehaviour
{
    private NavMeshAgent agent;
    private Monster _monster;
    private MonsterController _controller;
    private MonsterFSM _monsterFsm;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _monster = GetComponent<Monster>();
        _controller = GetComponent<MonsterController>();
        _monsterFsm = GetComponent<MonsterFSM>();
        
        
        _monster.Init(agent, _controller);
        _monsterFsm.Init(_monster, _controller);
        _controller.Init(_monster);
    }



}
