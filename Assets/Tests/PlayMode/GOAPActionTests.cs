using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using NUnit.Framework;
using UnityEngine.TestTools;


public class GOAPActionTests 
{
    GameObject GetGameObjectMock(){
        var gameObject = new GameObject();
        gameObject.AddComponent<StateSet>();
        gameObject.AddComponent<WorldState>();
        gameObject.AddComponent<GOAPActionMock1>();
        gameObject.AddComponent<GOAPActionMock2>();
        return gameObject;
    }

    [Test]
    public void PreconditionsSatisfiedTest(){
        var obj = GetGameObjectMock();
        var worldState = obj.GetComponent<WorldState>();
        var action1 = obj.GetComponent<GOAPActionMock1>();
        Assert.IsTrue(!action1.PreconditionsSatisfied(worldState));
        foreach (var i in action1.preconditions){
            worldState.AddState(i.Key, i.Value);
        }
        Assert.IsTrue(action1.PreconditionsSatisfied(worldState));
        foreach (var i in action1.preconditions){
            worldState.RemoveBoolState(i.Key);
            break;
        }
        Assert.IsTrue(!action1.PreconditionsSatisfied(worldState));

        var action2 = obj.GetComponent<GOAPActionMock2>();
        // Empty preconditions
        Assert.IsTrue(action2.PreconditionsSatisfied(worldState));
    }

    [Test]
    public void EffectsSatisfiedTest(){
        var obj = GetGameObjectMock();
        var worldState = obj.GetComponent<WorldState>();
        var action1 = obj.GetComponent<GOAPActionMock1>();
        Assert.IsTrue(!action1.EffectsSatisfied(worldState));
        foreach (var i in action1.effects){
            worldState.AddState(i.Key, i.Value);
        }
        Assert.IsTrue(action1.EffectsSatisfied(worldState));
        foreach (var i in action1.effects){
            worldState.RemoveBoolState(i.Key);
            break;
        }
        Assert.IsTrue(!action1.EffectsSatisfied(worldState));

        var action2 = obj.GetComponent<GOAPActionMock2>();
        Assert.IsTrue(!action2.EffectsSatisfied(worldState));
        foreach (var i in action2.effects){
            worldState.AddState(i.Key, i.Value);
        }
        Assert.IsTrue(action2.EffectsSatisfied(worldState));
        foreach (var i in action2.effects){
            worldState.RemoveBoolState(i.Key);
            break;
        }
        Assert.IsTrue(!action2.EffectsSatisfied(worldState));
    }

    


}