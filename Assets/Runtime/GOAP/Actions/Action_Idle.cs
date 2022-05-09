using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class Action_Idle : GOAPAction
{
    public override float GetCost(){
        return 0.0f;
    }

    protected override void OnDeactivateDerived(){
        worldState.RemoveBoolState("WasIdle");
    }

    public override void OnTick(){
        worldState.AddState("WasIdle", true);
    }

    protected override void SetupActionLayers()
    {
        actionLayers.Add("Idle");
    }

    protected override void SetupEffects(){
        effects["WasIdle"] = true;
    }
}