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
    protected override void SetupDerived(){
        movement = GetComponent<Movement>();
        inventory = GetComponent<Inventory>();
        memory = GetComponent<Memory>();
        foodStore = (SmartObject)memory.RememberNearest("FoodStore");
    }

    protected override void OnActivateDerived(){
        foodStore = (SmartObject)memory.RememberNearest("FoodStore");
        if (foodStore == null){
            StopAction();
            return;
        }
        movement.GoTo(foodStore);
    }

    protected override void OnDeactivateDerived(){
        worldState.RemoveBoolState("FoodRemovedFromStore");
    }

    public override void OnTick()
    {
        movement.GoTo(foodStore);
        if (movement.AtTarget()){
            inventory.Add(
                foodStore.Extract(
                    worldState.GetFloatState("FoodExtractValue")
                )
            );
            worldState.AddState("FoodRemovedFromStore", true);
        }
    }

    public override bool PreconditionsSatisfied(WorldState worldState)
    {
        bool result = base.PreconditionsSatisfied(worldState);
        if (!result){
            return result;
        }
        else{
            return (foodStore != null);
        }
    }

    protected override void SetupActionLayers(){
        actionLayers.Add("Food");
    }
    protected override void SetupEffects(){
        effects["HoldingFood"] = true;
        effects["FoodRemovedFromStore"] = true;
    }
    protected override void SetupConditions(){
        preconditions["g_FoodAvailable"] = true;
    }

}