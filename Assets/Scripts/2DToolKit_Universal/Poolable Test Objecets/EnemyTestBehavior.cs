using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTestBehavior : MonoBehaviour
{
    public int _amountOfSecondsAlive = 0;

    public bool _isAlive = false;


    private void OnEnable()
    {
        BecomeAlive();
    }

    private void OnDisable()
    {
        ResetBehavior();
    }

    public void BecomeAlive()
    {
        if (_isAlive == false)
        {
            _isAlive = true;

            CountTimeAlive();
        }
    }

    private void CountTimeAlive()
    {
        InvokeRepeating("TickSecond", 1, 1);
    }

    private void TickSecond()
    {
        _amountOfSecondsAlive++;
    }

    public void Die()
    {
        _isAlive = false;
        CancelInvoke();
    }


    public void ResetBehavior()
    {
        Die();

        _amountOfSecondsAlive = 0;
    }
}
