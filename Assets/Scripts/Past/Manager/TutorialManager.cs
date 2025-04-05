using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    public bool watchTutorial;
    [Header("기본구성")]
    [SerializeField] private GameObject tutorialCamera;
    [SerializeField] private GameObject houseCamera;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject tutorialMan;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private TalkSessionData tutorialTalkSessionData;

    [Header("튜토리얼 카메라 이동설정")] 
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;
    [SerializeField] private float moveDuration;

    [Header("사운드")] 
    private AudioSource audioSource;
    
    
    
    private DOTweenAnimation doTweenAnimation;//튜토리얼맨
    
    private CinemachineTransposer transposer;
    private bool isResetPos = false;
    private Vector3 originOffset;
    
    
    
    private List<TalkData> datas;
    private int index = 0;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
        
        tutorialMan.SetActive(false);
        doTweenAnimation = tutorialMan.GetComponent<DOTweenAnimation>();
        audioSource = GetComponent<AudioSource>();
        datas = tutorialTalkSessionData.talkDatas;
    }
    

    public void StartTutorial()
    {
        if (!watchTutorial)
            return;
        GameEventsManager.Instance.playerEvents.DisablePlayerMovement();
        GameEventsManager.Instance.inputEvents.DisableInput();
        
        StartCoroutine(InvokeTutorial());
    }

    private IEnumerator InvokeTutorial()
    {
        yield return new WaitForSeconds(0.6f);
        houseCamera.SetActive(false);
        tutorialCamera.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        tutorialMan.SetActive(true);
        
        
        
        StartTalk();
        yield return new WaitUntil(() => !TalkManager.Instance.isTalk);//대사 : 이제 일어난건가?
        
        player.FlipCharacter(-1.0f);
        yield return new WaitForSeconds(1.0f);
        player.FlipCharacter(1.0f); //캐릭터 플립
        Animator animator = tutorialMan.GetComponent<Animator>();
        
        animator.SetBool("isWalk", true);//튜토리얼맨 등장
        audioSource.Play();
        tutorialMan.transform.DOLocalMove(new Vector3(0.42f, 2.15f, 0f), 3f);
        yield return new WaitForSeconds(3.2f);
        audioSource.Stop();
        animator.SetBool("isWalk", false);//튜토리얼맨 정지
        
        StartTalk(); //대사 : 자네는 저기 있는 악마왕이 소환환 게이트를 통해 이곳으로 온거 같더군
        yield return new WaitForSeconds(1.0f);
        ApplyCinemachineEffect();
        yield return new WaitUntil(() => !TalkManager.Instance.isTalk);
        isResetPos = true;
        
        
        StartTalk();
        yield return new WaitUntil(() => !TalkManager.Instance.isTalk);
        
        StartTalk();
        yield return new WaitUntil(() => !TalkManager.Instance.isTalk);
        
        StartTalk();
        yield return new WaitUntil(() => !TalkManager.Instance.isTalk);
        
        StartTalk();
        yield return new WaitUntil(() => !TalkManager.Instance.isTalk);
        
        StartTalk();
    }

    private void ApplyCinemachineEffect()
    {
        transposer = tutorialCamera.GetComponent<CinemachineVirtualCamera>()
            .GetCinemachineComponent<CinemachineTransposer>();
        
        originOffset = transposer.m_FollowOffset;
        Vector3 targetOffset = originOffset + new Vector3(offsetX, offsetY, 0);
        CinemachineVirtualCamera cam = tutorialCamera.GetComponent<CinemachineVirtualCamera>();
        cam.m_Lens.OrthographicSize = 5;
        
        
        //이동
        DOTween.To(
            () => transposer.m_FollowOffset,
            x => transposer.m_FollowOffset = x,
            targetOffset,
            moveDuration
        ).OnComplete(() =>
        {
            StartCoroutine(WaitAndResetCamera(cam));
        });
    }
    
    private IEnumerator WaitAndResetCamera(CinemachineVirtualCamera cam)
    {

        // 대화 끝날 때까지 대기
        yield return new WaitUntil(()=> isResetPos);

        // 원래 위치로 복귀
        DOTween.To(
            () => transposer.m_FollowOffset,
            x => transposer.m_FollowOffset = x,
            originOffset,
            moveDuration
        );

        // 줌 복귀
        cam.m_Lens.OrthographicSize = 3;
    }


    private void StartTalk()
    {
        if (index >= datas.Count)
        {
            TalkManager.Instance.HidePanel();
            GameEventsManager.Instance.playerEvents.EnablePlayerMovement();
            GameEventsManager.Instance.inputEvents.EnableInput();
            tutorialCamera.SetActive(false);
            mainCamera.SetActive(true);
            return;
        }

        TalkManager.Instance.StartShortTalk(datas[index]);
        index++;
    }

}
