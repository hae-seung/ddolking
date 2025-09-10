using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_Teleport : MonoBehaviour
{
    [SerializeField] private FieldShortCut fieldShortCut;
    
    private void Start()
    {
        fieldShortCut.gameObject.SetActive(false);
        fieldShortCut.Init();
    }

    public void OpenFieldShortCut(Interactor player)
    {
        fieldShortCut.Open(player);
    }

    public void RegisterFieldShortCut(VillageType type)
    {
        fieldShortCut.RegisterShortCut(type);
    }
}
