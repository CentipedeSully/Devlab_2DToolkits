using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;
using SullysToolkit;

public class TimerStateTestsOnPlay
{
    [UnityTest]
    public IEnumerator Test_TimerIsntTickingWhenHalted()
    {
        GameObject testGameObject = new GameObject();
        Timer timer = testGameObject.AddComponent<Timer>();

        timer.SetTimer(9);
        timer.StartTimer();
        yield return new WaitForSeconds(.5f);
        testGameObject.GetComponent<Timer>().HaltTimer();

        Assert.AreEqual(false, timer.IsTimerTicking());
    }

    [UnityTest]
    public IEnumerator Test_TimerIsTickingWhenOn()
    {
        GameObject testGameObject = new GameObject();
        Timer timer = testGameObject.AddComponent<Timer>();

        timer.SetTimer(9);
        timer.StartTimer();
        yield return new WaitForSeconds(.5f);

        Assert.AreEqual(true, timer.IsTimerTicking());
    }

    //Test Timer tick accuracy: 1 milliseconds
    [UnityTest]
    public IEnumerator Test_TimerDisplays1msAfter1msPasses()
    {
        GameObject testGameObject = new GameObject();
        Timer timer = testGameObject.AddComponent<Timer>();
        float testDuration = 1f / 100f;

        timer.SetTimer(12);
        timer.StartTimer();
        yield return new WaitForSeconds(testDuration);

        Assert.AreEqual(true, timer.GetCurrentTimeInSeconds() == testDuration);
    }

    //Test Timer tick accuracy: 10 milliseconds
    [UnityTest]
    public IEnumerator Test_TimerDisplays10msAfter10msPasses()
    {
        GameObject testGameObject = new GameObject();
        Timer timer = testGameObject.AddComponent<Timer>();
        float testDuration = 1 / 10f;

        timer.SetTimer(12);
        timer.StartTimer();
        yield return new WaitForSeconds(testDuration);

        Assert.AreEqual(true, timer.GetCurrentTimeInSeconds() == testDuration);
    }

    //Test Timer tick accuracy: 100 milliseconds
    [UnityTest]
    public IEnumerator Test_TimerDisplays100msAfter100msPasses()
    {
        GameObject testGameObject = new GameObject();
        Timer timer = testGameObject.AddComponent<Timer>();
        float testDuration = 1f;

        timer.SetTimer(12);
        timer.StartTimer();
        yield return new WaitForSeconds(testDuration);

        Assert.AreEqual(true, timer.GetCurrentTimeInSeconds() == testDuration);
    }

    //Test Timer tick accuracy: 1000 milliseconds
    [UnityTest]
    public IEnumerator Test_TimerDisplays1000msAfter1000msPasses()
    {
        GameObject testGameObject = new GameObject();
        Timer timer = testGameObject.AddComponent<Timer>();
        float testDuration = 10f;

        timer.SetTimer(12);
        timer.StartTimer();
        yield return new WaitForSeconds(testDuration);

        Assert.AreEqual(true, timer.GetCurrentTimeInSeconds() == testDuration);
    }

    //Test Timer Return after completion
    [UnityTest]
    public IEnumerator Test_TimerCompletionEvent()
    {
        GameObject testGameObject = new GameObject();
        Timer timer = testGameObject.AddComponent<Timer>();

        timer.SetTimer(1);
        timer.StartTimer();
        yield return new WaitForSeconds(2);

        Assert.AreEqual(true, timer.GetCurrentTimeInSeconds() == timer.GetTargetTimeInSeconds() && timer.IsTimerTicking() == false);
    }
}
