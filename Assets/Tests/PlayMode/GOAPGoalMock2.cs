using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class GOAPGoalMock2 : GOAPGoal
{
    protected override void SetupDerived(){
        actionLayer = "Mock";
        conditions["mockCondition2"] = true;
        preconditions["mockPrecondtion2"] = true;
    }

    public override float GetPriority()
    {
        return 0.8f;
    }
}
