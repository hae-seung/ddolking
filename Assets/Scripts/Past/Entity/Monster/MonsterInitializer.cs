using System;
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
        // 필드에 할당
        agent = GetComponent<NavMeshAgent>();
        _monster = GetComponent<Monster>();
        _controller = GetComponent<MonsterController>();
        _monsterFsm = GetComponent<MonsterFSM>();
        
        // Monster와 Controller 초기화
        _monster.Init(agent, _controller);
        _controller.Init(_monster);
        _monsterFsm.Init(_monster, _controller);

        // NavMeshAgent 활성화 시점 보장
        if (agent != null)
        {
            agent.enabled = true;
        }
    }
    
}
