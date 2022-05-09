using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GOAP;

public class Action_EatFood : GOAPAction
{

    Inventory inventory;

    public override float GetCost(){
        return 0.0f;
    }

    protected override void SetupDerived(){
        inventory = GetComponent<Inventory>();
    }

    protected override void OnActivateDerived(){
        worldState.AddState("ReducedHunger", false);
    }

    protected override void OnDeactivateDerived(){
        worldState.AddState("ReducedHunger", false);
    }

    public override void OnTick()
    {
        inventory.Remove("Food", 1, true);
        worldState.AddState("Hunger", 0f);
        worldState.AddState("ReducedHunger", true);
    }

    protected override void SetupActionLayers(){
        actionLayers.Add("Food");
    }

    protected override void SetupEffects(){
        effects["ReducedHunger"] = true;
    }

    protected override void SetupConditions(){
        preconditions["HoldingFood"] = true;
    }
}