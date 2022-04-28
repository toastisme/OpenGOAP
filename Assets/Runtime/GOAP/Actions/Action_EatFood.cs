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

    public override void Setup(){
        base.Setup();
        inventory = GetComponent<Inventory>();
    }

    public override void OnActivated(){
        worldState.AddState("ReducedHunger", false);
    }

    public override void OnDeactivated(){
        worldState.AddState("ReducedHunger", false);
    }

    public override void OnTick()
    {
        if(PreconditionsSatisfied()){
            inventory.Remove("Food", 1);
            worldState.AddState("Hunger", 0f);
            worldState.AddState("ReducedHunger", true);
        }
        else{
            StopAction();
        }
    }

    protected override void SetupActionLayers(){
        base.SetupActionLayers();
        actionLayers.Add("Food");
    }

    protected override void SetupEffects(){
        base.SetupEffects();
        effects["ReducedHunger"] = true;
    }

    protected override void SetupConditions(){
        base.SetupConditions();
        preconditions["HoldingFood"] = true;
    }
}