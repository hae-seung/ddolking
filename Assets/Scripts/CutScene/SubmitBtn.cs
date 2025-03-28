using System;
using DG.Tweening;
using UnityEngine;


public class SubmitBtn : MonoBehaviour
{
    private DOTweenAnimation nextDoTweenAnimation;
    private string nextSceneName;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(!gameObject.activeSelf)
            return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            nextDoTweenAnimation.DOPlayForwardById(nextSceneName);
            gameObject.SetActive(false);
        }
    }


    public void RegisterNextDotWeenObject(GameObject nextObject)
    {
        gameObject.SetActive(true);
        nextDoTweenAnimation = nextObject.GetComponent<DOTweenAnimation>();
    }

    public void RegisterNextSceneName(string sceneName)
    {
        nextSceneName = sceneName;
    }

  
    
}
