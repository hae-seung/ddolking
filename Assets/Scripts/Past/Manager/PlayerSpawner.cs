using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Interactor player;
    [SerializeField] private AudioClip pastBgm;
    
    private void Start()
    {
        SpawnPlayer();
        PlayBgm();
    }

    private void PlayBgm()
    {
       AudioManager.Instance.PlayBgm(pastBgm);
    }


    private void SpawnPlayer()
    {
        player.transform.position = spawnPoint.position;
    }
}
