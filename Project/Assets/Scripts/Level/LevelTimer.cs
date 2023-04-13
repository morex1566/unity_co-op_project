using System;
using UnityEngine;
using System.Collections;

public enum TimePer
{
    Milisec = 10,
    Sec = 1
}

public class LevelTimer : MonoBehaviour
{
    private float _currTime;
    private bool _isPaused;

    private void Awake()
    {
        _currTime = 0;
        _isPaused = false;
    }

    // ACTION : 타이머 시간설정 + 작동
    public IEnumerator StartTimerFor(TimePer unit, float time)
    {
        while (_currTime < time)
        {
            if (!_isPaused)
            {
                _currTime += Time.deltaTime * (float)unit;
            }
            yield return null;
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
