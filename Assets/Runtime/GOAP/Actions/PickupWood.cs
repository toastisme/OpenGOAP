using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sensors;

namespace GOAP{
public class PickUpWood : GOAPAction
{
    Movement movement;
    Awareness awareness;
    SmartObject targetWood;
    
    public override float GetCost(){
        return 0.0f;
    }
    public override void Setup(ref WorldState worldState, ref Inventory inventory){
        base.Setup(ref worldState, ref inventory);
        movement = GetComponent<Movement>();
        awareness = GetComponent<Awareness>();
        this.worldState = worldState;
        preconditions["WoodNearby"] = true;
        worldState.boolKeys["WoodNearby"] = awareness.Nearby("Wood");
        effects["HoldingWood"] = true;
        worldState.boolKeys["HoldingWood"] = inventory.Contains("Wood");
    }

    public override void OnActivated()
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
        if (CanRun()){
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
}