using UnityEngine;
using UnityEngine.UI;

public class SoundSet : MonoBehaviour
{
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider bgmVolume;
    [SerializeField] private Slider sfxVolume;

    private void Start()
    {
        masterVolume.onValueChanged.AddListener((float value) =>
        {
            AudioManager.Instance.ChangeVolume(SettingOption.Sound.Master, value);
        });

        bgmVolume.onValueChanged.AddListener((float value) =>
        {
            AudioManager.Instance.ChangeVolume(SettingOption.Sound.Bgm, value);
        });

        sfxVolume.onValueChanged.AddListener((float value) =>
        {
            AudioManager.Instance.ChangeVolume(SettingOption.Sound.Sfx, value);
        });

        LoadUI();
    }

    private void LoadUI()
    {
        masterVolume.value = Mathf.Pow(10, DataManager.Instance.settingOption.MasterSound / 20);
        bgmVolume.value = Mathf.Pow(10, DataManager.Instance.settingOption.BGMSound / 20);
        sfxVolume.value = Mathf.Pow(10, DataManager.Instance.settingOption.SFXSound / 20);
    }
}