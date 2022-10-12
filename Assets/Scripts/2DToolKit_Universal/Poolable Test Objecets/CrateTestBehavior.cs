using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateTestBehavior : MonoBehaviour
{
    public int maxHp = 10;

    public int currenthp = 10;



    private void Start()
    {
        TakeRandomDamage();
    }



    public void TakeRandomDamage()
    {
        currenthp -= Random.Range(0,11);
    }

    public void ResetCrateBehavior()
    {
        currenthp = maxHp;
    }
}
