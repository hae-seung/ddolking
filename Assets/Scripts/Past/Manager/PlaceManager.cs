using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceManager : MonoBehaviour
{
    public static PlaceManager Instance;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Interactor player;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        player.transform.position = spawnPoint.position;
    }
}
