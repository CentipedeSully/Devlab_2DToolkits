using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using SullysToolkit;

public class LerpController: MonoBehaviour
{
    /*
    [SerializeField] private float _startValue;
    [SerializeField] private float _targetValue;
    [SerializeField] private float _totalDuration;
    [SerializeField] private float _currentLerpedValue;

    private float _keypressCooldown = 1;
    private bool _pressEnabled = true;

    [SerializeField] private Lerper _lerperReference;

    private void OnEnable()
    {
        _lerperReference.OnShareLerpResult.AddListener(GetLerpResult);
    }

    private void OnDisable()
    {
        _lerperReference.OnShareLerpResult.RemoveListener(GetLerpResult);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _pressEnabled)
        {
            Debug.Log("Lerp Command Sent!");
            _pressEnabled = false;
            EnablePressAfterCooldown();

            _lerperReference.SetLerp(_startValue, _targetValue, _totalDuration);
            _lerperReference.StartLerp();

        }
    }



    private void DisablePress()
    {
        _pressEnabled = false;
    }

    private void EnablePress()
    {
        _pressEnabled = true;
    }

    private void EnablePressAfterCooldown()
    {
        Invoke("EnablePress", _keypressCooldown);
    }

    private void GetLerpResult(float result)
    {
        _currentLerpedValue = result;
    }

    */
}
