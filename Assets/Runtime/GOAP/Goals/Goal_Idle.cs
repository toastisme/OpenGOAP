using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class Goal_Idle : GOAPGoal
{

    public override void Setup(){
        base.Setup();
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