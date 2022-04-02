using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP{
public class TakeWoodToStore : GOAPAction
{

    Movement movement;
    [SerializeField]
    GameObject woodStore;

    public override float GetCost(){
        return 0.0f;
    }
    public override void Setup(ref WorldState worldState){
        movement = GetComponent<Movement>();
        this.worldState = worldState;
        requiredState = new WorldState();
        requiredState.boolKeys["HoldingWood"] = true;

        outputState = new WorldState();
        outputState.boolKeys["WoodHarvested"] = true;
    }

    public override void OnActivated(){
        movement.GoTo(woodStore.transform.position);
    }

    public override void OnTick()
    {
        while(CanRun()){
            if (movement.AtTarget()){
                worldState.boolKeys["WoodHarvested"] = true;
            }
            break;
        }
    }

    

}
}