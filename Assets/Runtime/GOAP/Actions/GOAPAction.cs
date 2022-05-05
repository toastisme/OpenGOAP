using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP{
public class GOAPAction : MonoBehaviour, IAction
{

    /**
     * \class GOAP.GOAPAction
     * A behaviour that requires preconditions to run and has known 
     * effects upon completion.
     */

    protected WorldState worldState;

    // What must be in worldState for the action to run
    public Dictionary<string, bool> preconditions{get; protected set;} 
    // What will be in worldState when action completed
    public Dictionary<string, bool> effects{get; protected set;} 

    /*
    * GOAPPlanner will only consider this action for GOAPGoals 
    * with one of these layers as GOAPGoal.actionLayer
    */
    public List<string> actionLayers; 

    // Used to prematurely stop action continuing
    bool stopAction_;

    // Absent key treated the same as key = false in preconditions and effects
    [SerializeField]
    protected bool defaultFalse = true; 

    
    public virtual void Setup(){

        /**
         * Called by GOAPPlanner when entering Play Mode.
         */

        this.worldState = GetComponent<WorldState>();
        stopAction_ = false;
        SetupConditions();
        SetupEffects();
        SetupActionLayers();
    }

    public virtual float GetCost(){

        /**
         * Assumed to be between 0f and 1f
         */ 

        return 0f;
    }

    public virtual bool SatisfiesConditions(
        Dictionary<string, bool> conditions
        ){

        /**
         * Returns true if effects are a superset for conditions
         */

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

        /**
         * Called when selected by GOAPPlanner
         */

        stopAction_ = false;
    }

    public virtual void OnDeactivate(){

        /**
         * Called by GOAPPlanner when action effects achieved or plan cancelled
         */

        StopAction();
    }

    public virtual void OnTick(){

        /**
         * Called every frame by GOAPPlanner
         */

    }

    public void StopAction(){
        stopAction_=true;
    }

    public virtual bool PreconditionsSatisfied(WorldState worldState){

        /**
         * true if worldState is a superset of preconditions
         */

        if (stopAction_){ return false; }
        return(worldState.IsSubset(preconditions));
    }

    public virtual bool EffectsSatisfied(WorldState worldState){

        /**
         * true if worldState is a superset of effects
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