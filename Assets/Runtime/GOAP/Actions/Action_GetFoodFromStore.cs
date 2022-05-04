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
        foodStore = (SmartObject)memory.RememberNearest("FoodStore");
    }

    public override void OnActivate(){
        foodStore = (SmartObject)memory.RememberNearest("FoodStore");
        if (foodStore == null){
            StopAction();
            return;
        }
        movement.GoTo(foodStore);
    }

    public override void OnDeactivate(){
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
        base.SetupActionLayers();
        actionLayers.Add("Food");
    }
    protected override void SetupEffects(){
        base.SetupEffects();
        effects["HoldingFood"] = true;
        effects["FoodRemovedFromStore"] = true;
    }
    protected override void SetupConditions(){
        base.SetupConditions();
        preconditions["g_FoodAvailable"] = true;
    }

}