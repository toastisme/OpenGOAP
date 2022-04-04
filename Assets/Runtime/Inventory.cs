using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

[RequireComponent(typeof(WorldState))]
public class Inventory : MonoBehaviour
{
    WorldState worldState;
    public Dictionary<string, List<SmartObject> > items;

    public Inventory(){
        items = new Dictionary<string, List<SmartObject>>();
        worldState = GetComponent<WorldState>();
    }

    public void Add(SmartObject obj){
        if (!items.ContainsKey(obj.typeName)){
            items[obj.typeName] = new List<SmartObject>();
        }
        obj.PickedUp();
        items[obj.typeName].Add(obj);
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
