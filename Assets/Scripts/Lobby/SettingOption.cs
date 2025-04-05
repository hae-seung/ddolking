using UnityEngine;

public class SettingOption
{
    public enum Resolution
    {
        p480,
        p720,
        p1080,
        p1440
    };

    public enum Sound
    {
        Master,
        Bgm,
        Sfx
    }
    
    public FullScreenMode screenMode { get; private set; }
    public Resolution resolution { get; private set; }
    
    
    //로그로 변형된 값들이 저장됨
    public float MasterSound { get; private set; }
    public float SFXSound { get; private set; }
    public float BGMSound { get; private set; }

    
    //sound settings (Mixer의 값들)
    
    private void SaveSound()
    {
        PlayerPrefs.SetFloat("MasterSound", MasterSound);
        PlayerPrefs.SetFloat("BGMSound", BGMSound);
        PlayerPrefs.SetFloat("SFXSound", SFXSound);
    }

    private void LoadSound()
    {
        float value = Mathf.Log10(0.5f) * 20;//값이 없던 경우 초기화
        
        BGMSound = PlayerPrefs.GetFloat("BGMSound", value);
        SFXSound = PlayerPrefs.GetFloat("SFXSound", value);
        MasterSound = PlayerPrefs.GetFloat("MasterSound", value);
    }

    // 볼륨 저장 메서드 (AudioManager에서 호출)
    public void SetMasterSound(float value)
    {
        MasterSound = value;
        SaveSound();
    }

    public void SetBgmSound(float value)
    {
        BGMSound = value;
        SaveSound();
    }

    public void SetSfxSound(float value)
    {
        SFXSound = value;
        SaveSound();
    }
    
    // Resolution Settings

    private void SaveResol()
    {
        PlayerPrefs.SetInt("ScreenMode", (int)screenMode);
        PlayerPrefs.SetInt("Resolution", (int)resolution);
    }

    private void LoadResol()
    {
        screenMode = (FullScreenMode)PlayerPrefs.GetInt("ScreenMode", 
            (int)FullScreenMode.Windowed);
        resolution = (Resolution)PlayerPrefs.GetInt("Resolution", 
            (int)Resolution.p720);
        
        ApplyResolutionOption();
    }

    public void ChangeWindow(int index)
    {
        switch (index)
        {
            case 0: // 창 모드 (Windowed)
                screenMode = FullScreenMode.Windowed;
                break;
            case 1: // 전체 창 모드 (Borderless)
                screenMode = FullScreenMode.FullScreenWindow;
                break;
        }

        ApplyResolutionOption();
        SaveResol();
    }

    public void ChangeResolution(int index)
    {
        resolution = (Resolution)index;
        ApplyResolutionOption();
        SaveResol();
    }

    private void ApplyResolutionOption()
    {
        int width = 1280, height = 720; // 기본값

        switch (resolution)
        {
            case Resolution.p480:
                width = 640;
                height = 480;
                break;
            case Resolution.p720:
                width = 1280;
                height = 720;
                break;
            case Resolution.p1080:
                width = 1920;
                height = 1080;
                break;
            case Resolution.p1440:
                width = 2560;
                height = 1440;
                break;
        }
        Screen.fullScreenMode = screenMode;
        Screen.SetResolution(width, height, screenMode);
    }


    public void LoadAll()
    {
        LoadSound();
        LoadResol();
    }

    public void SaveAll()
    {
        SaveSound();
        SaveResol();
        PlayerPrefs.Save();
    }
}
