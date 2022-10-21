using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SullysToolkit
{
    public class Timer : MonoBehaviour
    {
        /// <summary>
        /// This class counts time until a maximum time is reached. It raises an event on completion.
        /// </summary>

        //Declarations 
        private int _durationInMilliseconds = 0;
        private int _millisecondsPassed = 0;
        private bool _isTimerStarted = false;
        private bool _isTimerTicking = false;

        [Header("Timer Settings")]
        [Tooltip("A unique and useful identity for this timer instance. Available for referencing to other scripts (Read-only)")]
        [SerializeField] private string _timerName = "Unnamed Timer";

        [Space(20)]
        [Header("Timer Events")]
        public UnityEvent OnTimerExpiration;



        //Monobehaviors
        //...



        //Utils
        public string GetTimerName()
        {
            return _timerName;
        }

        public void SetName(string name)
        {
            if (name != null)
                _timerName = name;
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

        private float ConvertMillisecondsIntoSeconds(int milliseconds)
        {
            if (milliseconds == 0)
                return 0;
            else return milliseconds / 100;
        }



        public void SetTimer(float seconds)
        {
            if (seconds > 0)
                _durationInMilliseconds = ConvertSecondsIntoMilliseconds(seconds);
        }

        private int ConvertSecondsIntoMilliseconds(float seconds)
        {
            return (int)(seconds * 100);
        }



        public void StartTimer()
        {
            if (_isTimerStarted == false)
            {
                _isTimerStarted = true;
                StartTicking();
            }

            else if (!IsTimerDurationReached() && !_isTimerTicking)
                StartTicking();
        }

        private void StartTicking()
        {
            _isTimerTicking = true;
            InvokeRepeating("TickMillisecond", .01f, .01f);
        }

        private bool IsTimerDurationReached()
        {
            if (_millisecondsPassed >= _durationInMilliseconds)
                return true;
            else return false;
        }

        private void TickMillisecond()
        {
            _millisecondsPassed += 1;

            if (IsTimerDurationReached())
            {
                StopTicking();
                InvokeOnTimerExpirationEvent();
            }
        }

        private void InvokeOnTimerExpirationEvent()
        {
            OnTimerExpiration?.Invoke();

        }



        public void HaltTimer()
        {
            StopTicking();
        }

        private void StopTicking()
        {
            CancelInvoke("TickMillisecond");
            _isTimerTicking = false;
        }

        public void ResetTimer()
        {
            if (_isTimerTicking)
                StopTicking();

            _millisecondsPassed = 0;
            _isTimerStarted = false;
        }













    }

}
