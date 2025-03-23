using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CutManager : MonoBehaviour
{
    public static CutManager Instance;
    
    [SerializeField] private DOTweenAnimation cameraTween;
    [SerializeField] private GameObject cut0;

    [SerializeField] private Subtitle subtitle;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }
    
    
    private void Start()
    {
        cameraTween.DOPlayForwardAllById("start");
        Invoke(nameof(PlayCutScene), 0.5f);
    }

    private void PlayCutScene()//0번 컷씬을 컷매니저가 실행시키면서 컷씬 시작
    {
        cut0.SetActive(true);//AutoPlay로 실행될거임
    }

    public void ShowText(string text)
    {
        subtitle.gameObject.SetActive(true);
        subtitle.ShowText(text);
    }
}
