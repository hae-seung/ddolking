using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalFSM : MonoBehaviour
{
    private Animal animal;
    private float idleTime;
    private float walkTime;
    private bool isWalk = false;
    private float timer;
    
    
    
    
    private void Awake()
    {
        animal = GetComponent<Animal>();

        idleTime = GetIdleTime();
        walkTime = GetWalkTime();
        timer = 0f;
    }

    private void Update()
    {
        if (animal.IsDead)
            return;
        
        timer += Time.deltaTime;

        if (isWalk)
        {
            if (timer >= walkTime)
            {
                SetStop();
            }
            else
            {
                animal.SetMove();
            }
        }
        else
        {
            if (timer >= idleTime)
            {
                SetMove();
            }
        }
    }

    private void SetMove()
    {
        isWalk = true;
        walkTime = GetWalkTime();
        timer = 0f;
        animal.MoveRandom();
    }
    

    private void SetStop()
    {
        isWalk = false;
        idleTime = GetIdleTime();
        timer = 0f;
        animal.StopMove();
    }

    private float GetIdleTime()
    {
        return Random.Range(7f, 10f);
    }

    private float GetWalkTime()
    {
        return Random.Range(5f, 8f);
    }
}
