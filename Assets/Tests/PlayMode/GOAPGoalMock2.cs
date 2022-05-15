using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class GOAPGoalMock2 : GOAPGoal
{
    protected override void SetupDerived(){
        actionLayer = "Mock";
        conditions["mockGoalCondition2"] = true;
        preconditions["mockGoalPrecondtion2"] = true;
    }

    public override float GetPriority()
    {
        return 0.8f;
    }
}
