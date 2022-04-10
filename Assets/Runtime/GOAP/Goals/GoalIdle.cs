using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class GoalIdle : Goal
{

    public override void Setup(){
        base.Setup();
    }

    public override float GetPriority()
    {
        return 0f;
    }

    public override bool PreconditionsSatisfied(){
        return true;
    }
    
}