using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class VillagerProperties : MonoBehaviour
{
    WorldState worldState;

    // Initial states and decay rates
    Dictionary<string, KeyValuePair<float, float>> properties = 
        new Dictionary<string, KeyValuePair<float, float>>{
        {"Health", new KeyValuePair<float, float>(1f, 0f)},
        {"Fatigue", new KeyValuePair<float, float>(0f, .01f)},
        {"Hunger", new KeyValuePair<float, float>(0f, .02f)},
        {"Thirst", new KeyValuePair<float, float>(0f, 0.3f)},
        {"Warmth", new KeyValuePair<float, float>(1f, 0f)}
    };


    void Awake(){
        worldState = GetComponent<WorldState>();
        SetInitialStates();
    }

    void Update(){
        UpdateProperties();
    }

    void SetInitialStates(){
        foreach (var i in properties){
            worldState.AddState(i.Key, i.Value.Key);
            worldState.AddState(GetDeltaKey(i.Key), i.Value.Value);
        }
    }

    string GetDeltaKey(string name){
        return "Delta" + name;
    }

    public void AddProperty(string name, float value, float delta){
        properties[name] = new KeyValuePair<float, float>(value, delta);
        worldState.AddState(name, value);
        worldState.AddState(GetDeltaKey(name), delta);
    }

    public void RemoveProperty(string name){
        worldState.RemoveFloatState(name);
        worldState.RemoveFloatState(GetDeltaKey(name));
        properties.Remove(name);
    }

    void UpdateProperties(){
        foreach(var i in properties){
            worldState.UpdateState(
                i.Key, 
                worldState.GetFloatState(GetDeltaKey(i.Key)) * Time.deltaTime
                );
        }
    }
}
