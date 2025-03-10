using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        GameEventsManager.Instance.playerEvents.onPlayAnimation += PlayAnimation;
    }

    private void PlayAnimation(string parameter, bool state)
    {
        animator.SetBool(parameter, state);
    }
}
