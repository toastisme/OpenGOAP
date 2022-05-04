using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class Action_Idle : GOAPAction
{
    public override float GetCost(){
        return 0.0f;
    }

    public override void OnDeactivate(){
        worldState.RemoveBoolState("WasIdle");
    }

    public override void OnTick(){
        worldState.AddState("WasIdle", true);
    }

    protected override void SetupActionLayers()
    {
        base.SetupActionLayers();
        actionLayers.Add("Idle");
    }

    protected override void SetupEffects(){
        base.SetupEffects();
        effects["WasIdle"] = true;
    }
}