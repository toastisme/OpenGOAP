using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class VillagerProperties : MonoBehaviour
{
    WorldState personalState;

    // Initial states and decay rates
    Dictionary<string, KeyValuePair<float, float>> properties = 
        new Dictionary<string, KeyValuePair<float, float>>{
        {"health", new KeyValuePair<float, float>(1f, 0f)},
        {"fatigue", new KeyValuePair<float, float>(0f, .01f)},
        {"hunger", new KeyValuePair<float, float>(0f, .02f)},
        {"thirst", new KeyValuePair<float, float>(0f, 0.3f)},
        {"warmth", new KeyValuePair<float, float>(1f, 0f)}
    };


    void Start(){
        personalState = GetComponent<WorldState>();
        SetInitialStates();
    }

    void SetInitialStates(){
        foreach (var i in properties){
            personalState.AddFloatValue(i.Key, i.Value.Key);
            personalState.AddFloatValue(GetDeltaKey(i.Key), i.Value.Value);
        }
    }

    string GetDeltaKey(string name){
        return "Delta" + name;
    }

    public void AddProperty(string name, float value, float delta){
        properties[name] = new KeyValuePair<float, float>(value, delta);
        personalState.AddFloatValue(name, value);
        personalState.AddFloatValue(GetDeltaKey(name), delta);
    }

    public void RemoveProperty(string name){
        personalState.floatStates.Remove(name);
        personalState.floatStates.Remove(GetDeltaKey(name));
        properties.Remove(name);
    }

    void UpdateProperties(){
        foreach(var i in properties){
            personalState.floatStates[i.Key] += 
                personalState.floatStates[GetDeltaKey(i.Key)] * Time.deltaTime;
        }
    }



    
}
