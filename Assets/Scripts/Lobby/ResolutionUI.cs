using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResolutionUI : MonoBehaviour
{
    public TMP_Dropdown windowDropdown;
    public TMP_Dropdown resolutionDropdown;

    private void Start()
    {
        // 창 모드 변경 이벤트 연결
        windowDropdown.onValueChanged.AddListener((int index) =>
        {
            DataManager.Instance.settingOption.ChangeWindow(index);
        });

        // 해상도 변경 이벤트 연결
        resolutionDropdown.onValueChanged.AddListener((int index) =>
        {
            DataManager.Instance.settingOption.ChangeResolution(index);
        });

        LoadUI();
    }


    private void LoadUI()
    {
        LoadResolutionUI();
        LoadSoundUI();
    }

    private void LoadSoundUI()
    {
        
    }

    private void LoadResolutionUI()
    {
        windowDropdown.value = DataManager.Instance.settingOption.screenMode == FullScreenMode.Windowed ? 0 : 1;
        resolutionDropdown.value = (int)DataManager.Instance.settingOption.resolution;
    }
}