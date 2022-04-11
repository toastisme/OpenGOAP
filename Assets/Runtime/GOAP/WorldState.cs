using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GOAP{
public class WorldState : MonoBehaviour
{
    public Dictionary<string, bool> states;
    public Dictionary<string, float> floatStates;

    void Awake(){
        states = new Dictionary<string, bool>();
        floatStates = new Dictionary<string, float>();
    }

    public bool IsSubset(Dictionary<string, bool> state){
        foreach(var i in state){
            if (!states.ContainsKey(i.Key)){
                return false;
            }
            if (states[i.Key] != i.Value){
                return false;
            }
        }
        return true;
    }

    public void UpdateFloatValue(string name, float addedValue){
        if (!floatStates.ContainsKey(name)){
            floatStates[name] = 0;
        }
        floatStates[name] += addedValue;
    }

    public void ReplaceFloatValue(string name, float value){
        floatStates[name] = value;
    }
}
}