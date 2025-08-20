





using UnityEngine;

public class BossPatternState 
{
    private BossPattern pattern;
    private float lastUsedTime;

    public BossPattern Pattern => pattern;
    
    public BossPatternState(BossPattern pattern) 
    {
        this.pattern = pattern;
        lastUsedTime = -pattern.Cooldown; // 시작할 때 바로 사용 가능
    }

    public bool IsReady() 
    {
        return Time.time - lastUsedTime >= pattern.Cooldown;
    }

    public void Use() 
    {
        lastUsedTime = Time.time;
    }
}