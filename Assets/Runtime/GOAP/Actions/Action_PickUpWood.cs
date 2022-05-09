using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sensors;
using GOAP;

public class Action_PickUpWood : GOAPAction
{
    Movement movement;
    Awareness awareness;
    SmartObject targetWood;
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
        targetWood = (SmartObject)awareness.GetNearest("Wood");
        if (targetWood != null){
            movement.GoTo(targetWood);
        }
        else{
            StopAction();
            return;
        }
    }

    public override void OnTick(){
        if (targetWood == null){
            targetWood = (SmartObject)awareness.GetNearest("Wood");
            if (targetWood == null)
            {
                StopAction();
                return;
            }
        } 
        movement.GoTo(targetWood);
        if (movement.AtTarget()){
            inventory.Add(targetWood);
        }
    }

    protected override void SetupActionLayers(){
        actionLayers.Add("Wood");
    }

    protected override void SetupEffects(){
        effects["HoldingWood"] = true;
    }

    protected override void SetupConditions(){
        preconditions["WoodNearby"] = true;
    }
}