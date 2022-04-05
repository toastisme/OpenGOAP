using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using Sensors;

[RequireComponent(typeof(WorldState))]
public class Inventory : MonoBehaviour
{
    WorldState worldState;
    Awareness awareness;
    public Dictionary<string, List<SmartObject> > items;

    void Start(){
        items = new Dictionary<string, List<SmartObject>>();
        worldState = GetComponent<WorldState>();
        awareness = GetComponent<Awareness>();
    }

    public void Add(SmartObject obj){
        if (!items.ContainsKey(obj.typeName)){
            items[obj.typeName] = new List<SmartObject>();
        }
        obj.PickedUp();
        items[obj.typeName].Add(obj);
        awareness.Forget(obj);
        worldState.states[$"Holding{obj.typeName}"] = true;
    }

    public void Remove(SmartObject obj){
        items[obj.typeName].Remove(obj);
        if (!Contains(obj.typeName)){
            worldState.states[$"Holding{obj.typeName}"] = false;
        }
    }

    public bool Contains(string typeName){
        return (items.ContainsKey(typeName) && items[typeName].Count > 0);
    }
}
