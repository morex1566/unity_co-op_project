using System;
using UnityEngine;
using System.Threading.Tasks;

public enum TimePer
{
    Milisec = 10,
    Sec = 1
}

public class LevelTimer : MonoBehaviour
{
    private float _currTime;
    private bool _isPaused;

    public static Task Timer;

    private void Awake()
    {
        _currTime = 0;
        _isPaused = false;
    }

    public void StartTimerFor(TimePer unit, float time)
    {
        Timer = startTimer(unit, time);
    }

    // ACTION : 타이머 시간설정 + 작동
    private async Task startTimer(TimePer unit, float time)
    {
        while (_currTime < time)
        {
            if (!_isPaused)
            {
                _currTime += Time.deltaTime * (float)unit;
            }
            await Task.Yield();
        }
    }
    
    // ACTION : 타이머 정지
    public void PauseTimer(){_isPaused = true;}
    // ACTION : 타이머 재개
    public void ResumeTimer(){_isPaused = false;}
    // ACTION : 현재 시간
    public float GetTime(TimePer unit)
    {
        return (float)Math.Round(_currTime * (float)unit);
    }
}
