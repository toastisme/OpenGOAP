using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP{

public class GOAPAction : MonoBehaviour, IAction
{
    protected WorldState worldState;
    public Dictionary<string, bool> preconditions{get; protected set;} // worldState.states that must be true to start
    public Dictionary<string, bool> effects{get; protected set;} // worldState.states that are true on completion

    /*
    * When searching for action plans, only goals with these layers will include this
    * as a potential action
    */
    public List<string> actionLayers; 
    bool stopAction_;

    [SerializeField]
    protected bool defaultFalse = true; // Absent key treated the same as key = false

    
    public virtual void Setup(){
        this.worldState = GetComponent<WorldState>();
        stopAction_ = false;
        SetupConditions();
        SetupEffects();
        SetupActionLayers();
    }

    public virtual float GetCost(){
        return 0f;
    }

    public virtual bool SatisfiesConditions(
        Dictionary<string, bool> conditions
        ){

        foreach(var i in conditions){
            if (!effects.ContainsKey(i.Key)){
                return defaultFalse && i.Value==false? true : false;
            }
            if (effects[i.Key] != i.Value){
                return false;
            }
        }

        return true;
    }

    public virtual void OnActivate(){
        stopAction_ = false;
    }

    public virtual void OnDeactivate(){
        StopAction();
    }

    public virtual void OnTick(){}

    public void StopAction(){
        stopAction_=true;
    }

    public virtual bool PreconditionsSatisfied(WorldState worldState){
        /**
         * true if worldState satisfies preconditions
         */
        if (stopAction_){ return false; }
        return(worldState.IsSubset(preconditions));
    }

    public virtual bool EffectsSatisfied(WorldState worldState){
        /**
         * true if worldState satisfies effects
         */
        return (worldState.IsSubset(effects));
    }

    protected virtual void SetupActionLayers(){
        actionLayers = new List<string>();
    }
    protected virtual void SetupEffects(){
        effects = new Dictionary<string, bool>();
    }
    protected virtual void SetupConditions(){
        preconditions = new Dictionary<string, bool>();
    }
}
}