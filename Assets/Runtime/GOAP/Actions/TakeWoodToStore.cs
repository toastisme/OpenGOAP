using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP{
public class TakeWoodToStore : GOAPAction
{

    Inventory inventory;
    Movement movement;
    [SerializeField]
    SmartObject woodStore;

    public override float GetCost(){
        return 0.0f;
    }
    public override void Setup(){
        base.Setup();
        movement = GetComponent<Movement>();
        inventory = GetComponent<Inventory>();
        preconditions["HoldingWood"] = true;
        worldState.states["WoodHarvested"] = false;
        effects["WoodHarvested"] = true;
    }

    public override void OnActivated(){
        worldState.states["WoodHarvested"] = false;
        movement.GoTo(woodStore);
    }

    public override void OnDeactivated(){
        worldState.states["WoodHarvested"] = false;
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
                worldState.states["WoodHarvested"] = true;
            }
        }
    }


    

}
}