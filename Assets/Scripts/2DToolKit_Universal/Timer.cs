using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    //Declarations 
    private int _durationInMilliseconds = 0;
    private int _millisecondsPassed = 0;
    private bool _isTimerStarted = false;
    private bool _isTimerTicking = false;

    [Header("Timer Settings")]
    [Tooltip("A unique and useful identity for this timer instance. Available for referencing to other scripts (Read-only)")]
    [SerializeField] private string _timerName = "Unnamed Timer";
    [SerializeField] private bool _autoResetTimerOnExpiration = false;

    [Space(20)]
    [Header("Timer Events")]
    public UnityEvent OnTimerExpiration;








    //Monobehaviors
    private void OnEnable()
    {
        if (IsAutoResetActive())
            OnTimerExpiration.AddListener(ResetTimer);
    }

    private void OnDisable()
    {
        if (IsAutoResetActive())
            OnTimerExpiration.RemoveListener(ResetTimer);
    }





    //Utils

    public void SetTimer(float seconds)
    {
        if (seconds > 0)
            _durationInMilliseconds = ConvertSecondsIntoMilliseconds(seconds);
        else throw new System.Exception("Invalid TimeArgument passed to SetTimer().");
    }

    private int ConvertSecondsIntoMilliseconds(float seconds)
    {
        return (int)(seconds * 100);
    }

    private float ConvertMillisecondsIntoSeconds(int milliseconds)
    {
        if (milliseconds == 0)
            return 0;
        else return milliseconds / 100;
    }



    public void StartTimer()
    {
        if (_isTimerStarted == false)
        {
            _isTimerStarted = true;
            StartTicking();
        }

        else if (IsTimerDurationReached() == false && _isTimerTicking == false)
            StartTicking();
    }

    private void StartTicking()
    {
        _isTimerTicking = true;
        InvokeRepeating("TickMillisecond", .01f, .01f);
    }

    private void TickMillisecond()
    {
        _millisecondsPassed += 1;

        if(IsTimerDurationReached())
        {
            StopTicking();
            InvokeOnTimerExpirationEvent();
        }
    }

    private bool IsTimerDurationReached()
    {
        if (_millisecondsPassed >= _durationInMilliseconds)
            return true;
        else return false;
    }

    private void InvokeOnTimerExpirationEvent()
    {
        OnTimerExpiration?.Invoke();

    }

    private void StopTicking()
    {
        CancelInvoke("TickMillisecond");
        _isTimerTicking = false;
    }

    public void HaltTimer()
    {
        StopTicking();
    }

    public void ResetTimer()
    {
        if (_isTimerTicking)
            StopTicking();

        _millisecondsPassed = 0;
        _isTimerStarted = false;
    }



    public bool IsTimerTicking()
    {
        return _isTimerTicking;
    }

    public float GetCurrentTimeInSeconds()
    {
        return ConvertMillisecondsIntoSeconds(_millisecondsPassed);
    }

    public float GetTargetTimeInSeconds()
    {
        return ConvertMillisecondsIntoSeconds(_durationInMilliseconds);
    }

    public bool IsAutoResetActive()
    {
        return _autoResetTimerOnExpiration;
    }

    public void EnableAutoReset()
    {
        if (IsAutoResetActive() == false)
        {
            OnTimerExpiration.AddListener(ResetTimer);
            _autoResetTimerOnExpiration = true;
        }

    }

    public void DisableAutoReset()
    {
        if (IsAutoResetActive())
        {
            OnTimerExpiration.RemoveListener(ResetTimer);
            _autoResetTimerOnExpiration = false;
        }

    }

    public string GetTimerName()
    {
        return _timerName;
    }

}
