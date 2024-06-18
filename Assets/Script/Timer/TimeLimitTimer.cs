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
    public override double QuestionTime { get => timer.Elapsed.TotalSeconds; }

    float maxQuestionTime;
    public override float MaxQuestionTime { get => maxQuestionTime; set { if (!active) maxQuestionTime = value; } }
    Stopwatch timer = new Stopwatch();
    OnEvent OnTimeCompleted;

    Task<bool> TimeCompletionCheck;
    bool active = false;

    public override bool CheckIfTimeLeft()
    {
       if(maxQuestionTime == 0)
       {
            return true;
       }
       if(QuestionTime >= maxQuestionTime)
       {
            return false;
       }
       return true;
    }

    public override void Stop()
    {
        active = false;
        timer.Reset();
        TimeCompletionCheck = null;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        active = true;
        timer.Start();
    }
    public override void Start(float maxQuestionTime, OnEvent onTimeComplete)
    {
        this.maxQuestionTime = maxQuestionTime;
        timer.Start();
        OnTimeCompleted = new OnEvent(onTimeComplete);
        TimeCompletionCheck = CheckContinuously();
        active = true;
    }

    private async Task<bool> CheckContinuously()
    {
        while (timer.Elapsed.TotalSeconds < maxQuestionTime)
        {
            await Task.Delay(100); // Check every 100 milliseconds
        }
        OnTimeCompleted?.Invoke();
        Stop();

        
        return true;
    }
    public override void Pause()
    {
        active = false;
        timer.Stop();
    }

}

public abstract class Timer
{
    public abstract double QuestionTime { get; }
    public abstract float MaxQuestionTime { get; set; }
    public abstract bool CheckIfTimeLeft();
    public abstract void Start();
    public abstract void Start(float maxQuestionTime, OnEvent O);
    public abstract void Stop();
    public abstract void Pause();

}
