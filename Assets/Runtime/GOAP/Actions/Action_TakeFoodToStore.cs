using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GOAP;
using Sensors;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Memory))]
public class Action_TakeFoodToStore : GOAPAction
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
        preconditions["HoldingFood"] = true;
        preconditions["FoodRemovedFromStore"] = false;
        worldState.AddState("FoodHarvested", false);
        effects["FoodHarvested"] = true;
        effects["g_FoodAvailable"] = true;
        foodStore = (SmartObject)memory.RememberNearest("FoodStore");
        actionLayers.Add("Food");
    }

    public override void OnActivated(){
        foodStore = (SmartObject)memory.RememberNearest("FoodStore");
        worldState.AddState("FoodHarvested", false);
        if (foodStore == null){
            StopAction();
            return;
        }
        movement.GoTo(foodStore);
    }

    public override void OnDeactivated(){
        worldState.AddState("FoodHarvested", false);
    }

    public override void OnTick()
    {
        movement.GoTo(foodStore);
        if (movement.AtTarget()){
            for(int i = inventory.items["Food"].Count -1; i >= 0; i--){
                foodStore.Add(inventory.items["Food"][i]);
                inventory.Remove(inventory.items["Food"][i]);
            }
            worldState.AddState("FoodHarvested", true);
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