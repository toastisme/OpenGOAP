using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sensors;

namespace GOAP{
public class PickUpWood : GOAPAction
{
    Movement movement;
    Vision vision;
    GameObject targetWood;
    
    public override float GetCost(){
        return 0.0f;
    }
    public override void Setup(ref WorldState worldState){
        movement = GetComponent<Movement>();
        vision = GetComponent<Vision>();
        this.worldState = worldState;
        requiredState = new WorldState();
        requiredState.boolKeys["WoodNearby"] = true;
        outputState = new WorldState();
        outputState.boolKeys["HoldingWood"] = true;
    }

    public override void OnActivated()
    {
        //targetWood = vision.GetNearest(objectTag:"Wood");
        movement.GoTo(targetWood);
    }

    

    public override void OnTick(){
        /*
        if (targetWood == null){
            targetWood = vision.GetNearest(objectTag:"Wood");
            movement.GoTo(targetWood);
        } 
        if (movement.AtTarget()){
            worldState.boolKeys["HoldingWood"] = true;
        }
        */
    }
}
}