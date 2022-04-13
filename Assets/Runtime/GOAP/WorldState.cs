using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP{
[RequireComponent(typeof(StateSet))]
public class WorldState : MonoBehaviour
{
    /* 
    Attached and specific to the GameObject 
    */
    StateSet localState; 

    /* 
    Shared between objects.
    All states assumed to have g_ prefix
    */
    StateSet globalState; 

    protected void Awake(){
        localState = GetComponent<StateSet>();
    }

    public void SetGlobalState(StateSet globalState){
        this.globalState = globalState;
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

    public bool IsSubset(StateSet stateSet){
        foreach(var i in stateSet.GetBoolStates()){
            if (!InSet(i.Key, i.Value)){
                return false;
            }
        }
        foreach(var i in stateSet.GetFloatStates()){
            if (!InSet(i.Key, i.Value)){
                return false;
            }
        }
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

    public void UpdateState(string name, float value){
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

    private bool InSet(string name, bool value){
        if (IsGlobalState(name)){
            if (globalState == null){
                return false;
            }
            return globalState.InSet(name, value);
        }
        return localState.InSet(name, value);
    }

    private bool InSet(string name, float value){
        if (IsGlobalState(name)){
            if (globalState == null){
                return false;
            }
            return globalState.InSet(name, value);
        }
        return localState.InSet(name, value);
    }

    private bool IsGlobalState(string name){
        return name.Contains("g_");
    }
}
}