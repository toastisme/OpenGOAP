using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GOAP{
public class StateSet : MonoBehaviour
{
    /**
     * GOAP.StateSet 
     * A series of string,value dictionaries to describe a state
     */

    Dictionary<string, bool> boolStates;
    Dictionary<string, float> floatStates;
    Dictionary<string, int> intStates;
    Dictionary<string, Vector2> vector2States;
    Dictionary<string, Vector3> vector3States;
    Dictionary<string, string> stringStates;
    Dictionary<string, GameObject> gameObjectStates;

    [SerializeField]
    bool defaultFalse = true; // Absent key in boolStates treated the same as key = false

    protected void Awake(){
        boolStates = new Dictionary<string, bool>();
        floatStates = new Dictionary<string, float>();
        intStates = new Dictionary<string, int>();
        vector2States = new Dictionary<string, Vector2>();
        vector3States = new Dictionary<string, Vector3>();
        stringStates = new Dictionary<string, string>();
        gameObjectStates = new Dictionary<string, GameObject>();
    }

    public void SetDefaultFalse(bool val){
        defaultFalse = val;
    }

    public bool IsSubset(Dictionary<string, bool> state){
        foreach(var i in state){
            if (!boolStates.ContainsKey(i.Key)){
                return defaultFalse && i.Value==false? true : false;
            }
            if (boolStates[i.Key] != i.Value){
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

    public virtual void UpdateState(string name, float addedValue){
        if (!floatStates.ContainsKey(name)){
            floatStates[name] = 0;
        }
        floatStates[name] += addedValue;
    }

    public virtual void UpdateState(string name, int addedValue){
        if (!intStates.ContainsKey(name)){
            intStates[name] = 0;
        }
        intStates[name] += addedValue;
    }

    public virtual void UpdateState(string name, Vector2 addedValue){
        if (!vector2States.ContainsKey(name)){
            vector2States[name] = Vector2.zero;
        }
        vector2States[name] += addedValue;
    }

    public virtual void UpdateState(string name, Vector3 addedValue){
        if (!vector3States.ContainsKey(name)){
            vector3States[name] = Vector3.zero;
        }
        vector3States[name] += addedValue;
    }

    public virtual void AddState(string name, bool value){
        boolStates[name] = value;
    }

    public virtual void AddState(string name, float value){
        floatStates[name] = value;
    }

    public virtual void AddState(string name, int value){
        intStates[name] = value;
    }

    public virtual void AddState(string name, Vector2 value){
        vector2States[name] = value;
    }

    public virtual void AddState(string name, Vector3 value){
        vector3States[name] = value;
    }

    public virtual void AddState(string name, string value){
        stringStates[name] = value;
    }

    public virtual void AddState(string name, GameObject value){
        gameObjectStates[name] = value;
    }

    public void RemoveBoolState(string name){
        boolStates.Remove(name);
    }

    public void RemoveFloatState(string name){
        floatStates.Remove(name);
    }

    public void RemoveIntState(string name){
        intStates.Remove(name);
    }

    public void RemoveVector2State(string name){
        vector2States.Remove(name);
    }

    public void RemoveVector3State(string name){
        vector3States.Remove(name);
    }

    public void RemoveStringState(string name){
        stringStates.Remove(name);
    }

    public void RemoveGameObjectState(string name){
        gameObjectStates.Remove(name);
    }

    public bool GetBoolState(string name){
        return boolStates[name];
    }

    public float GetFloatState(string name){
        return floatStates[name]; 
    }

    public int GetIntState(string name){
        return intStates[name]; 
    }

    public Vector2 GetVector2State(string name){
        return vector2States[name]; 
    }

    public Vector3 GetVector3State(string name){
        return vector3States[name]; 
    }

    public string GetStringState(string name){
        return stringStates[name]; 
    }

    public GameObject GetGameObjectState(string name){
        return gameObjectStates[name]; 
    }

    public bool InBoolStates(string name){
        return boolStates.ContainsKey(name);
    }

    public bool InFloatStates(string name){
        return floatStates.ContainsKey(name);
    }

    public bool InIntStates(string name){
        return intStates.ContainsKey(name);
    }

    public bool InVector2States(string name){
        return vector2States.ContainsKey(name);
    }

    public bool InVector3States(string name){
        return vector3States.ContainsKey(name);
    }

    public bool InStringStates(string name){
        return stringStates.ContainsKey(name);
    }

    public bool InGameObjectStates(string name){
        return gameObjectStates.ContainsKey(name);
    }

    public bool InSet(string name, bool value){
        if (!InBoolStates(name)){
            return defaultFalse && value==false ? true : false;
        }
        if (boolStates[name] != value){
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

    public bool InSet(string name, int value){
        if (!InIntStates(name)){
            return false;
        }
        if (Mathf.Abs(intStates[name] - value) > 0){
            return false;
        }
        return true;
    }

    public bool InSet(string name, Vector2 value, float tolerance=1e-7f){
        if (!InVector2States(name)){
            return false;
        }
        if ((vector2States[name] - value).sqrMagnitude > tolerance){
            return false;
        }
        return true;
    }

    public bool InSet(string name, Vector3 value, float tolerance=1e-7f){
        if (!InVector3States(name)){
            return false;
        }
        if ((vector3States[name] - value).sqrMagnitude > tolerance){
            return false;
        }
        return true;
    }

    public bool InSet(string name, string value, float tolerance=1e-7f){
        if (!InStringStates(name)){
            return false;
        }
        return stringStates[name] == value;
    }

    public bool InSet(string name, GameObject value, float tolerance=1e-7f){
        if (!InGameObjectStates(name)){
            return false;
        }
        return gameObjectStates[name] == value;
    }

    public Dictionary<string, bool> GetBoolStates(){
        return boolStates;
    }

    public Dictionary<string, float> GetFloatStates(){
        return floatStates;
    }

    public Dictionary<string, int> GetIntStates(){
        return intStates;
    }

    public Dictionary<string, Vector2> GetVector2States(){
        return vector2States;
    }

    public Dictionary<string, Vector3> GetVector3States(){
        return vector3States;
    }

    public Dictionary<string, string> GetStringStates(){
        return stringStates;
    }

    public Dictionary<string, GameObject> GetGameObjectStates(){
        return gameObjectStates;
    }
}
}