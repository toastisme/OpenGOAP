
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sensors;
using GOAP;

public class Action_PickUpFood : GOAPAction
{
    Movement movement;
    Awareness awareness;
    SmartObject targetFood;
    Inventory inventory;
    
    public override float GetCost(){
        return 0.0f;
    }
    protected override void SetupDerived(){
        movement = GetComponent<Movement>();
        awareness = GetComponent<Awareness>();
        inventory = GetComponent<Inventory>();
    }

    protected override void OnActivateDerived()
    {
        targetFood = (SmartObject)awareness.GetNearest("Food");
        if (targetFood != null){
            movement.GoTo(targetFood);
        }
        else{
            StopAction();
            return;
        }
    }

    public override void OnTick(){
        if (targetFood == null){
            targetFood = (SmartObject)awareness.GetNearest("Food");
            if (targetFood == null)
            {
                StopAction();
                return;
            }
        } 
        movement.GoTo(targetFood);
        if (movement.AtTarget()){
            inventory.Add(targetFood);
        }
    }

    protected override void SetupActionLayers(){
        actionLayers.Add("Food");
    }

    protected override void SetupEffects(){
        effects["HoldingFood"] = true;
    }

    protected override void SetupConditions(){
        preconditions["FoodNearby"] = true;
    }
}