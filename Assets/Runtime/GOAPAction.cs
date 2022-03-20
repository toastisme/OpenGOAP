using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP{
public class GOAPAction : MonoBehaviour, IAction
{
    public WorldState worldState;
    public WorldState requiredState;
    public WorldState outputState;

    public virtual void Setup(ref WorldState worldState){
        this.worldState = worldState;
    }

    public virtual float GetCost(){
        return 0f;
    }

    public virtual bool SatisfiesCondition(string condition){
        bool conditionValue;
        if(outputState.boolKeys.TryGetValue(
            condition, out conditionValue) && conditionValue == true
            ){
                return true;
            }
        return false;
    }

    public virtual bool SatisfiesState(WorldState state){
        return outputState.IsSubset(state);
    }

    public virtual void OnActivated(){
    }

    public virtual void OnDeactivated(){
    }

    public virtual void OnTick(){}

    public virtual bool CanRun(){
        /**
         * true if worldState satisfies preconditions
         */
        return requiredState.IsSubset(worldState);
    }
}
}