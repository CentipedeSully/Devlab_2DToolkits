using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SullysToolkit;

public class StateTests
{
    [Test]
    public void Test_EnablingState()
    {
        State myState = new State("isJumping");
        myState.SetStateActivity(true);

        Assert.AreEqual(true, myState.IsStateActive());
    }

    [Test]
    public void Test_ValueEquivalencyTestUsingObjEquals()
    {
        State myFirstState = new State("isJumping");
        State mySecondState = new State("isJumping");

        Assert.AreEqual(true, myFirstState.Equals(mySecondState));
    }

    [Test]
    public void Test_ValueEquivalencyTestUsingDoubleEquals()
    {
        State myFirstState = new State("isJumping");
        State mySecondState = new State("isJumping");

        bool expression = myFirstState == mySecondState;
        Assert.AreEqual(true, expression);
    }

    [Test]
    public void Test_ValueNonequivalencyTestUsingObjectEquals()
    {
        State myFirstState = new State("isJumping");
        State mySecondState = new State("isNotJumping");

        Assert.AreEqual(false, myFirstState.Equals(mySecondState));
    }

    [Test]
    public void Test_ValueNonequivalencyTestUsingDoubleEquals()
    {
        State myFirstState = new State("isJumping");
        State mySecondState = new State("isNotJumping");

        bool expression = myFirstState == mySecondState;
        Assert.AreEqual(false, expression);
    }

    [Test]
    public void Test_NullNonequivalencyTest()
    {
        State myFirstState = new State("isJumping");
        State mySecondState = null;

        Assert.AreEqual(false, myFirstState.Equals(mySecondState));
    }

    [Test]
    public void Test_HashEquivalencyTest()
    {
        State myFirstState = new State("isJumping");
        State mySecondState = new State("isJumping");

        Assert.AreEqual(true, myFirstState.GetHashCode() == mySecondState.GetHashCode());
    }

    [Test]
    public void Test_HashNonequivalencyTest()
    {
        State myFirstState = new State("isJumping");
        State mySecondState = new State("isNotJumping");

        Assert.AreEqual(false, Equals(myFirstState.GetHashCode(), mySecondState.GetHashCode()));
    }

    [Test]
    public void Test_NullEquivalencyTest()
    {
        State myFirstState = null;

        Assert.AreEqual(true, Equals(myFirstState, null));
    }

    [Test]
    public void Test_StringHashToStateHashNonequivalencyTest()
    {
        State myState = new State("isGoofy");
        string myString = "isGoofy";

        Assert.AreEqual(false, myState.GetHashCode() == myString.GetHashCode());
    }
}
