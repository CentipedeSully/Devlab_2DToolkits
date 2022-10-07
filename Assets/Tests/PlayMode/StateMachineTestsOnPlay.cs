using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class StateMachineTestsOnPlay
{
    [UnityTest]
    public IEnumerator UsmToStringTest()
    {

        GameObject testObject = new GameObject();
        UniversalStateMachine uStateMachine = testObject.AddComponent<UniversalStateMachine>();
        uStateMachine.SetName("PlayerStateMachine");

        uStateMachine.LogDictionary();

        Assert.AreEqual(true, uStateMachine.GetName() == "PlayerStateMachine");
        yield return null;
    }

    [UnityTest]
    public IEnumerator UsmNonexistingStateTest()
    {

        GameObject testObject = new GameObject();
        UniversalStateMachine uStateMachine = testObject.AddComponent<UniversalStateMachine>();
        uStateMachine.SetName("PlayerStateMachine");


        Assert.AreEqual(true,uStateMachine.DoesStateExist("isJumping") == false);
        yield return null;
    }


    [UnityTest]
    public IEnumerator UsmDoesStateExistTest()
    {

        GameObject testObject = new GameObject();
        UniversalStateMachine uStateMachine = testObject.AddComponent<UniversalStateMachine>();
        uStateMachine.SetName("PlayerStateMachine");
        uStateMachine.AddState("isJumping");

        Assert.AreEqual(true, uStateMachine.DoesStateExist("isJumping") == true);
        yield return null;
    }

    [UnityTest]
    public IEnumerator UsmAddStatesTest()
    {
        GameObject testObject = new GameObject();
        UniversalStateMachine uStateMachine = testObject.AddComponent<UniversalStateMachine>();
        uStateMachine.SetName("PlayerStateMachine");

        uStateMachine.AddState("isJumping");
        uStateMachine.AddState("isAlive");
        uStateMachine.AddState("isMoving");
        Debug.Log("Testing Adding states: 3 entries expected...");
        uStateMachine.LogAllStates();

        Assert.AreEqual(true, uStateMachine.StateCount() == 3);
        yield return null;
    }

    [UnityTest]
    public IEnumerator UsmTryAddingDuplicateStateTest()
    {
        GameObject testObject = new GameObject();
        UniversalStateMachine uStateMachine = testObject.AddComponent<UniversalStateMachine>();
        uStateMachine.SetName("PlayerStateMachine");

        uStateMachine.AddState("isMoving");
        uStateMachine.AddState("isMoving");
        Debug.Log("Testing Adding Duplicate States: 1 entry expected...");
        uStateMachine.LogAllStates();

        Assert.AreEqual(true, uStateMachine.StateCount() == 1);
        yield return null;
    }

    [UnityTest]
    public IEnumerator UsmTryAddingNullStateTest()
    {
        GameObject testObject = new GameObject();
        UniversalStateMachine uStateMachine = testObject.AddComponent<UniversalStateMachine>();
        uStateMachine.SetName("PlayerStateMachine");

        uStateMachine.AddState("");
        Debug.Log("Testing Adding Null States: 0 entries expected...");
        uStateMachine.LogAllStates();

        Assert.AreEqual(true, uStateMachine.StateCount() == 0);
        yield return null;
    }

    [UnityTest]
    public IEnumerator UsmRemoveStatesTest()
    {
        GameObject testObject = new GameObject();
        UniversalStateMachine uStateMachine = testObject.AddComponent<UniversalStateMachine>();
        uStateMachine.SetName("PlayerStateMachine");

        uStateMachine.AddState("isJumping");
        uStateMachine.AddState("isAlive");
        uStateMachine.AddState("isMoving");

        uStateMachine.RemoveState("isAlive");

        Debug.Log("Testing Removing States: 2 entries expected...");
        uStateMachine.LogAllStates();

        Assert.AreEqual(true, uStateMachine.StateCount() == 2);
        yield return null;
    }

    [UnityTest]
    public IEnumerator UsmTryRemovingNonexistentStatesTest()
    {
        GameObject testObject = new GameObject();
        UniversalStateMachine uStateMachine = testObject.AddComponent<UniversalStateMachine>();
        uStateMachine.SetName("PlayerStateMachine");

        uStateMachine.AddState("isJumping");
        uStateMachine.AddState("isAlive");
        uStateMachine.AddState("isMoving");

        uStateMachine.RemoveState("isInvincible");

        Debug.Log("Testing Removing nonexistent States: 3 entries expected...");
        uStateMachine.LogAllStates();

        Assert.AreEqual(true, uStateMachine.StateCount() == 3);
        yield return null;
    }

    [UnityTest]
    public IEnumerator UsmTryRemovingNullStatesTest()
    {
        GameObject testObject = new GameObject();
        UniversalStateMachine uStateMachine = testObject.AddComponent<UniversalStateMachine>();
        uStateMachine.SetName("PlayerStateMachine");

        uStateMachine.AddState("isJumping");
        uStateMachine.AddState("isAlive");
        uStateMachine.AddState("isMoving");

        uStateMachine.RemoveState("");

        Debug.Log("Testing Removing nonexistent States: 3 entries expected...");
        uStateMachine.LogAllStates();

        Assert.AreEqual(true, uStateMachine.StateCount() == 3);
        yield return null;
    }

    [UnityTest]
    public IEnumerator UsmGetStateActivityTest()
    {
        GameObject testObject = new GameObject();
        UniversalStateMachine uStateMachine = testObject.AddComponent<UniversalStateMachine>();
        uStateMachine.SetName("PlayerStateMachine");

        uStateMachine.AddState("isJumping");
        uStateMachine.AddState("isAlive");
        uStateMachine.AddState("isMoving");

        Assert.AreEqual(true, uStateMachine.GetStateActivity("isAlive") == false);
        yield return null;
    }

    [UnityTest]
    public IEnumerator UsmTryGettingNonexistentStateActivityTest()
    {
        GameObject testObject = new GameObject();
        UniversalStateMachine uStateMachine = testObject.AddComponent<UniversalStateMachine>();
        uStateMachine.SetName("PlayerStateMachine");

        Debug.Log("Testing illegal request of nonexistent state named 'isStronk'...");

        Assert.AreEqual(true, uStateMachine.GetStateActivity("isStronk") == false);
        yield return null;
    }

    [UnityTest]
    public IEnumerator UsmSetStateActivityTest()
    {
        GameObject testObject = new GameObject();
        UniversalStateMachine uStateMachine = testObject.AddComponent<UniversalStateMachine>();
        uStateMachine.SetName("PlayerStateMachine");

        uStateMachine.AddState("isJumping");
        uStateMachine.AddState("isAlive");
        uStateMachine.AddState("isMoving");

        uStateMachine.UpdateStateActivity("isAlive", true);
        uStateMachine.UpdateStateActivity("isJumping", true);
        uStateMachine.UpdateStateActivity("isMoving", true);

        Debug.Log("Testing Setting all states to true...");
        uStateMachine.LogAllStates();

        Assert.AreEqual(true, uStateMachine.GetStateActivity("isAlive") == true);
        yield return null;
    }

    [UnityTest]
    public IEnumerator UsmTrySettingNonexistentStateActivityTest()
    {
        GameObject testObject = new GameObject();
        UniversalStateMachine uStateMachine = testObject.AddComponent<UniversalStateMachine>();
        uStateMachine.SetName("PlayerStateMachine");

        Debug.Log("Testing illegal Setting of nonexistent state named 'isStronk'...");
        uStateMachine.UpdateStateActivity("isStronk", true);

        Assert.AreEqual(true, uStateMachine.StateCount() == 0);
        yield return null;
    }

    [UnityTest]
    public IEnumerator UsmGettingListOfAllStateNamesTest()
    {
        GameObject testObject = new GameObject();
        UniversalStateMachine uStateMachine = testObject.AddComponent<UniversalStateMachine>();
        uStateMachine.SetName("PlayerStateMachine");

        uStateMachine.AddState("isJumping");
        uStateMachine.AddState("isAlive");
        uStateMachine.AddState("isMoving");

        List<string> namesList = uStateMachine.GetAllStateNames();
        Debug.Log("Testing getting list of all names of states in state machine. 3 entries expected...");
        Debug.Log(namesList);

        Assert.AreEqual(true, namesList.Count == 3);
        yield return null;
    }
}
