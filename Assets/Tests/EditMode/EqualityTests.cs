using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EqualityTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void ValueNonequailtyTestOnInts()
    {
        int myFirstInt = 5;
        int mySecondInt = 10;
        Assert.AreEqual(false, myFirstInt == mySecondInt);
    }

    [Test]
    public void ValueEquailtyTestOnInts()
    {
        int myFirstInt = 10;
        int mySecondInt = 10;
        Assert.AreEqual(true, myFirstInt == mySecondInt);
    }

    [Test]
    public void ValueNonequailtyTestOnStrings()
    {
        string myFirstString = "cats";
        string mySecondString = "dogs";
        Assert.AreEqual(false, myFirstString == mySecondString);
    }

    [Test]
    public void ValueEquailtyTestOnStrings()
    {
        string myFirstString = "cats";
        string mySecondString = "cats";
        Assert.AreEqual(true, myFirstString == mySecondString);
    }

    [Test]
    public void ReferenceEquailtyTestOnStates()
    {
        State moveState = new State("isMoving");
        State CopyOfMoveState = moveState;
        Assert.AreEqual(true, moveState == CopyOfMoveState);
    }

    [Test]
    public void ValueEqualityOverloadTestOnStates()
    {
        State moveState = new State("isMoving");
        State differentMoveState = new State("isMoving");
        Assert.AreEqual(true, moveState == differentMoveState);
    }

}
