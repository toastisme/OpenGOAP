using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GOAP;
using Sensors;


[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Memory))]
public class Action_GetFoodFromStore : GOAPAction
{

    Inventory inventory;
    Movement movement;
    Memory memory;
    SmartObject foodStore;

    public override float GetCost(){
        return 0.1f * worldState.GetFloatState("Fatigue");
    }
    public override void Setup(){
        base.Setup();
        movement = GetComponent<Movement>();
        inventory = GetComponent<Inventory>();
        memory = GetComponent<Memory>();
        preconditions["g_FoodAvailable"] = true;
        effects["HoldingFood"] = true;
    }

    public override void OnActivated(){
        foodStore = (SmartObject)memory.RememberNearest("FoodStore");
        if (foodStore == null){
            StopAction();
            return;
        }
        movement.GoTo(foodStore);
    }

    public override void OnDeactivated(){
    }

    public override void OnTick()
    {
        if(PreconditionsSatisfied()){
            movement.GoTo(foodStore);
            if (movement.AtTarget()){
                inventory.Add(foodStore.Extract(5f));
            }
        }
    }

    public override bool PreconditionsSatisfied()
    {
        bool result = base.PreconditionsSatisfied();
        if (!result){
            return result;
        }
        else{
            return (foodStore != null);
        }
    }

}