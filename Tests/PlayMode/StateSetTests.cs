
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using NUnit.Framework;
using UnityEngine.TestTools;


public class StateSetTests 
{
    StateSet GetStateSetMock(){
        var gameObject = new GameObject();
        gameObject.AddComponent<StateSet>();
        return gameObject.GetComponent<StateSet>();
    }

    [Test]
    public void BoolStateAddRemoveTest(){

        StateSet stateSet = GetStateSetMock();

        string testBool = "testBool";
        Assert.IsTrue(stateSet.InBoolStates("testBool") == false);
        stateSet.AddState(testBool, true);
        Assert.IsTrue(stateSet.InBoolStates("testBool") == true);
        Assert.IsTrue(stateSet.InSet("testBool", true));
        Assert.IsTrue(!stateSet.InSet("testBool", false));
        Assert.IsTrue(stateSet.GetBoolState("testBool") == true);
        stateSet.RemoveBoolState("testBool");
        Assert.IsTrue(stateSet.InBoolStates("testBool") == false);
    }

    [Test]
    public void FloatStateAddRemoveTest(){

        StateSet stateSet = GetStateSetMock();

        string testFloat = "testFloat";
        Assert.IsTrue(stateSet.InFloatStates("testFloat") == false);
        stateSet.AddState(testFloat, 3f);
        Assert.IsTrue(stateSet.InFloatStates("testFloat") == true);
        Assert.IsTrue(Mathf.Abs(stateSet.GetFloatState("testFloat") - 3f) < float.Epsilon);
        stateSet.UpdateState("testFloat", 1f);
        Assert.IsTrue(Mathf.Abs(stateSet.GetFloatState("testFloat") - 4f) < float.Epsilon);
        stateSet.RemoveFloatState("testFloat");
        Assert.IsTrue(stateSet.InFloatStates("testFloat") == false);
    }

    [Test]
    public void FloatStateUpdateTest(){

        StateSet stateSet = GetStateSetMock();
        string testFloat = "testFloat";
        stateSet.AddState(testFloat, 3f);
        Assert.IsTrue(Mathf.Abs(stateSet.GetFloatState("testFloat") - 3f) < float.Epsilon);
        stateSet.UpdateState("testFloat", 1f);
        Assert.IsTrue(Mathf.Abs(stateSet.GetFloatState("testFloat") - 4f) < float.Epsilon);
        stateSet.UpdateState("testFloat", -1f);
        Assert.IsTrue(Mathf.Abs(stateSet.GetFloatState("testFloat") - 3f) < float.Epsilon);
    }

    [Test]
    public void IsSubsetBoolTest(){
        StateSet stateSet = GetStateSetMock();
        // Absent key treated the same as key = false by default
        stateSet.SetDefaultFalse(true);

        stateSet.AddState("testBool1", true);
        stateSet.AddState("testBool2", false);
        Dictionary<string, bool> states = new Dictionary<string, bool>();
        Assert.IsTrue(stateSet.IsSubset(states));        
        states["testBool1"] = true;
        Assert.IsTrue(stateSet.IsSubset(states));        
        states["testBool2"] = false;
        Assert.IsTrue(stateSet.IsSubset(states));        
        states["testBool2"] = true;
        Assert.IsTrue(!stateSet.IsSubset(states));        
        states["testBool2"] = false;
        Assert.IsTrue(stateSet.IsSubset(states));        
        states["testBool3"] = false;
        Assert.IsTrue(stateSet.IsSubset(states));        
        states["testBool3"] = true;
        Assert.IsTrue(!stateSet.IsSubset(states));        
    }

    [Test]
    public void IsSubsetFloatTest(){
        StateSet stateSet = GetStateSetMock();
        stateSet.AddState("testFloat1", 1f);
        stateSet.AddState("testFloat2", 2f);
        Dictionary<string, float> states = new Dictionary<string, float>();
        Assert.IsTrue(stateSet.IsSubset(states));        
        states["testFloat1"] = 1f;
        Assert.IsTrue(stateSet.IsSubset(states));        
        states["testFloat2"] = 2f;
        Assert.IsTrue(stateSet.IsSubset(states));        
        states["testFloat2"] = 3f;
        Assert.IsTrue(!stateSet.IsSubset(states));        
        states["testFloat2"] = 2f;
        Assert.IsTrue(stateSet.IsSubset(states));        
        states["testFloat3"] = 1f;
        Assert.IsTrue(!stateSet.IsSubset(states));        
    }

    [Test]
    public void DefaultFalseTest(){
        StateSet stateSet = GetStateSetMock();
        stateSet.AddState("testBool1", true);

        Dictionary<string, bool> states = new Dictionary<string, bool>();
        states["testBool1"] = true;
        states["testBool2"] = false;
        Assert.IsTrue(stateSet.IsSubset(states));

        stateSet.SetDefaultFalse(true);
        Assert.IsTrue(stateSet.IsSubset(states));

        stateSet.SetDefaultFalse(false);
        Assert.IsTrue(!stateSet.IsSubset(states));
        stateSet.SetDefaultFalse(true);
        Assert.IsTrue(stateSet.IsSubset(states));
    }

}
