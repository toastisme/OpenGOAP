using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP{
public class GOAPAction : MonoBehaviour, IAction
{
    protected WorldState worldState;
    protected Inventory inventory;
    public Dictionary<string, bool> preconditions{get; protected set;} // worldState.boolKeys that must be true to start
    public Dictionary<string, bool> effects{get; protected set;} // worldState.boolKeys that are true on completion
    bool stopAction_;
    
    public virtual void Setup(
        ref WorldState worldState,
        ref Inventory inventory
        ){
        this.worldState = worldState;
        this.inventory = inventory;
        stopAction_ = false;
        preconditions = new Dictionary<string, bool>();
        effects = new Dictionary<string, bool>();
    }

    public virtual float GetCost(){
        return 0f;
    }

    public virtual bool SatisfiesConditions(
        Dictionary<string, bool> conditions
        ){

        foreach(var i in conditions){
            if (!effects.ContainsKey(i.Key)){
                return false;
            }
            if (effects[i.Key] != i.Value){
                return false;
            }
        }

        return true;
    }

    public virtual void OnActivated(){
        stopAction_ = false;
    }

    public virtual void OnDeactivated(){
        StopAction();
    }

    public virtual void OnTick(){}

    public void StopAction(){
        stopAction_=true;
    }

    public virtual bool CanRun(){
        /**
         * true if worldState satisfies preconditions
         */
        if (stopAction_){ return false; }
        foreach(var i in preconditions){
            if (!worldState.boolKeys.ContainsKey(i.Key)){
                return false;
            }
            if (worldState.boolKeys[i.Key]() != i.Value){
                return false;
            }
        }
        return true;
    }

    public virtual bool EffectsSatisfied(){
        /**
         * true if worldState satisfies effects
         */
        foreach(var i in effects){
            if (!worldState.boolKeys.ContainsKey(i.Key)){
                return false;
            }
            if (worldState.boolKeys[i.Key]() != i.Value){
                return false;
            }
        }
        return true;
    }
}
}