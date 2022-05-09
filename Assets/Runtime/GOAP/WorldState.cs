using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP{
[RequireComponent(typeof(StateSet))]
public class WorldState : MonoBehaviour
{

    /**
     * \class GOAP.WorldState
     * The state of the world represented as a local StateSet and a global StateSet.
     * The local StateSet is specific to the GameObject, whereas the global StateSet
     * can be shared between GameObjects.
     */

     
    //Attached and specific to the GameObject 
    StateSet localState; 

    /* 
    Shared between objects.
    All states assumed to have g_ prefix
    */
    [SerializeField]
    StateSet globalState; 

    protected void Awake(){
        localState = GetComponent<StateSet>();
    }

    public void SetGlobalState(StateSet globalState){
        this.globalState = globalState;
    }

    public void SetGlobalDefaultFalse(bool val){
        globalState?.SetDefaultFalse(val);
    }

    public void SetLocalDefaultFalse(bool val){
        localState?.SetDefaultFalse(val);
    }

    public bool IsSubset(Dictionary<string, bool> state){
        foreach(var i in state){
            if (!InSet(i.Key, i.Value)){
                return false;
            }
        }
        return true;
    }

    public bool IsSubset(Dictionary<string, float> state){
        foreach(var i in state){
            if (!InSet(i.Key, i.Value)){
                return false;
            }
        }
        return true;
    }

    public bool IsSubset(Dictionary<string, int> state){
        foreach(var i in state){
            if (!InSet(i.Key, i.Value)){
                return false;
            }
        }
        return true;
    }

    public bool IsSubset(Dictionary<string, Vector2> state){
        foreach(var i in state){
            if (!InSet(i.Key, i.Value)){
                return false;
            }
        }
        return true;
    }

    public bool IsSubset(Dictionary<string, Vector3> state){
        foreach(var i in state){
            if (!InSet(i.Key, i.Value)){
                return false;
            }
        }
        return true;
    }

    public bool IsSubset(Dictionary<string, string> state){
        foreach(var i in state){
            if (!InSet(i.Key, i.Value)){
                return false;
            }
        }
        return true;
    }

    public bool IsSubset(Dictionary<string, GameObject> state){
        foreach(var i in state){
            if (!InSet(i.Key, i.Value)){
                return false;
            }
        }
        return true;
    }

    public bool IsSubset(StateSet stateSet){

        if (!IsSubset(stateSet.GetBoolStates())){return false;}
        if (!IsSubset(stateSet.GetFloatStates())){return false;}
        if (!IsSubset(stateSet.GetIntStates())){return false;}
        if (!IsSubset(stateSet.GetVector2States())){return false;}
        if (!IsSubset(stateSet.GetVector3States())){return false;}
        if (!IsSubset(stateSet.GetStringStates())){return false;}
        if (!IsSubset(stateSet.GetGameObjectStates())){return false;}
        return true;
    }

    public void AddState(string name, bool value){
        if (IsGlobalState(name)){
            globalState.AddState(name, value);
        }
        else{
            localState.AddState(name, value);
        }
    }

    public void AddState(string name, float value){
        if (IsGlobalState(name)){
            globalState.AddState(name, value);
        }
        else{
            localState.AddState(name, value);
        }
    }

    public void AddState(string name, int value){
        if (IsGlobalState(name)){
            globalState.AddState(name, value);
        }
        else{
            localState.AddState(name, value);
        }
    }

    public void AddState(string name, Vector2 value){
        if (IsGlobalState(name)){
            globalState.AddState(name, value);
        }
        else{
            localState.AddState(name, value);
        }
    }

    public void AddState(string name, Vector3 value){
        if (IsGlobalState(name)){
            globalState.AddState(name, value);
        }
        else{
            localState.AddState(name, value);
        }
    }

    public void AddState(string name, string value){
        if (IsGlobalState(name)){
            globalState.AddState(name, value);
        }
        else{
            localState.AddState(name, value);
        }
    }

    public void AddState(string name, GameObject value){
        if (IsGlobalState(name)){
            globalState.AddState(name, value);
        }
        else{
            localState.AddState(name, value);
        }
    }

    public void UpdateState(string name, float value){
        if (IsGlobalState(name)){
            globalState.UpdateState(name, value);
        }
        else{
            localState.UpdateState(name, value);
        }
    }

    public void UpdateState(string name, int value){
        if (IsGlobalState(name)){
            globalState.UpdateState(name, value);
        }
        else{
            localState.UpdateState(name, value);
        }
    }

    public void UpdateState(string name, Vector2 value){
        if (IsGlobalState(name)){
            globalState.UpdateState(name, value);
        }
        else{
            localState.UpdateState(name, value);
        }
    }

    public void UpdateState(string name, Vector3 value){
        if (IsGlobalState(name)){
            globalState.UpdateState(name, value);
        }
        else{
            localState.UpdateState(name, value);
        }
    }

    public void RemoveBoolState(string name){
        if (IsGlobalState(name)){
            globalState.RemoveBoolState(name);
        }
        else{
            localState.RemoveBoolState(name);
        }
    }

    public void RemoveFloatState(string name){
        if (IsGlobalState(name)){
            globalState.RemoveFloatState(name);
        }
        else{
            localState.RemoveFloatState(name);
        }
    }

    public void RemoveIntState(string name){
        if (IsGlobalState(name)){
            globalState.RemoveIntState(name);
        }
        else{
            localState.RemoveIntState(name);
        }
    }

    public void RemoveVector2State(string name){
        if (IsGlobalState(name)){
            globalState.RemoveVector2State(name);
        }
        else{
            localState.RemoveVector2State(name);
        }
    }

    public void RemoveVector3State(string name){
        if (IsGlobalState(name)){
            globalState.RemoveVector3State(name);
        }
        else{
            localState.RemoveVector3State(name);
        }
    }

    public void RemoveStringState(string name){
        if (IsGlobalState(name)){
            globalState.RemoveStringState(name);
        }
        else{
            localState.RemoveStringState(name);
        }
    }

    public void RemoveGameObjectState(string name){
        if (IsGlobalState(name)){
            globalState.RemoveGameObjectState(name);
        }
        else{
            localState.RemoveGameObjectState(name);
        }
    }

    public bool GetBoolState(string name){
        if (IsGlobalState(name)){
            return globalState.GetBoolState(name);
        }
        return localState.GetBoolState(name);
    }

    public float GetFloatState(string name){
        if (IsGlobalState(name)){
            return globalState.GetFloatState(name);
        }
        return localState.GetFloatState(name);
    }

    public int GetIntState(string name){
        if (IsGlobalState(name)){
            return globalState.GetIntState(name);
        }
        return localState.GetIntState(name);
    }

    public Vector2 GetVector2State(string name){
        if (IsGlobalState(name)){
            return globalState.GetVector2State(name);
        }
        return localState.GetVector2State(name);
    }

    public Vector3 GetVector3State(string name){
        if (IsGlobalState(name)){
            return globalState.GetVector3State(name);
        }
        return localState.GetVector3State(name);
    }

    public string GetStringState(string name){
        if (IsGlobalState(name)){
            return globalState.GetStringState(name);
        }
        return localState.GetStringState(name);
    }

    public GameObject GetGameObjectState(string name){
        if (IsGlobalState(name)){
            return globalState.GetGameObjectState(name);
        }
        return localState.GetGameObjectState(name);
    }

    public bool InBoolStates(string name){
        if (IsGlobalState(name)){
            return globalState.InBoolStates(name);
        }
        return localState.InBoolStates(name);
    }

    public bool InFloatStates(string name){
        if (IsGlobalState(name)){
            return globalState.InFloatStates(name);
        }
        return localState.InFloatStates(name);
    }

    public bool InIntStates(string name){
        if (IsGlobalState(name)){
            return globalState.InIntStates(name);
        }
        return localState.InIntStates(name);
    }

    public bool InVector2States(string name){
        if (IsGlobalState(name)){
            return globalState.InVector2States(name);
        }
        return localState.InVector2States(name);
    }

    public bool InVector3States(string name){
        if (IsGlobalState(name)){
            return globalState.InVector3States(name);
        }
        return localState.InVector3States(name);
    }

    public bool InStringStates(string name){
        if (IsGlobalState(name)){
            return globalState.InStringStates(name);
        }
        return localState.InStringStates(name);
    }

    public bool InGameObjectStates(string name){
        if (IsGlobalState(name)){
            return globalState.InGameObjectStates(name);
        }
        return localState.InGameObjectStates(name);
    }

    public bool InSet(string name, bool value){
        if (IsGlobalState(name)){
            return globalState.InSet(name, value);
        }
        return localState.InSet(name, value);
    }

    public bool InSet(string name, float value){
        if (IsGlobalState(name)){
            return globalState.InSet(name, value);
        }
        return localState.InSet(name, value);
    }

    public bool InSet(string name, int value){
        if (IsGlobalState(name)){
            return globalState.InSet(name, value);
        }
        return localState.InSet(name, value);
    }

    public bool InSet(string name, Vector2 value){
        if (IsGlobalState(name)){
            return globalState.InSet(name, value);
        }
        return localState.InSet(name, value);
    }

    public bool InSet(string name, Vector3 value){
        if (IsGlobalState(name)){
            return globalState.InSet(name, value);
        }
        return localState.InSet(name, value);
    }

    public bool InSet(string name, string value){
        if (IsGlobalState(name)){
            return globalState.InSet(name, value);
        }
        return localState.InSet(name, value);
    }

    public bool InSet(string name, GameObject value){
        if (IsGlobalState(name)){
            return globalState.InSet(name, value);
        }
        return localState.InSet(name, value);
    }

    public StateSet GetLocalState(){
        return localState;
    }

    public StateSet GetGlobalState(){
        return globalState;
    }

    private bool IsGlobalState(string name){
        return name.Contains("g_");
    }
}
}