using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasChest : MonoBehaviour
{
    [Header("게임 시작하고 tab 끄는 용도")]
    [SerializeField] private ChestUI chestTab;

    private void Awake()
    {
        GameEventsManager.Instance.playerEvents.onAcquireItem += AcquireItem;
        chestTab.isOpen = false;
    }

    private void AcquireItem(int index)
    {
        chestTab.UpdateInvenSlot(index);
    }


    private void Start()
    {
        chestTab.gameObject.SetActive(false);
    }
}
