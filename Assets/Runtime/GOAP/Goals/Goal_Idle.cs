using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class Goal_Idle : GOAPGoal
{

    protected override void SetupDerived(){
        conditions["WasIdle"] = true;
        actionLayer = "Idle";
    }

    public override float GetPriority()
    {
        return 0f;
    }

    public override bool PreconditionsSatisfied(WorldState worldState){
        return true;
    }
    
}