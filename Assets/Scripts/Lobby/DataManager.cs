
public class DataManager : Singleton<DataManager>
{
    public SettingOption settingOption;

    
    protected override void Awake()
    {
        base.Awake();
        
        settingOption = new SettingOption();
        
        LoadAllData();
    }


    private void LoadAllData()
    {
        settingOption.LoadAll();
    }

    private void SaveAllData()
    {
        settingOption.SaveAll();
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        SaveAllData();
    }
    
}
