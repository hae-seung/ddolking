using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignBehaviour : InteractionBehaviour
{
    [SerializeField] private int needLevel;
    [SerializeField] private int needMoney;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private float highlightTime;

    private void Start()
    {
        _particleSystem.Stop();
    }

    protected override void Interact(Interactor interactor)
    {
        UIManager.Instance.OpenSignTab(needLevel, needMoney, UnlockSignQuest);
    }

    private void UnlockSignQuest()
    {
        //돈 차감
        PlayerWallet.Instance.SpendMoney(MoneyType.past, needMoney);
        
        //파티클 시작
        VirtualCameraManager.Instance.GetCamera(CameraType.highlight).SetActive(true);
        StartCoroutine(StartParticle());
    }

    private IEnumerator StartParticle()
    {
        yield return new WaitForSeconds(1.0f);
        _particleSystem.Play();
        GameEventsManager.Instance.playerEvents.DisablePlayerMovement();
        yield return new WaitForSeconds(highlightTime);
        VirtualCameraManager.Instance.GetCamera(CameraType.highlight).SetActive(false);
        GameEventsManager.Instance.playerEvents.EnablePlayerMovement();
        Destroy(gameObject);
    }
}
