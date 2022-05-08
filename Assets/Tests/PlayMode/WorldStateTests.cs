using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using NUnit.Framework;
using UnityEngine.TestTools;


public class WorldStateTests 
{
    WorldState GetWorldStateMock(){
        var gameObject = new GameObject();
        gameObject.AddComponent<StateSet>();
        gameObject.AddComponent<WorldState>();
        return gameObject.GetComponent<WorldState>();
    }

    [Test]
    public void BoolStateAddRemoveTest(){

        WorldState worldState = GetWorldStateMock();

        string testBool = "testBool";
        Assert.IsTrue(worldState.InBoolStates("testBool") == false);
        worldState.AddState(testBool, true);
        Assert.IsTrue(worldState.InBoolStates("testBool") == true);
        Assert.IsTrue(worldState.InSet("testBool", true));
        Assert.IsTrue(!worldState.InSet("testBool", false));
        Assert.IsTrue(worldState.GetBoolState("testBool") == true);
        worldState.RemoveBoolState("testBool");
        Assert.IsTrue(worldState.InBoolStates("testBool") == false);
    }

    [Test]
    public void FloatStateAddRemoveTest(){

        WorldState worldState = GetWorldStateMock();

        string testFloat = "testFloat";
        Assert.IsTrue(worldState.InFloatStates("testFloat") == false);
        worldState.AddState(testFloat, 3f);
        Assert.IsTrue(worldState.InFloatStates("testFloat") == true);
        Assert.IsTrue(Mathf.Abs(worldState.GetFloatState("testFloat") - 3f) < float.Epsilon);
        worldState.UpdateState("testFloat", 1f);
        Assert.IsTrue(Mathf.Abs(worldState.GetFloatState("testFloat") - 4f) < float.Epsilon);
        worldState.RemoveFloatState("testFloat");
        Assert.IsTrue(worldState.InFloatStates("testFloat") == false);
    }

    [Test]
    public void FloatStateUpdateTest(){

        WorldState worldState = GetWorldStateMock();
        string testFloat = "testFloat";
        worldState.AddState(testFloat, 3f);
        Assert.IsTrue(Mathf.Abs(worldState.GetFloatState("testFloat") - 3f) < float.Epsilon);
        worldState.UpdateState("testFloat", 1f);
        Assert.IsTrue(Mathf.Abs(worldState.GetFloatState("testFloat") - 4f) < float.Epsilon);
        worldState.UpdateState("testFloat", -1f);
        Assert.IsTrue(Mathf.Abs(worldState.GetFloatState("testFloat") - 3f) < float.Epsilon);
    }

    [Test]
    public void IsSubsetBoolTest(){
        WorldState worldState = GetWorldStateMock();
        // Absent key treated the same as key = false by default
        worldState.SetLocalDefaultFalse(true);

        worldState.AddState("testBool1", true);
        worldState.AddState("testBool2", false);
        Dictionary<string, bool> states = new Dictionary<string, bool>();
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testBool1"] = true;
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testBool2"] = false;
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testBool2"] = true;
        Assert.IsTrue(!worldState.IsSubset(states));        
        states["testBool2"] = false;
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testBool3"] = false;
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testBool3"] = true;
        Assert.IsTrue(!worldState.IsSubset(states));        
    }

    [Test]
    public void IsSubsetFloatTest(){
        WorldState worldState = GetWorldStateMock();
        worldState.AddState("testFloat1", 1f);
        worldState.AddState("testFloat2", 2f);
        Dictionary<string, float> states = new Dictionary<string, float>();
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testFloat1"] = 1f;
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testFloat2"] = 2f;
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testFloat2"] = 3f;
        Assert.IsTrue(!worldState.IsSubset(states));        
        states["testFloat2"] = 2f;
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testFloat3"] = 1f;
        Assert.IsTrue(!worldState.IsSubset(states));        
    }

    [Test]
    public void DefaultFalseTest(){
        WorldState worldState = GetWorldStateMock();
        worldState.AddState("testBool1", true);

        Dictionary<string, bool> states = new Dictionary<string, bool>();
        states["testBool1"] = true;
        states["testBool2"] = false;
        Assert.IsTrue(worldState.IsSubset(states));

        worldState.SetLocalDefaultFalse(true);
        worldState.SetGlobalDefaultFalse(true);
        Assert.IsTrue(worldState.IsSubset(states));

        worldState.SetLocalDefaultFalse(false);
        Assert.IsTrue(!worldState.IsSubset(states));
        worldState.SetLocalDefaultFalse(true);
        Assert.IsTrue(worldState.IsSubset(states));
        worldState.SetGlobalDefaultFalse(false);
        Assert.IsTrue(worldState.IsSubset(states));
    }

}
