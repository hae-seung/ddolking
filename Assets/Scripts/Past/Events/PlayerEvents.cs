using System;
using UnityEngine;

public class PlayerEvents
{
    public event Action onDisablePlayerMovement;
    public void DisablePlayerMovement()
    {
        onDisablePlayerMovement?.Invoke();
    }

    public event Action onEnablePlayerMovement;
    public void EnablePlayerMovement()
    {
        onEnablePlayerMovement?.Invoke();
    }

    public event Action<int, int> onAcquireItem;//아이템 획득 또는 이미 있는지
    public void AcquireItem(int amount, int itemId)
    {
        onAcquireItem?.Invoke(amount, itemId);
    }

    public event Action<int, int> onDisposeItem; //아이템이 인벤토리에서 사용되거나 나갈때
    public void DisposeItem(int amount, int itemId)
    {
        onDisposeItem?.Invoke(amount, itemId);
    }
    
    //todo: (경험치, 레벨)

    public event Action onUpdateStatusUI;
    public void UpdateStatusUI()
    {
        onUpdateStatusUI?.Invoke();
    }
}
