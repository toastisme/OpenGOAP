using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP{
public class GoalIdle : Goal
{

    public override void Setup(){
        base.Setup();
        conditions["WasIdle"] = true;
    }

    public override float GetPriority()
    {
        return .1f;
    }

    public override bool CanRun(){
        return true;
    }
    
}
}