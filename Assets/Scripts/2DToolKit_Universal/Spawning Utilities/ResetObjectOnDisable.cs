using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResetObjectOnDisable : MonoBehaviour
{
    public UnityEvent OnDisableSignal;


    private void OnDisable()
    {
        OnDisableSignal?.Invoke();
    }
}
