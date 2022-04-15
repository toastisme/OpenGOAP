using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GOAP;

public class GetFoodFromStore : GOAPAction
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
        preconditions["g_FoodAvailable"] = true;
        effects["HoldingFood"] = true;
    }

    public override void OnActivated(){
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


    

}