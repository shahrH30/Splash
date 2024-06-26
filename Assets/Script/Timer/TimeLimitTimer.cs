using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using System.Timers;
using System.Diagnostics;

public class TimeLimitTimer : Timer
{
    public override double QuestionTime => timer.Elapsed.Seconds;

    private float maxQuestionTime;
    public override float MaxQuestionTime
    {
        get => maxQuestionTime;
        set { if (!active) maxQuestionTime = value; }
    }

    public bool IsRunning => timer.IsRunning;

    private readonly Stopwatch timer = new Stopwatch();
    private OnEvent OnTimeCompleted;

    private Task TimeCompletionCheck;
    private bool active = false;

    public override bool CheckIfTimeLeft()
    {
        if (maxQuestionTime == 0)
        {
            return true;
        }
        return QuestionTime < maxQuestionTime;
    }

    public override void Stop()
    {
        active = false;
        timer.Reset();
        TimeCompletionCheck = null;
    }

    public override void Start()
    {
        active = true;
        timer.Start();
    }

    public override void Start(float maxQuestionTime, OnEvent onTimeComplete)
    {
        this.maxQuestionTime = maxQuestionTime;
        timer.Start();
        OnTimeCompleted = onTimeComplete;
        TimeCompletionCheck = CheckContinuously();
        active = true;
    }

    private async Task CheckContinuously()
    {
        while (timer.Elapsed.TotalSeconds < maxQuestionTime)
        {
            await Task.Delay(100); // Check every 100 milliseconds
        }
        OnTimeCompleted?.Invoke();
        Stop();
    }

    public override void Pause()
    {
        active = false;
        timer.Stop();
    }

    public void ResetTimer()
    {
        timer.Reset();
        timer.Start();
    }
}

public abstract class Timer
{
    public abstract double QuestionTime { get; }
    public abstract float MaxQuestionTime { get; set; }
    public abstract bool CheckIfTimeLeft();
    public abstract void Start();
    public abstract void Start(float maxQuestionTime, OnEvent onTimeComplete);
    public abstract void Stop();
    public abstract void Pause();
}
