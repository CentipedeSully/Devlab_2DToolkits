using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTest : MonoBehaviour
{
    //Declarations
    [SerializeField] private float _timerDuration = 0;
    [SerializeField] private Timer _timerReference;





    //Monobehaviors
    private void Update()
    {
        if (IsShiftInputDetected())
            SetTimer();

        if (IsSpaceInputDetected())
            ToggleTimerTicking();

        if (IsEnterInputDetected())
            ResetTimer();

        if (IsXInputDetected())
            ToggleAutoReset();
    }






    //Utils
    private void StartTimer()
    {
        _timerReference.StartTimer();
        Debug.Log("Timer Started");
    }

    private void HaltTimer()
    {
        _timerReference.HaltTimer();
        Debug.Log("Timer Stopped");
    }

    private void ResetTimer()
    {
        _timerReference.ResetTimer();
        Debug.Log("Timer Reset");
    }

    private void ToggleTimerTicking()
    {
        if (_timerReference.IsTimerTicking())
            HaltTimer();
        else StartTimer();
    }

    private void SetTimer()
    {
        _timerReference.SetTimer(_timerDuration);
        Debug.Log("Timer Set");
    }

    private void ToggleAutoReset()
    {
        if (_timerReference.IsAutoResetActive())
            _timerReference.DisableAutoReset();
        else _timerReference.EnableAutoReset();
    }


    private bool IsSpaceInputDetected()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            return true;
        else return false;
    }

    private bool IsEnterInputDetected()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
            return true;
        else return false;
    }

    private bool IsShiftInputDetected()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
            return true;
        else return false;
    }

    private bool IsXInputDetected()
    {
        if (Input.GetKeyDown(KeyCode.X))
            return true;
        else return false;
    }

    private void LogTestResults(string testName, bool result)
    {
        Debug.Log(testName + " results: " + result);
    }
}
