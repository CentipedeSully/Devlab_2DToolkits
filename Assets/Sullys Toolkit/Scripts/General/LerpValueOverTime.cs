using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace SullysToolkit
{

    public class LerpValueOverTime : MonoBehaviour
    {
        [Tooltip("Name it for organizational purposes, in case there are multiple lerpers on a single gameObject")]
        [SerializeField] private string _LerperName;

        [Space(10)]
        [Tooltip("This is used to communcate the lerping value to other scripts throughout the lerp process")]
        public UnityEvent<float> OnShareLerpResult;


        private float _startValue;
        private float _targetValue;
        private float _lerpDuration;

        private float _currentLerpValue;
        private float _timeElapsed;
        private float _percentageComplete;

        private bool _isLerping = false;



        private void Update()
        {
            if (_isLerping)
            {
                Lerp();
                CalculateTimeDelta();
                CalculatePercentageCompleted();
            }
        }



        public bool IsLerping()
        {
            return _isLerping;
        }

        public string GetName()
        {
            return _LerperName;
        }

        public void SetLerp(float start, float target, float totalLerpDuration)
        {
            if (_isLerping)
                ResetLerp();

            _startValue = start;
            _targetValue = target;

            if (totalLerpDuration >= 0)
                _lerpDuration = totalLerpDuration;
            else _lerpDuration = 0;
        }

        public void StartLerp()
        {
            _isLerping = true;
        }

        public void Lerp()
        {
            _currentLerpValue = Mathf.Lerp(_startValue, _targetValue, _percentageComplete);
            OnShareLerpResult?.Invoke(_currentLerpValue);
            if (_percentageComplete >= 1)
                ResetLerp();
        }


        private void CalculateTimeDelta()
        {
            if (_isLerping)
                _timeElapsed += Time.deltaTime;
        }


        private void CalculatePercentageCompleted()
        {
            if (_isLerping)
            {
                if (_timeElapsed == 0)
                    _percentageComplete = 0;
                else _percentageComplete = _timeElapsed / _lerpDuration;
            }

        }


        private void ResetLerp()
        {
            _isLerping = false;

            _startValue = 0;
            _targetValue = 0;
            _lerpDuration = 0;
            _timeElapsed = 0;
            _percentageComplete = 0;

            _currentLerpValue = 0;
        }
    }

}

