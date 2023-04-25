using System;
using UnityEngine;
using System.Threading.Tasks;

public enum TimePer
{
    Milisec = 10,
    Sec = 1
}

public class LevelTimer
{
    private float _current;
    private bool _isPaused;
    private float _at;

    private Task _timer;
    public LevelTimer()
    {
        _current = 0;
        _isPaused = false;
    }

    public void StartTimerFor(TimePer unit, float time)
    {
        _at = time;
        _timer = startTimer(unit, time);
    }

    // ACTION : 타이머 시간설정 + 작동
    private async Task startTimer(TimePer unit, float time)
    {
        _at = time;
        
        while (_current < time)
        {
            if (!_isPaused)
            {
                _current += Time.deltaTime * (float)unit;
            }
            await Task.Yield();
        }

        _at = 0;
    }
    
    // ACTION : 타이머 정지
    public void PauseTimer(){_isPaused = true;}
    // ACTION : 타이머 재개
    public void ResumeTimer(){_isPaused = false;}
    // ACTION : 현재 시간
    public float Current()
    {
        return (float)Math.Round(_current);
    }

    // ACTION : 입력된 타이머 시간
    public float At()
    {
        return _at;
    }
}
