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
    public override void Setup(){
        base.Setup();
        movement = GetComponent<Movement>();
        awareness = GetComponent<Awareness>();
        inventory = GetComponent<Inventory>();
    }

    public override void OnActivate()
    {
        base.OnActivate();
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
        base.SetupActionLayers();
        actionLayers.Add("Wood");
    }

    protected override void SetupEffects(){
        base.SetupEffects();
        effects["HoldingWood"] = true;
    }

    protected override void SetupConditions(){
        base.SetupConditions();
        preconditions["WoodNearby"] = true;
    }
}