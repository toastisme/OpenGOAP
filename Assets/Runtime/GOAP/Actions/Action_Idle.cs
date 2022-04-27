using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class Action_Idle : GOAPAction
{
    public override float GetCost(){
        return 0.0f;
    }

    public override void Setup(){
        base.Setup();
        effects["WasIdle"] = true;
    }

    public override void OnDeactivated(){
        worldState.RemoveBoolState("WasIdle");
    }

    public override void OnTick(){
        worldState.AddState("WasIdle", true);
    }
}