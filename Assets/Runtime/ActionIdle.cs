using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP{
public class ActionIdle : GOAPAction
{
    public override float GetCost(){
        return 0.0f;
    }
    public override void Setup(ref WorldState worldState){
        this.worldState = worldState;
        requiredState = new WorldState();
        outputState = new WorldState();
        outputState.boolKeys["HasBeenIdle"] = true;
    }

}
}