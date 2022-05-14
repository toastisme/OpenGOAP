
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using NUnit.Framework;
using UnityEngine.TestTools;


public class GOAPGoalTests 
{
    GameObject GetGameObjectMock(){
        var gameObject = new GameObject();
        gameObject.AddComponent<StateSet>();
        gameObject.AddComponent<WorldState>();
        gameObject.AddComponent<GOAPGoalMock1>();
        gameObject.AddComponent<GOAPGoalMock2>();
        return gameObject;
    }

    [Test]
    public void PreconditionsSatisfiedTest(){
        var obj = GetGameObjectMock();
        var worldState = obj.GetComponent<WorldState>();
        var goal1 = obj.GetComponent<GOAPGoalMock1>();
        Assert.IsTrue(!goal1.PreconditionsSatisfied(worldState));
        foreach (var i in goal1.preconditions){
            worldState.AddState(i.Key, i.Value);
        }
        Assert.IsTrue(goal1.PreconditionsSatisfied(worldState));
        foreach (var i in goal1.preconditions){
            worldState.RemoveBoolState(i.Key);
            break;
        }
        Assert.IsTrue(!goal1.PreconditionsSatisfied(worldState));

        var goal2 = obj.GetComponent<GOAPGoalMock2>();
        Assert.IsTrue(!goal2.PreconditionsSatisfied(worldState));
        foreach (var i in goal2.preconditions){
            worldState.AddState(i.Key, i.Value);
        }
        Assert.IsTrue(goal2.PreconditionsSatisfied(worldState));
        foreach (var i in goal2.preconditions){
            worldState.RemoveBoolState(i.Key);
            break;
        }
        Assert.IsTrue(!goal2.PreconditionsSatisfied(worldState));
    }

    [Test]
    public void ConditionsSatisfiedTest(){
        var obj = GetGameObjectMock();
        var worldState = obj.GetComponent<WorldState>();
        var goal1 = obj.GetComponent<GOAPGoalMock1>();
        Assert.IsTrue(!goal1.ConditionsSatisfied(worldState));
        foreach (var i in goal1.conditions){
            worldState.AddState(i.Key, i.Value);
        }
        Assert.IsTrue(goal1.ConditionsSatisfied(worldState));
        foreach (var i in goal1.conditions){
            worldState.RemoveBoolState(i.Key);
            break;
        }
        Assert.IsTrue(!goal1.ConditionsSatisfied(worldState));

        var goal2 = obj.GetComponent<GOAPGoalMock2>();
        Assert.IsTrue(!goal2.ConditionsSatisfied(worldState));
        foreach (var i in goal2.conditions){
            worldState.AddState(i.Key, i.Value);
        }
        Assert.IsTrue(goal2.ConditionsSatisfied(worldState));
        foreach (var i in goal2.conditions){
            worldState.RemoveBoolState(i.Key);
            break;
        }
        Assert.IsTrue(!goal2.ConditionsSatisfied(worldState));
    }

}