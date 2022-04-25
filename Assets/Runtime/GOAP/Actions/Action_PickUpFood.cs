
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
    public override void Setup(){
        base.Setup();
        movement = GetComponent<Movement>();
        awareness = GetComponent<Awareness>();
        inventory = GetComponent<Inventory>();
        preconditions["FoodNearby"] = true;
        effects["HoldingFood"] = true;
    }

    public override void OnActivated()
    {
        base.OnActivated();
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
}