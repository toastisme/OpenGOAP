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
    // All of these states are removed from worldState when OnDeactivate is called
    private WorldState temporaryState; 
    bool usedTemporaryState=false;

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

    
    void Awake(){
        this.worldState = GetComponent<WorldState>();
        this.temporaryState = this.gameObject.AddComponent<WorldState>();
        stopAction_ = false;
        actionLayers = new List<string>();
        effects = new Dictionary<string, bool>();
        preconditions = new Dictionary<string, bool>();
        SetupConditions();
        SetupEffects();
        SetupActionLayers();
        SetupDerived();
    }

    protected virtual void SetupDerived(){}

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

    public void OnActivate(){

        /**
         * Called when selected by GOAPPlanner
         */

        stopAction_ = false;
        OnActivateDerived();
    }

    protected virtual void OnActivateDerived(){}

    public void OnDeactivate(){

        /**
         * Called by GOAPPlanner when action effects achieved or plan cancelled
         */

        StopAction();
        OnDeactivateDerived();
        ClearTemporaryStates();
    }

    protected virtual void OnDeactivateDerived(){}

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
    }
    protected virtual void SetupEffects(){
    }
    protected virtual void SetupConditions(){
    }

    protected void AddState(string name, bool val){
        worldState.AddState(name, val);
    }

    protected void AddState(string name, float val){
        worldState.AddState(name, val);
    }

    protected void AddState(string name, int val){
        worldState.AddState(name, val);
    }

    protected void AddState(string name, Vector2 val){
        worldState.AddState(name, val);
    }

    protected void AddState(string name, Vector3 val){
        worldState.AddState(name, val);
    }

    protected void AddState(string name, string val){
        worldState.AddState(name, val);
    }

    protected void AddState(string name, GameObject val){
        worldState.AddState(name, val);
    }

    protected void AddTemporaryState(string name, bool val){
        AddState(name, val);
        temporaryState.AddState(name, val);
        usedTemporaryState=true;
    }
    protected void AddTemporaryState(string name, float val){
        AddState(name, val);
        temporaryState.AddState(name, val);
        usedTemporaryState=true;
    }
    protected void AddTemporaryState(string name, int val){
        AddState(name, val);
        temporaryState.AddState(name, val);
        usedTemporaryState=true;
    }
    protected void AddTemporaryState(string name, Vector2 val){
        AddState(name, val);
        temporaryState.AddState(name, val);
        usedTemporaryState=true;
    }
    protected void AddTemporaryState(string name, Vector3 val){
        AddState(name, val);
        temporaryState.AddState(name, val);
        usedTemporaryState=true;
    }
    protected void AddTemporaryState(string name, string val){
        AddState(name, val);
        temporaryState.AddState(name, val);
        usedTemporaryState=true;
    }
    protected void AddTemporaryState(string name, GameObject val){
        AddState(name, val);
        temporaryState.AddState(name, val);
        usedTemporaryState=true;
    }

    protected void ClearTemporaryStates(){
        if (!usedTemporaryState){return;}
        StateSet localState = temporaryState.GetLocalState();
        StateSet globalState = temporaryState.GetGlobalState();
        
        foreach(var i in localState.GetBoolStates().Keys){
            worldState.RemoveBoolState(i);
        }
        foreach(var i in globalState.GetBoolStates().Keys){
            worldState.RemoveBoolState(i);
        }
        foreach(var i in localState.GetFloatStates().Keys){
            worldState.RemoveFloatState(i);
        }
        foreach(var i in globalState.GetFloatStates().Keys){
            worldState.RemoveFloatState(i);
        }
        foreach(var i in localState.GetIntStates().Keys){
            worldState.RemoveIntState(i);
        }
        foreach(var i in globalState.GetIntStates().Keys){
            worldState.RemoveIntState(i);
        }
        foreach(var i in localState.GetVector2States().Keys){
            worldState.RemoveVector2State(i);
        }
        foreach(var i in globalState.GetVector2States().Keys){
            worldState.RemoveVector2State(i);
        }
        foreach(var i in localState.GetVector3States().Keys){
            worldState.RemoveVector3State(i);
        }
        foreach(var i in globalState.GetVector3States().Keys){
            worldState.RemoveVector3State(i);
        }
        foreach(var i in localState.GetStringStates().Keys){
            worldState.RemoveStringState(i);
        }
        foreach(var i in globalState.GetStringStates().Keys){
            worldState.RemoveStringState(i);
        }
        foreach(var i in localState.GetGameObjectStates().Keys){
            worldState.RemoveGameObjectState(i);
        }
        foreach(var i in globalState.GetGameObjectStates().Keys){
            worldState.RemoveGameObjectState(i);
        }
        usedTemporaryState = false;
    }

}
}