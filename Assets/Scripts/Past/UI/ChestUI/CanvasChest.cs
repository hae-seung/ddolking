using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasChest : MonoBehaviour
{
    [Header("게임 시작하고 tab 끄는 용도")]
    [SerializeField] private GameObject chestTab;


    private void Start()
    {
        chestTab.SetActive(false);
    }
}
