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
    Detectable targetWood;
    
    public override float GetCost(){
        return 0.0f;
    }
    public override void Setup(ref WorldState worldState){
        movement = GetComponent<Movement>();
        awareness = GetComponent<Awareness>();
        this.worldState = worldState;
        requiredState = new WorldState();
        requiredState.boolKeys["WoodNearby"] = true;
        outputState = new WorldState();
        outputState.boolKeys["HoldingWood"] = true;
    }

    public override void OnActivated()
    {
        targetWood = awareness.GetNearest("Wood");
        if (targetWood != null){
            movement.GoTo(targetWood);
        }

    }

    public override void OnTick(){
        if (CanRun()){
            if (targetWood == null){
                targetWood = awareness.GetNearest("Wood");
                if (targetWood == null)
                {
                    StopAction();
                    return;
                }
            } 
            movement.GoTo(targetWood);
            if (movement.AtTarget()){
                worldState.boolKeys["HoldingWood"] = true;
            }
        }
    }
}
}