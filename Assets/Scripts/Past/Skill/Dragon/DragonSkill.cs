using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DragonSkill : PlayerSkill
{
    [SerializeField] private GameObject leftSlash;
    [SerializeField] private GameObject rightSlash;
    [SerializeField] private GameObject downSlash;

    public float aniTime;
    public float lastAniTime;
    public float dieTime;
    
    private Collider2D[] hitBuffer;
    
    public override void Init(SkillData data)
    {
        this.data = data;
        hitBuffer = new Collider2D[data.HitCount/3];
    }
    
    
    public override void ActiveSkill()
    {
        GameEventsManager.Instance.playerEvents.DisablePlayerMovement();
        GameEventsManager.Instance.playerEvents.PlayerNoHurt();
        GameEventsManager.Instance.inputEvents.DisableInput();
        StartCoroutine(StartSlash());
    }

    private IEnumerator StartSlash()
    {
        yield return new WaitForSeconds(aniTime);
        
        //1타
        Instantiate(leftSlash,
            transform.position, 
            leftSlash.transform.rotation);
        Attack();
        StartCoroutine(MakeNoise(0.25f));
        yield return new WaitForSeconds(aniTime);
        
        //2타
        Instantiate(rightSlash,
            transform.position,
            rightSlash.transform.rotation);
        Attack();
        StartCoroutine(MakeNoise(0.25f));
        yield return new WaitForSeconds(aniTime);
        
        
        //3타
        Instantiate(downSlash,
            transform.position,
            downSlash.transform.rotation);
        Attack();
        StartCoroutine(MakeNoise(lastAniTime));
        yield return new WaitForSeconds(lastAniTime);
        

        StartCoroutine(DestroyRoutine());
    }

    private IEnumerator MakeNoise(float time)
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

        yield return new WaitForSeconds(time);

        perlin.m_FrequencyGain = 0f;
        perlin.m_AmplitudeGain = 0f;
    }

    private void Attack()
    {
        Debug.Log("공격");
        Vector2 center = transform.position;
        Vector2 size = new Vector2(17f, 10f);
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
    
    private IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(dieTime);
        GameEventsManager.Instance.playerEvents.EnablePlayerMovement();
        GameEventsManager.Instance.playerEvents.PlayerYesHurt();
        GameEventsManager.Instance.inputEvents.EnableInput();
        Destroy(gameObject);
    }

}
