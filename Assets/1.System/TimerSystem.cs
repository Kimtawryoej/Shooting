using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimerSystem : SingleTone<TimerSystem>
{
    private HashSet<TimeAgent> timeAgentHashSet = new();
    private HashSet<TimeAgent> destroyTimeAgentHashSet = new();

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
        timeAgentHashSet.Add(timeAgent);
    }

    private void UpdateTimeAgent() //조건이 안겹친다는 조건하에 잘 작동
    {
        foreach (var updateTimeAgent in timeAgentHashSet)
        {
            updateTimeAgent.AddTime(Time.deltaTime);
            updateTimeAgent.UpdateTimeAction?.Invoke(updateTimeAgent);
            if (updateTimeAgent.IsTimeUp)
            {
                destroyTimeAgentHashSet.Add(updateTimeAgent);
            }
        }
    }

    private void DestroyTimeAgent()
    {
        foreach (var destroyTimeAgent in destroyTimeAgentHashSet)
        {
            timeAgentHashSet.Remove(destroyTimeAgent);
            destroyTimeAgent.EndTimeAction?.Invoke(destroyTimeAgent);
        }
        destroyTimeAgentHashSet.Clear();
    }
}

public class TimeAgent
{
    public float CurrentTime { get; private set; }
    private readonly float timerTime;
    public float TimerTime => timerTime;

    public Action<TimeAgent> UpdateTimeAction { get; set; }
    public Action<TimeAgent> EndTimeAction { get; set; }

    public TimeAgent(float time, Action<TimeAgent> updateTimeAction = default, Action<TimeAgent> endTimeAction = default)
    {
        this.timerTime = time;
        UpdateTimeAction = updateTimeAction;
        EndTimeAction = endTimeAction;
    }

    public bool IsTimeUp => CurrentTime >= timerTime;

    public void AddTime(float time)
    {
        CurrentTime += time;
    }
}