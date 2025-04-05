using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TransitionUI : MonoBehaviour
{
    public static TransitionUI Instance;
    
    private DOTweenAnimation _doTweenAnimation;
    [SerializeField] private GameObject side1;
    [SerializeField] private GameObject side2;
    [SerializeField] private AudioClip transitionSfx;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }

        _doTweenAnimation = side1.GetComponent<DOTweenAnimation>();
    }

    public void EnableTransitionUI()
    {
        AudioManager.Instance.PlaySfx(transitionSfx);
        side1.SetActive(true);
        side2.SetActive(true);
        _doTweenAnimation.DORestartAllById("side");
    }
}
