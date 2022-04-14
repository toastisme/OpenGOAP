using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GOAP;

public class TakeFoodToStore : GOAPAction
{

    Inventory inventory;
    Movement movement;
    [SerializeField]
    SmartObject foodStore;

    public override float GetCost(){
        return 0.1f * worldState.GetFloatState("Fatigue");
    }
    public override void Setup(){
        base.Setup();
        movement = GetComponent<Movement>();
        inventory = GetComponent<Inventory>();
        preconditions["HoldingFood"] = true;
        worldState.AddState("FoodHarvested", false);
        effects["FoodHarvested"] = true;
        effects["g_FoodAvailable"] = true;
    }

    public override void OnActivated(){
        worldState.AddState("FoodHarvested", false);
        movement.GoTo(foodStore);
    }

    public override void OnDeactivated(){
        worldState.AddState("FoodHarvested", false);
    }

    public override void OnTick()
    {
        if(PreconditionsSatisfied()){
            movement.GoTo(foodStore);
            if (movement.AtTarget()){
                for(int i = inventory.items["Food"].Count -1; i >= 0; i--){
                    foodStore.Add(inventory.items["Food"][i]);
                    inventory.Remove(inventory.items["Food"][i]);
                }
                worldState.AddState("FoodHarvested", true);
            }
        }
    }
}