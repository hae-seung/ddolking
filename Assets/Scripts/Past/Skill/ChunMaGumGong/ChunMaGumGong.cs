using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ChunMaGumGong : PlayerSkill
{
    [Header("1.카메라 소환으로 렌더 텍스쳐 업데이트")]
    [SerializeField] private GameObject camera;

    [Header("2.찢어질 화면들 소환")] 
    [SerializeField] private GameObject moveParent;

    [Header("3.천마검공 한자 소환")]
    [SerializeField] private List<Sprite> words;
    [SerializeField] private GameObject WordImage;//소환된 오브젝트에 스프라이트 할당
    [SerializeField] private float wordDelay;

    [Header("4.슬래시 이펙트 소환")]
    [SerializeField] private GameObject slashEffect;
    [SerializeField] private float slashDelay;
    
    [Header("5.가르기 이펙트 소환")]
    [SerializeField] private GameObject cutEffect;
    [SerializeField] private float cutDelay;

    
    [Header("베기 다음 컷이 나오기까지의 딜레이")]
    [SerializeField] private float slashTerm;

    [Header("잘리는 화면 이펙트 종료대기")] [SerializeField]
    private float cuttingDelay;
    
    private Collider2D[] hitBuffer;
    
    //한번에 지우기 용
    private List<GameObject> spawnLists;

    private DOTweenAnimation ani;
    private DOTweenAnimation ani1;
    private DOTweenAnimation ani2;
    
    public override void Init(SkillData data)
    {
        this.data = data;
        hitBuffer = new Collider2D[data.HitCount/2];
        spawnLists = new List<GameObject>();
    }
    
    public override void ActiveSkill()
    {
        GameEventsManager.Instance.playerEvents.DisablePlayerMovement();
        GameEventsManager.Instance.playerEvents.PlayerNoHurt();
        GameEventsManager.Instance.inputEvents.DisableInput();
        
        SpawnCamera();
        SpawnMoveScreens();
        StartCoroutine(RealEffect());
    }

    private void SpawnCamera()
    {
        GameObject cam = Instantiate(camera);
        Vector3 newPos = transform.position;
        newPos.z = -10f;

        cam.transform.position = newPos;
        
        spawnLists.Add(cam);
    }
    
    private void SpawnMoveScreens()
    {
        GameObject go = Instantiate(moveParent, UIManager.Instance.transform);
        ani = go.GetComponent<DOTweenAnimation>();
        spawnLists.Add(go);

        ani1 = go.transform.GetChild(0).GetComponent<DOTweenAnimation>();
        ani2 = go.transform.GetChild(1).GetComponent<DOTweenAnimation>();
    }
    
    private IEnumerator RealEffect()
    {
        for(int i = 0; i<words.Count; i++)
        {
            SpawnWords(i);
            yield return new WaitForSeconds(wordDelay);
        }

        SpawnSlash();
        Attack();
        ani.DOPlay();
        yield return new WaitForSeconds(slashDelay);
        
        yield return new WaitForSeconds(slashTerm); // 화면을 베는 이펙트가 나오기 까지 대기
        SpawnCutSlash();
        yield return new WaitForSeconds(cutDelay);//화면을 베는 이펙트 종료까지 대기
        Attack();
        ani1.DOPlay();
        ani2.DOPlay();

        yield return new WaitForSeconds(cuttingDelay);
        RemoveObjects();
    }

    private void RemoveObjects()
    {
        for (int i = 0; i < spawnLists.Count; i++)
        {
            if(spawnLists[i] != null)
                Destroy(spawnLists[i]);
        }
        
        GameEventsManager.Instance.playerEvents.EnablePlayerMovement();
        GameEventsManager.Instance.playerEvents.PlayerYesHurt();
        GameEventsManager.Instance.inputEvents.EnableInput();
        Destroy(gameObject);
    }

    private void SpawnCutSlash()
    {
        GameObject go = Instantiate(cutEffect, transform.position, cutEffect.transform.rotation);
        go.transform.localScale = cutEffect.transform.localScale;
        spawnLists.Add(go);

        Vector3 newPos = new Vector3(transform.position.x + 0.98f, transform.position.y - 1.2f, 0f);
        go.transform.position = newPos;
    }

    private void SpawnSlash()
    {
        GameObject go = Instantiate(slashEffect, transform.position, slashEffect.transform.rotation);
        go.transform.localScale = slashEffect.transform.localScale;
        spawnLists.Add(go);
    }

    private void SpawnWords(int i)
    {
        GameObject go = Instantiate(WordImage, UIManager.Instance.transform);
        Image image = go.GetComponent<Image>();
        image.sprite = words[i];

        RectTransform rt = go.GetComponent<RectTransform>();

        switch (i % 4) // 0~3 반복
        {
            case 0: // 좌상단
                rt.anchorMin = new Vector2(0f, 1f);
                rt.anchorMax = new Vector2(0f, 1f);
                rt.pivot = new Vector2(0f, 1f);
                break;

            case 1: // 우상단
                rt.anchorMin = new Vector2(1f, 1f);
                rt.anchorMax = new Vector2(1f, 1f);
                rt.pivot = new Vector2(1f, 1f);
                break;

            case 2: // 좌하단
                rt.anchorMin = new Vector2(0f, 0f);
                rt.anchorMax = new Vector2(0f, 0f);
                rt.pivot = new Vector2(0f, 0f);
                break;

            case 3: // 우하단
                rt.anchorMin = new Vector2(1f, 0f);
                rt.anchorMax = new Vector2(1f, 0f);
                rt.pivot = new Vector2(1f, 0f);
                break;
        }

        rt.anchoredPosition = Vector2.zero; // anchor 기준 위치
        spawnLists.Add(go);
    }

    private void Attack()
    {
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

    
}
