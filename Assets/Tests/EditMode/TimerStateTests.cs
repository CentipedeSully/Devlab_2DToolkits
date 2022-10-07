using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TimerStateTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestDefaultTimerName()
    {
        GameObject testGameObject = new GameObject();
        testGameObject.AddComponent<Timer>();

        Assert.AreEqual("Unnamed Timer", testGameObject.GetComponent<Timer>().GetTimerName());
    }

    [Test]
    public void TestSettingTimerName()
    {
        GameObject testGameObject = new GameObject();
        testGameObject.AddComponent<Timer>();

        string name = "My new timer";
        testGameObject.GetComponent<Timer>().SetName(name);

        Assert.AreEqual(name, testGameObject.GetComponent<Timer>().GetTimerName());
    }

    [Test]
    public void Test_TimerIsntTickingWhenOff()
    {
        GameObject testGameObject = new GameObject();
        testGameObject.AddComponent<Timer>();


        Assert.AreEqual(false,testGameObject.GetComponent<Timer>().IsTimerTicking());
    }





}
