using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SullysToolkit
{
    public interface ITimer
    {
        void SetTimer(float seconds);

        void StartTimer();

        void HaltTimer();

        void ResetTimer();

        bool IsTimerTicking();

        float GetCurrentTimeInSeconds();

        float GetTargetTimeInSeconds();

        string GetTimerName();
    }
}
