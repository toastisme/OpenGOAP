
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using NUnit.Framework;
using UnityEngine.TestTools;


public class GOAPPlannerTests 
{
    GameObject GetGameObjectMock(){
        var gameObject = new GameObject();
        gameObject.AddComponent<StateSet>();
        gameObject.AddComponent<WorldState>();
        gameObject.AddComponent<GOAPGoalMock1>();
        gameObject.AddComponent<GOAPGoalMock2>();
        gameObject.AddComponent<GOAPActionMock1>();
        gameObject.AddComponent<GOAPActionMock2>();
        gameObject.AddComponent<GOAPPlanner>();
        return gameObject;
    }

    [UnityTest]
    public IEnumerator EndToEndTest(){

        var gameObject = GetGameObjectMock();
        var planner = gameObject.GetComponent<GOAPPlanner>();
        var worldState = gameObject.GetComponent<WorldState>();
        worldState.AddState("mockGoalPrecondition1", true);
        yield return null;
        /* 
            Expected:
            - GOAPGoalMock1 as activeGoal
            - activePlan as GOAPAction2 -> GOAPAction1
        */
        Assert.IsTrue(planner.activeGoal.GetType() ==  typeof(GOAPGoalMock1));
        Assert.AreEqual(planner.activePlan.Count, 2);
        Assert.IsTrue(planner.activePlan[0].GetType() == typeof(GOAPActionMock2));
        Assert.IsTrue(planner.activePlan[1].GetType() == typeof(GOAPActionMock1));
        Assert.AreEqual(planner.activeActionIdx, 0);

        yield return null;
        // Move to next GOAPAction
        Assert.IsTrue(worldState.GetBoolState("mockActionCondition1"));
        Assert.AreEqual(planner.activePlan.Count, 2);
        Assert.AreEqual(planner.activeActionIdx, 1);
        // GOAPActionMock1 adds a temporary action which should be removed
        Assert.IsTrue(!worldState.InBoolStates("mockTemporaryState"));

        yield return null;
        // Complete plan
        Assert.IsTrue(worldState.GetBoolState("mockGoalCondition1"));
    }


}