using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayManager : MonoBehaviour
{
    [SerializeField] private Color dayColor;
    [SerializeField] private Color twilightColor;
    [SerializeField] private Color nightColor;

    private Light2D globalLight;
    
    [Tooltip("인게임 하루 시간 (초단위)")]
    [SerializeField] private float dayDuration = 1200f; // 실제 시간 20분 (1200초)
    
    [Tooltip("게임내 현재 시간")]
    [SerializeField] private float timeOfDay = 12f; // 게임 내 시간이 6:00 AM에서 시작 (6시)
    
    
    private int currentDay;
    private int currentHour;
    private int hour;

    
    private Color mineColor;
    private bool isMine = false;
    private Color tempColor;
    
    private void Awake()
    {
        globalLight = GetComponent<Light2D>();
        currentDay = 1;
        currentHour = GetCurrentTime();
        
        GameEventsManager.Instance.dayEvents.onGetCurrentTime += GetCurrentTime;
        
        
        GameEventsManager.Instance.playerEvents.onMineEnter += EnterMine;
        GameEventsManager.Instance.playerEvents.onMineExit += ExitMine;
    }

    private void Start()
    {
        //todo : 저장된 timeOfDay 불러오기
        
        GameEventsManager.Instance.dayEvents.ChangeDay(currentDay);
        GameEventsManager.Instance.dayEvents.ChangeTime(currentHour);
    }

    private void Update()
    {
        // 실제 시간으로 20분이 하루가 되도록 설정
        timeOfDay += Time.deltaTime / dayDuration * 24f; // 24시간 기준으로 계산

        if (timeOfDay >= 24f)
        {
            timeOfDay = 0f; // 하루가 끝나면 다시 0시로
            currentDay++;
            GameEventsManager.Instance.dayEvents.ChangeDay(currentDay);
        }
        
        // 시간 확인
        hour = GetCurrentTime();
        if (hour != currentHour)
        {
            currentHour = hour;
            //한시간 단위로 업데이트
            GameEventsManager.Instance.dayEvents.ChangeTime(currentHour);
        }
        
        // 색상 변화
        if(!isMine)
            globalLight.color = GetColorForTimeOfDay(timeOfDay);
    }

    // 시간에 따라 색상을 결정하는 함수
    private Color GetColorForTimeOfDay(float time)
    {
        if (time >= 6f && time < 8f) // 6:00 AM ~ 8:00 AM: 황혼 -> dayColor
        {
            return Color.Lerp(twilightColor, dayColor, (time - 6f) / 2f);
        }
        if (time >= 8f && time < 16f) // 8:00 AM ~ 4:00 PM: dayColor 유지
        {
            return dayColor;
        }
        if (time >= 16f && time < 18f) // 4:00 PM ~ 6:00 PM: dayColor -> 황혼
        {
            return Color.Lerp(dayColor, twilightColor, (time - 16f) / 2f);
        }
        if (time >= 18f && time < 22f) // 6:00 PM ~ 10:00 PM: 황혼 -> nightColor
        {
            return Color.Lerp(twilightColor, nightColor, (time - 18f) / 4f);
        }
        if (time >= 22f || time < 4f) // 10:00 PM ~ 4:00 AM: nightColor 유지
        {
            return nightColor;
        }
        if (time >= 4f && time < 6f)
        {
            return Color.Lerp(nightColor, twilightColor, (time - 4f) / 2f);
        }
        
        
        return nightColor; // 기본값
    }

    // 현재 시간을 계산하는 함수 (시만 반환)
    private int GetCurrentTime()
    {
        return Mathf.FloorToInt(timeOfDay); // 시간은 정수 값으로 반환 (소수점 버리기)
    }


    private void EnterMine(Color color)
    {
        isMine = true;
        tempColor = globalLight.color;
        globalLight.color = color;
    }

    private void ExitMine()
    {
        isMine = false;
        globalLight.color = tempColor;
    }
}
