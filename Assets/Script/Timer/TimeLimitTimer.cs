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

    private Coroutine TimeCompletion;
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
        if(TimeCompletion != null)
        {
            GameManagerScript.ObjectInvoker.StopCoroutine(TimeCompletion);
            TimeCompletion = null;
        }
    }

    public override void Start()
    {
        active = true;
        TimeCompletion = GameManagerScript.ObjectInvoker.StartCoroutine(EndTimeInvoker(maxQuestionTime));
        timer.Start();
    }

    public override void Start(float maxQuestionTime, OnEvent onTimeComplete)
    {
        
        this.maxQuestionTime = maxQuestionTime;
        ResetTimer();
        OnTimeCompleted = onTimeComplete;
        TimeCompletion = GameManagerScript.ObjectInvoker.StartCoroutine(EndTimeInvoker(maxQuestionTime));
        active = true;
    }

    private IEnumerator EndTimeInvoker(float maxQuestionTime)
    {
        yield return new WaitForSeconds(maxQuestionTime - (float)QuestionTime);
        OnTimeCompleted?.Invoke();
        Stop();
    }

    public override void Pause()
    {
        active = false;
        if(TimeCompletion != null)
            GameManagerScript.ObjectInvoker.StopCoroutine(TimeCompletion);
        timer.Stop();
    }

    public void ResetTimer()
    {
        timer.Reset();
        if(TimeCompletion != null)
            GameManagerScript.ObjectInvoker.StopCoroutine(TimeCompletion);
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
