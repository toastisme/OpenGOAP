using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP{
public class ActionIdle : GOAPAction
{
    bool _wasIdle;
    public override float GetCost(){
        return 0.0f;
    }

    public override void Setup(ref WorldState worldState, ref Inventory inventory){
        base.Setup(ref worldState, ref inventory);
        _wasIdle = false;
        worldState.boolKeys["WasIdle"] = WasIdle();
        effects["WasIdle"] = true;
    }
    public override void OnActivated(){
        _wasIdle = false;
    }

    public override void OnDeactivated(){
        _wasIdle = false;
    }

    public override void OnTick(){
        _wasIdle = true;
    }

    public bool WasIdle(){return _wasIdle;}

}
}