using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnSoundEvent : MonoBehaviour
{
    [SerializeField] private AudioClip buttonClickAudioClip;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        ConnectEvent();
    }

    private void ConnectEvent()
    {
        button.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySfx(buttonClickAudioClip);
        });
    }
}
