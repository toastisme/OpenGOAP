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
    public void IntStateAddRemoveTest(){

        WorldState worldState = GetWorldStateMock();

        string testInt = "testInt";
        Assert.IsTrue(worldState.InIntStates("testInt") == false);
        worldState.AddState(testInt, 1);
        Assert.IsTrue(worldState.InIntStates("testInt") == true);
        Assert.IsTrue(worldState.InSet("testInt", 1));
        Assert.IsTrue(!worldState.InSet("testInt", 0));
        Assert.IsTrue(worldState.GetIntState("testInt") == 1);
        worldState.RemoveIntState("testInt");
        Assert.IsTrue(worldState.InIntStates("testInt") == false);
    }

    [Test]
    public void IntStateUpdateTest(){

        WorldState worldState = GetWorldStateMock();
        string testInt = "testInt";
        worldState.AddState(testInt, 3);
        Assert.IsTrue(Mathf.Abs(worldState.GetIntState("testInt") - 3) == 0);
        worldState.UpdateState("testInt", 1);
        Assert.IsTrue(Mathf.Abs(worldState.GetIntState("testInt") - 4) == 0);
        worldState.UpdateState("testInt", -1);
        Assert.IsTrue(Mathf.Abs(worldState.GetIntState("testInt") - 3) == 0);
    }

    [Test]
    public void Vector2StateAddRemoveTest(){

        WorldState worldState = GetWorldStateMock();

        string testVector2 = "testVector2";
        Assert.IsTrue(worldState.InVector2States("testVector2") == false);
        worldState.AddState(testVector2, new Vector2(1,2));
        Assert.IsTrue(worldState.InVector2States("testVector2") == true);
        Assert.IsTrue(worldState.InSet("testVector2", new Vector2(1,2)));
        Assert.IsTrue(!worldState.InSet("testVector2", new Vector2(1,1)));
        Assert.IsTrue(worldState.GetVector2State("testVector2") == new Vector2(1,2));
        worldState.RemoveVector2State("testVector2");
        Assert.IsTrue(worldState.InVector2States("testVector2") == false);
    }

    [Test]
    public void Vector2StateUpdateTest(){

        Vector2 vec = new Vector2(1,2);

        WorldState worldState = GetWorldStateMock();
        string testVector2 = "testVector2";
        worldState.AddState(testVector2, vec);
        Assert.IsTrue(
            (worldState.GetVector2State("testVector2") - vec).sqrMagnitude < 1e-7);
        worldState.UpdateState("testVector2", vec);
        Assert.IsTrue(
            (worldState.GetVector2State("testVector2") - (vec+vec)).sqrMagnitude < 1e-7);
        worldState.UpdateState("testVector2", -vec);
        Assert.IsTrue(
            (worldState.GetVector2State("testVector2") - vec).sqrMagnitude < 1e-7);
    }

    [Test]
    public void Vector3StateAddRemoveTest(){

        WorldState worldState = GetWorldStateMock();

        string testVector3 = "testVector3";
        Assert.IsTrue(worldState.InVector3States("testVector3") == false);
        worldState.AddState(testVector3, new Vector3(1,2));
        Assert.IsTrue(worldState.InVector3States("testVector3") == true);
        Assert.IsTrue(worldState.InSet("testVector3", new Vector3(1,2)));
        Assert.IsTrue(!worldState.InSet("testVector3", new Vector3(1,1)));
        Assert.IsTrue(worldState.GetVector3State("testVector3") == new Vector3(1,2));
        worldState.RemoveVector3State("testVector3");
        Assert.IsTrue(worldState.InVector3States("testVector3") == false);
    }

    [Test]
    public void Vector3StateUpdateTest(){

        Vector3 vec = new Vector3(1,2,3);

        WorldState worldState = GetWorldStateMock();
        string testVector3 = "testVector3";
        worldState.AddState(testVector3, vec);
        Assert.IsTrue(
            (worldState.GetVector3State("testVector3") - vec).sqrMagnitude < 1e-7);
        worldState.UpdateState("testVector3", vec);
        Assert.IsTrue(
            (worldState.GetVector3State("testVector3") - (vec+vec)).sqrMagnitude < 1e-7);
        worldState.UpdateState("testVector3", -vec);
        Assert.IsTrue(
            (worldState.GetVector3State("testVector3") - vec).sqrMagnitude < 1e-7);
    }

    [Test]
    public void StringStateAddRemoveTest(){

        WorldState worldState = GetWorldStateMock();

        string testString = "testString";
        Assert.IsTrue(worldState.InStringStates("testString") == false);
        worldState.AddState(testString, "testStringVal");
        Assert.IsTrue(worldState.InStringStates("testString") == true);
        Assert.IsTrue(worldState.InSet("testString", "testStringVal"));
        Assert.IsTrue(!worldState.InSet("testString", "testStringVal2"));
        Assert.IsTrue(worldState.GetStringState("testString") == "testStringVal");
        worldState.RemoveStringState("testString");
        Assert.IsTrue(worldState.InStringStates("testString") == false);
    }

    [Test]
    public void GameObjectStateAddRemoveTest(){

        WorldState worldState = GetWorldStateMock();

        var gameObject = new GameObject();
        var gameObject2 = new GameObject();

        string testGameObject = "testGameObject";
        Assert.IsTrue(worldState.InGameObjectStates("testGameObject") == false);
        worldState.AddState(testGameObject, gameObject);
        Assert.IsTrue(worldState.InGameObjectStates("testGameObject") == true);
        Assert.IsTrue(worldState.InSet("testGameObject", gameObject));
        Assert.IsTrue(!worldState.InSet("testGameObject", gameObject2));
        Assert.IsTrue(worldState.GetGameObjectState("testGameObject") == gameObject);
        worldState.RemoveGameObjectState("testGameObject");
        Assert.IsTrue(worldState.InGameObjectStates("testGameObject") == false);
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
    public void IsSubsetIntTest(){
        WorldState worldState = GetWorldStateMock();
        worldState.AddState("testInt1", 1);
        worldState.AddState("testInt2", 2);
        Dictionary<string, int> states = new Dictionary<string, int>();
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testInt1"] = 1;
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testInt2"] = 2;
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testInt2"] = 3;
        Assert.IsTrue(!worldState.IsSubset(states));        
        states["testInt2"] = 2;
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testInt3"] = 1;
        Assert.IsTrue(!worldState.IsSubset(states));        
    }

    [Test]
    public void IsSubsetVector2Test(){
        WorldState worldState = GetWorldStateMock();
        worldState.AddState("testVector21", new Vector2(1,1));
        worldState.AddState("testVector22", new Vector2(2,2));
        Dictionary<string, Vector2> states = new Dictionary<string, Vector2>();
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testVector21"] = new Vector2(1,1);
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testVector22"] = new Vector2(2,2);
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testVector22"] = new Vector2(3,3);
        Assert.IsTrue(!worldState.IsSubset(states));        
        states["testVector22"] = new Vector2(2,2);
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testVector23"] = new Vector2(1,1);
        Assert.IsTrue(!worldState.IsSubset(states));        
    }

    [Test]
    public void IsSubsetVector3Test(){
        WorldState worldState = GetWorldStateMock();
        worldState.AddState("testVector31", new Vector3(1,1,1));
        worldState.AddState("testVector32", new Vector3(2,2,2));
        Dictionary<string, Vector3> states = new Dictionary<string, Vector3>();
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testVector31"] = new Vector3(1,1,1);
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testVector32"] = new Vector3(2,2,2);
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testVector32"] = new Vector3(3,3,3);
        Assert.IsTrue(!worldState.IsSubset(states));        
        states["testVector32"] = new Vector3(2,2,2);
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testVector33"] = new Vector3(1,1,1);
        Assert.IsTrue(!worldState.IsSubset(states));        
    }

    [Test]
    public void IsSubsetStringTest(){
        WorldState worldState = GetWorldStateMock();
        worldState.AddState("testString1", "val1");
        worldState.AddState("testString2", "val2");
        Dictionary<string, string> states = new Dictionary<string, string>();
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testString1"] = "val1";
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testString2"] = "val2";
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testString2"] = "val3";
        Assert.IsTrue(!worldState.IsSubset(states));        
        states["testString2"] = "val2";
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testString3"] = "val1";
        Assert.IsTrue(!worldState.IsSubset(states));        
    }

    [Test]
    public void IsSubsetGameObjectTest(){
        WorldState worldState = GetWorldStateMock();
        GameObject obj1 = new GameObject();
        GameObject obj2 = new GameObject();
        GameObject obj3 = new GameObject();
        worldState.AddState("testGameObject1", obj1);
        worldState.AddState("testGameObject2", obj2);
        Dictionary<string, GameObject> states = new Dictionary<string, GameObject>();
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testGameObject1"] = obj1;
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testGameObject2"] = obj2;
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testGameObject2"] = obj3;
        Assert.IsTrue(!worldState.IsSubset(states));        
        states["testGameObject2"] = obj2;
        Assert.IsTrue(worldState.IsSubset(states));        
        states["testGameObject3"] = obj1;
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
