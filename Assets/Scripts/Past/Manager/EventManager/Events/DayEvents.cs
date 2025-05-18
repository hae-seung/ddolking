using System;

public class DayEvents
{
    public Action<int> onChangeDay;
    public void ChangeDay(int day)
    {
        onChangeDay?.Invoke(day);
    }

    public Action<int> onChangeTime;//UI 표기용
    public void ChangeTime(int time)
    {
        onChangeTime?.Invoke(time);
    }
    

    public Func<int> onGetCurrentTime;
    public int GetCurrentTime()
    {
        if (onGetCurrentTime != null)
            return onGetCurrentTime.Invoke();
        return -1;
    }

}
