using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GOAP{
public class StateSet : MonoBehaviour
{
    Dictionary<string, bool> states;
    Dictionary<string, float> floatStates;

    [SerializeField]
    bool defaultFalse = true; // Absent key treated the same as key = false

    protected void Awake(){
        states = new Dictionary<string, bool>();
        floatStates = new Dictionary<string, float>();
    }

    public void SetDefaultFalse(bool val){
        defaultFalse = val;
    }

    public bool IsSubset(Dictionary<string, bool> state){
        foreach(var i in state){
            if (!states.ContainsKey(i.Key)){
                return defaultFalse && i.Value==false? true : false;
            }
            if (states[i.Key] != i.Value){
                return false;
            }
        }
        return true;
    }

    public virtual void UpdateState(string name, float addedValue){
        if (!floatStates.ContainsKey(name)){
            floatStates[name] = 0;
        }
        floatStates[name] += addedValue;
    }

    public virtual void AddState(string name, float value){
        floatStates[name] = value;
    }

    public virtual void AddState(string name, bool value){
        states[name] = value;
    }

    public void RemoveBoolState(string name){
        states.Remove(name);
    }

    public void RemoveFloatState(string name){
        floatStates.Remove(name);
    }

    public float GetFloatState(string name){
        return floatStates[name]; 
    }

    public bool GetBoolState(string name){
        return states[name];
    }

    public bool InBoolStates(string name){
        return states.ContainsKey(name);
    }

    public bool InFloatStates(string name){
        return floatStates.ContainsKey(name);
    }

    public bool InSet(string name, bool value){
        if (!InBoolStates(name)){
            return defaultFalse && value==false ? true : false;
        }
        if (states[name] != value){
            return false;
        }
        return true;
    }

    public bool InSet(string name, float value, float tolerance=1e-7f){
        if (!InFloatStates(name)){
            return false;
        }
        if (Mathf.Abs(floatStates[name] - value) > tolerance){
            return false;
        }
        return true;
    }

    public Dictionary<string, bool> GetBoolStates(){
        return states;
    }

    public Dictionary<string, float> GetFloatStates(){
        return floatStates;
    }
}
}