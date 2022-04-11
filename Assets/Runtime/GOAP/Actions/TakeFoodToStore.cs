
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
        return 0.0f;
    }
    public override void Setup(){
        base.Setup();
        movement = GetComponent<Movement>();
        inventory = GetComponent<Inventory>();
        preconditions["HoldingFood"] = true;
        personalState.states["FoodHarvested"] = false;
        effects["FoodHarvested"] = true;
    }

    public override void OnActivated(){
        personalState.states["FoodHarvested"] = false;
        movement.GoTo(foodStore);
    }

    public override void OnDeactivated(){
        personalState.states["FoodHarvested"] = false;
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
                personalState.states["FoodHarvested"] = true;
            }
        }
    }


    

}