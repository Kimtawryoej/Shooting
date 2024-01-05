using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimerSystem : SingleTone<TimerSystem>
{

    private List<TimeAgent> timeAgentHashSet = new();//LinkedList,Hash성능차이
    private List<TimeAgent> destroyTimeAgentHashSet = new(); 

    private void Update()
    {
        TimeManager();
    }


    private void TimeManager()
    {
        if (timeAgentHashSet.Count > 0)
        {
            UpdateTimeAgent();
            DestroyTimeAgent();
        }
    }

    public void AddTimer(TimeAgent timeAgent)
    {
        if(SameCheck<TimeAgent>(timeAgentHashSet,timeAgent))
        {
            timeAgentHashSet.Add(timeAgent);
            
        }
    }

    private void UpdateTimeAgent() //함수 진행도중 timeAgentHashSet에 값이 추가되면 오류 => foreach문 for문으로 바꿔서 해결
    {
        for(int i= 0; i< timeAgentHashSet.Count; i++)
        {
            timeAgentHashSet[i].AddTime(Time.deltaTime);
            timeAgentHashSet[i].UpdateTimeAction?.Invoke(timeAgentHashSet[i]);
            if (timeAgentHashSet[i].IsTimeUp)
            {
                destroyTimeAgentHashSet.Add(timeAgentHashSet[i]);
            }
        }
    }

    private void DestroyTimeAgent()
    {
        foreach (var destroyTimeAgent in destroyTimeAgentHashSet)
        {
            timeAgentHashSet.Remove(destroyTimeAgent);
            destroyTimeAgent.EndTimeAction?.Invoke(destroyTimeAgent);
            destroyTimeAgent.TimeReset();
        }
        destroyTimeAgentHashSet.Clear();
    }

    private bool SameCheck<T>(List<T> list, T sameObjecrt)
    {
        bool check = true;
        if (list.Contains(sameObjecrt))
            check = false;
        else
            check = true;
        return check; 
    }

    public void motivation(float time)
    {
        float currentTime = 0;
        while(currentTime<=time)
        {
            currentTime += Time.deltaTime;
        }
    }
}

public class TimeAgent
{
    public float CurrentTime { get; private set; }
    private readonly float timerTime;
    public float TimerTime => timerTime;
    public Action<TimeAgent> UpdateTimeAction { get; set; }
    public Action<TimeAgent> EndTimeAction { get; set; }

    public TimeAgent(float time,Action<TimeAgent> updateTimeAction = default, Action<TimeAgent> endTimeAction = default)
    {
        this.timerTime = time;
        UpdateTimeAction = updateTimeAction;
        EndTimeAction = endTimeAction;
    }

    public bool IsTimeUp => CurrentTime >= timerTime;

    public void TimeReset()
    {
        CurrentTime = 0;
    }
    public void AddTime(float time)
    {
        CurrentTime += time;
    }
}