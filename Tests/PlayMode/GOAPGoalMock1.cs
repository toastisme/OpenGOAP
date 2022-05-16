using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class GOAPGoalMock1 : GOAPGoal
{
    protected override void SetupDerived(){
        actionLayer = "Mock";
        conditions["mockGoalCondition1"] = true;
        preconditions["mockGoalPrecondition1"] = true;
    }

    public override float GetPriority()
    {
        return 1.0f;
    }
}
