using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("BGM")]
    private AudioSource bgmPlayer;

    [Header("SFX")] //UI 같은 2D 사운드
    private GameObject sfxObject;
    private int channels = 30;
    private List<AudioSource> sfxPlayers;
    private int channelIndex = 0;

    public bool isFinishInit { get; private set; }
    
    protected override void Awake()
    {
        isFinishInit = false;
        base.Awake();
        Init();
    }

    private void Init()
    {
        InitBGM();
        InitSFX();
    }

    private void Start()
    {
        InitVolume();
    }

    private void InitVolume()
    {
        // 데이터에 저장된 볼륨 값 적용
        audioMixer.SetFloat("MasterSound", DataManager.Instance.settingOption.MasterSound);
        audioMixer.SetFloat("SfxSound", DataManager.Instance.settingOption.SFXSound);
        audioMixer.SetFloat("BgmSound", DataManager.Instance.settingOption.BGMSound);
        isFinishInit = true;
    }

    private void InitSFX()
    {
        sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new List<AudioSource>();

        for (int i = 0; i < channels; i++)
        {
            AudioSource newAudioSource =  sfxObject.AddComponent<AudioSource>();
            newAudioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sfx")[0];
            newAudioSource.playOnAwake = false;
            sfxPlayers.Add(newAudioSource);
        }
    }

    private void InitBGM()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Bgm")[0];
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.clip = null;
    }

    public void PlayBgm(AudioClip audioClip)
    {
        StopAndPlayBgm(audioClip);
    }

    public void PlaySfx(AudioClip audioClip)
    {
        bool isPlayed = false;
        for (int i = 0; i < sfxPlayers.Count; i++)
        {
            int loopIndex = (i + channelIndex) % sfxPlayers.Count;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[channelIndex].clip = audioClip;
            sfxPlayers[channelIndex].Play();
            isPlayed = true;
            break;
        }

        if (!isPlayed)
        {
            channels++;
            
            //새로 추가
            AudioSource newAudioSource = sfxObject.AddComponent<AudioSource>();
            newAudioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sfx")[0];
            newAudioSource.playOnAwake = false;
            sfxPlayers.Add(newAudioSource);

            //실행
            newAudioSource.clip = audioClip;
            newAudioSource.Play();
        }
    }

    public void StopAndPlayBgm(AudioClip clip)
    {
        if (bgmPlayer.isPlaying)
        {
            StartCoroutine(FadeOutAndStopBgm(clip));
        }
        else
        {
            bgmPlayer.clip = clip;
            bgmPlayer.Play();
        }
    }

    private IEnumerator FadeOutAndStopBgm(AudioClip clip)
    {
        float startVolume = bgmPlayer.volume;
        float fadeDuration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            bgmPlayer.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        bgmPlayer.Stop();

        bgmPlayer.clip = clip;
        bgmPlayer.volume = startVolume;
        bgmPlayer.Play();
    }

    public void ChangeVolume(SettingOption.Sound sound, float value)
    {
        float dBValue = Mathf.Log10(value) * 20;
        
        switch (sound)
        {
            case SettingOption.Sound.Master:
                audioMixer.SetFloat("MasterSound", dBValue);
                DataManager.Instance.settingOption.SetMasterSound(dBValue); // 저장
                break;
            case SettingOption.Sound.Bgm:
                audioMixer.SetFloat("BgmSound", dBValue);
                DataManager.Instance.settingOption.SetBgmSound(dBValue); // 저장
                break;
            case SettingOption.Sound.Sfx:
                audioMixer.SetFloat("SfxSound", dBValue);
                DataManager.Instance.settingOption.SetSfxSound(dBValue); // 저장
                break;
        }
    }
}
