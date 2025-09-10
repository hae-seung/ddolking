using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CherryBlossom : PlayerSkill
{
    [SerializeField] private GameObject slashEffect;

    public float blossomTime;
    public float dieTime;
    public float x;//8.5
    public float y;//5.0

    private Vector3 originPos;

    private Collider2D[] hitBuffer;
    
    public override void Init(SkillData data)
    {
        this.data = data;
        hitBuffer = new Collider2D[data.HitCount];
    }
    
    public override void ActiveSkill()
    {
        originPos = transform.position;
        Vector3 pos = new Vector3(originPos.x + x, originPos.y + y, 0);
        transform.position = pos;
        
        StartCoroutine(StartSlash());
    }

    private IEnumerator StartSlash()
    {
        GameEventsManager.Instance.playerEvents.DisablePlayerMovement();
        GameEventsManager.Instance.playerEvents.PlayerNoHurt();
        GameEventsManager.Instance.inputEvents.DisableInput();
        
        yield return new WaitForSeconds(blossomTime);

        Instantiate(slashEffect, originPos, slashEffect.transform.rotation);

        yield return new WaitForSeconds(0.4f);
        Attack();
        
        StartCoroutine(DestroyRoutine());
    }

    private void Attack()
    {
        StartCoroutine(MakeNoise());
        
        Vector2 center = originPos;
        Vector2 size = new Vector2(2 * x, 2 * y);
        float angle = 0f;

        int count = Physics2D.OverlapBoxNonAlloc(
            center,
            size,
            angle,
            hitBuffer,
            LayerMask.GetMask("Monster"));

        for (int i = 0; i < count; i++)
        {
            Collider2D col = hitBuffer[i];
            if (col != null)
            {
                IDamageable dmg = col.GetComponent<IDamageable>();
                
                for(int j = 0; j<data.AttackCount; j++)
                    dmg?.OnDamage(CalculateDamage(false), false);
            }
        }
    }

    private IEnumerator MakeNoise()
    {
        CinemachineVirtualCamera vcam = VirtualCameraManager.Instance.GetCamera();
        if (vcam == null)
        {
            Debug.Log("vcam is null");
            yield break;
        }

        var perlin = vcam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        if (perlin == null)
        {
            Debug.Log("perlin is null");
            yield break;
        }

        perlin.m_AmplitudeGain = 2f; //흔들림 세기
        perlin.m_FrequencyGain = 3f; // 흔들림 속도

        yield return new WaitForSeconds(dieTime);

        perlin.m_FrequencyGain = 0f;
        perlin.m_AmplitudeGain = 0f;
    }

    private IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(dieTime);
        GameEventsManager.Instance.playerEvents.EnablePlayerMovement();
        GameEventsManager.Instance.playerEvents.PlayerYesHurt();
        GameEventsManager.Instance.inputEvents.EnableInput();
        Destroy(gameObject);
    }
    
}
