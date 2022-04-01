using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP{
public class GOAPAction : MonoBehaviour, IAction
{
    public WorldState worldState;
    public WorldState requiredState;
    public WorldState outputState;

    bool stopAction_;
    
    public virtual void Setup(ref WorldState worldState){
        this.worldState = worldState;
        stopAction_ = false;
    }

    public virtual float GetCost(){
        return 0f;
    }

    public virtual bool SatisfiesCondition(string condition){
        if (condition == ""){return true;}
        bool conditionValue;
        if(outputState.boolKeys.TryGetValue(
            condition, out conditionValue) && conditionValue == true
            ){
                return true;
            }
        return false;
    }

    public virtual bool SatisfiesState(WorldState state){
        return state.IsSubset(outputState);
    }

    public virtual void OnActivated(){
        stopAction_ = false;
    }

    public virtual void OnDeactivated(){
        stopAction_ = true;
    }

    public virtual void OnTick(){}

    public void StopAction(){
        stopAction_=true;
    }

    public virtual bool CanRun(){
        /**
         * true if worldState satisfies preconditions
         */
        return (!(stopAction_) && worldState.IsSubset(requiredState));
    }
}
}