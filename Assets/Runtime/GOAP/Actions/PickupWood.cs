using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sensors;
using GOAP;

public class PickUpWood : GOAPAction
{
    Movement movement;
    Awareness awareness;
    SmartObject targetWood;
    Inventory inventory;
    
    public override float GetCost(){
        return 0.0f;
    }
    public override void Setup(){
        base.Setup();
        movement = GetComponent<Movement>();
        awareness = GetComponent<Awareness>();
        inventory = GetComponent<Inventory>();
        preconditions["WoodNearby"] = true;
        effects["HoldingWood"] = true;
    }

    public override void OnActivated()
    {
        base.OnActivated();
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
        if (PreconditionsSatisfied()){
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
    }
}