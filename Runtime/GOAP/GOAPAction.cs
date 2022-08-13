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
    private Dictionary<string, List<string> > temporaryState; 

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
        stopAction_ = false;
        actionLayers = new List<string>();
        effects = new Dictionary<string, bool>();
        preconditions = new Dictionary<string, bool>();
        SetupConditions();
        SetupEffects();
        SetupActionLayers();
        SetupDerived();
        ResetTemporaryState();
    }

    void ResetTemporaryState(){
        temporaryState = new Dictionary<string, List<string> >();
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
        if (!temporaryState.ContainsKey("bool")){
            temporaryState["bool"] = new List<string>();
        }
        temporaryState["bool"].Add(name);
    }
    protected void AddTemporaryState(string name, float val){
        AddState(name, val);
        if (!temporaryState.ContainsKey("float")){
            temporaryState["float"] = new List<string>();
        }
        temporaryState["float"].Add(name);
    }
    protected void AddTemporaryState(string name, int val){
        AddState(name, val);
        if (!temporaryState.ContainsKey("int")){
            temporaryState["int"] = new List<string>();
        }
        temporaryState["int"].Add(name);
    }
    protected void AddTemporaryState(string name, Vector2 val){
        AddState(name, val);
        if (!temporaryState.ContainsKey("Vector2")){
            temporaryState["Vector2"] = new List<string>();
        }
        temporaryState["Vector2"].Add(name);
    }
    protected void AddTemporaryState(string name, Vector3 val){
        AddState(name, val);
        if (!temporaryState.ContainsKey("Vector3")){
            temporaryState["Vector3"] = new List<string>();
        }
        temporaryState["Vector3"].Add(name);
    }
    protected void AddTemporaryState(string name, string val){
        AddState(name, val);
        if (!temporaryState.ContainsKey("string")){
            temporaryState["string"] = new List<string>();
        }
        temporaryState["string"].Add(name);
    }
    protected void AddTemporaryState(string name, GameObject val){
        AddState(name, val);
        if (!temporaryState.ContainsKey("GameObject")){
            temporaryState["GameObject"] = new List<string>();
        }
        temporaryState["GameObject"].Add(name);
    }

    protected void ClearTemporaryStates(){
        foreach(var i in temporaryState){
            switch(i.Key){
                case("bool"):
                    for (int j = 0; j < i.Value.Count; j++){
                        worldState.RemoveBoolState(i.Value[j]);
                    }
                    break;
                case("float"):
                    for (int j = 0; j < i.Value.Count; j++){
                        worldState.RemoveFloatState(i.Value[j]);
                    }
                    break;
                case("int"):
                    for (int j = 0; j < i.Value.Count; j++){
                        worldState.RemoveIntState(i.Value[j]);
                    }
                    break;
                case("Vector2"):
                    for (int j = 0; j < i.Value.Count; j++){
                        worldState.RemoveVector2State(i.Value[j]);
                    }
                    break;
                case("Vector3"):
                    for (int j = 0; j < i.Value.Count; j++){
                        worldState.RemoveVector3State(i.Value[j]);
                    }
                    break;
                case("string"):
                    for (int j = 0; j < i.Value.Count; j++){
                        worldState.RemoveStringState(i.Value[j]);
                    }
                    break;
                case("GameObject"):
                    for (int j = 0; j < i.Value.Count; j++){
                        worldState.RemoveGameObjectState(i.Value[j]);
                    }
                    break;
            }
        }
        ResetTemporaryState();
    }

}
}
