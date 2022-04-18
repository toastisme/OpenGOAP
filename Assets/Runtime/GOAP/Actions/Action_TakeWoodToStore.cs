using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GOAP;

public class Action_TakeWoodToStore : GOAPAction
{

    Inventory inventory;
    Movement movement;
    [SerializeField]
    SmartObject woodStore;

    public override float GetCost(){
        return 0.1f * worldState.GetFloatState("Fatigue");
    }
    public override void Setup(){
        base.Setup();
        movement = GetComponent<Movement>();
        inventory = GetComponent<Inventory>();
        preconditions["HoldingWood"] = true;
        worldState.AddState("WoodHarvested", false);
        effects["WoodHarvested"] = true;
        effects["g_WoodAvailable"] = true;
    }

    public override void OnActivated(){
        worldState.AddState("WoodHarvested", false);
        movement.GoTo(woodStore);
    }

    public override void OnDeactivated(){
        worldState.AddState("WoodHarvested", false);
    }

    public override void OnTick()
    {
        if(PreconditionsSatisfied()){
            movement.GoTo(woodStore);
            if (movement.AtTarget()){
                for(int i = inventory.items["Wood"].Count -1; i >= 0; i--){
                    woodStore.Add(inventory.items["Wood"][i]);
                    inventory.Remove(inventory.items["Wood"][i]);
                }
                worldState.AddState("WoodHarvested", true);
            }
        }
    }


    

}