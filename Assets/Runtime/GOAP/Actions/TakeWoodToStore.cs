using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP{
public class TakeWoodToStore : GOAPAction
{

    Movement movement;
    [SerializeField]
    SmartObject woodStore;
    bool _woodHarvested;

    public override float GetCost(){
        return 0.0f;
    }
    public override void Setup(ref WorldState worldState, ref Inventory inventory){
        base.Setup(ref worldState, ref inventory);
        movement = GetComponent<Movement>();

        worldState.boolKeys2["HoldingWood"] = inventory.Contains("Wood");
        preconditions["HoldingWood"] = true;

        _woodHarvested = false;
        worldState.boolKeys["WoodHarvested"] = WoodHarvested();
        effects["WoodHarvested"] = true;
    }

    public override void OnActivated(){
        _woodHarvested = false;
        movement.GoTo(woodStore.transform.position);
    }

    public override void OnDeactivated(){
        _woodHarvested = false;
    }

    public override void OnTick()
    {
        while(CanRun()){
            if (movement.AtTarget()){
                for(int i = inventory.items["Wood"].Count -1; i >= 0; i--){
                    woodStore.Add(inventory.items["Wood"][i]);
                    inventory.Remove(inventory.items["Wood"][i]);
                }
                _woodHarvested = true;
                break;
            }
        }
    }

    public bool WoodHarvested(){return _woodHarvested;}

    

}
}