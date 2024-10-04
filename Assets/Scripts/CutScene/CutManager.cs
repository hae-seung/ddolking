using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement; // 씬 전환을 위해 추가

[System.Serializable]
public class Scene
{
    public GameObject cutScene;
    public GameObject panel;
    public GameObject[] texts;
}

public class CutManager : MonoBehaviour
{
    public List<Scene> cutScenes;
    private int curSceneNum = 0;
    private int curTextNum = 0;
    private bool isAnimating = false; // 텍스트 애니메이션 진행 중 여부를 나타내는 플래그

    public void Start()
    {
        // DOTween 초기화
        DOTween.Init();
    }

    public void Update()
    {
        // 애니메이션이 진행 중이 아니면 입력을 받아 다음으로 진행
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && !isAnimating)
        {
            if(curSceneNum >= cutScenes.Count -1 )
            {
                SceneManager.LoadScene("World");
                return;
            }
            // 모든 씬을 처리했다면 다음 씬 로드
            StartCoroutine(ProgressCutScene()); // 코루틴으로 텍스트 출력
        }
    }

    private IEnumerator ProgressCutScene()
    {
        // 현재 씬 인덱스가 리스트 범위를 초과하지 않는지 확인
        if (curSceneNum >= cutScenes.Count) {
            yield break; // 범위를 벗어났다면 코루틴을 종료
        }

        if (!cutScenes[curSceneNum].panel.activeSelf)
            cutScenes[curSceneNum].panel.SetActive(true);

        // 이전 텍스트를 비활성화
        if (curTextNum > 0 && curTextNum < cutScenes[curSceneNum].texts.Length && cutScenes[curSceneNum].texts[curTextNum - 1] != null)
            cutScenes[curSceneNum].texts[curTextNum - 1].SetActive(false);

        // 현재 텍스트를 활성화
        if (curTextNum < cutScenes[curSceneNum].texts.Length)
        {
            GameObject currentTextObject = cutScenes[curSceneNum].texts[curTextNum];
            if (currentTextObject != null)
            {
                currentTextObject.SetActive(true);
                Text textComponent = currentTextObject.GetComponent<Text>();
                if (textComponent != null)
                {
                    string fullText = textComponent.text;
                    textComponent.text = "";
                    isAnimating = true;
                    yield return textComponent.DOText(fullText, 2.3f).WaitForCompletion();
                }
                curTextNum++;
            }
        }
        else
        {
            // 마지막 텍스트를 처리한 후 다음 씬으로 이동
            if (curSceneNum < cutScenes.Count - 1)
            {
                DOTweenAnimation doTweenAnimation = cutScenes[curSceneNum].cutScene.GetComponent<DOTweenAnimation>();
                if(doTweenAnimation != null)
                    doTweenAnimation.DORestartById("0");
                
                curSceneNum++;
                curTextNum = 0; // 다음 씬의 첫 텍스트로 초기화
                
                doTweenAnimation = cutScenes[curSceneNum].cutScene.GetComponent<DOTweenAnimation>();
                if(doTweenAnimation != null)
                    doTweenAnimation.DORestartById("1");
            }
        }

        isAnimating = false;
    }

}
