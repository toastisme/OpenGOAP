using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GOAP{
public class WorldState : MonoBehaviour
{
    public Dictionary<string, bool> states;

    void Awake(){
        states = new Dictionary<string, bool>();
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
}
}