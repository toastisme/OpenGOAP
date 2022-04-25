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

    public void Add(GameObject obj){
        Add(obj.GetComponent<SmartObject>());
    }

    public void Add(SmartObject obj){
        if (obj==null){
            return;
        }
        if (!items.ContainsKey(obj.typeName)){
            items[obj.typeName] = new List<SmartObject>();
        }
        if (items[obj.typeName].Contains(obj)){
            return;
        }
        obj.PickedUp();
        items[obj.typeName].Add(obj);
        awareness.Forget(obj);
        worldState.AddState($"Holding{obj.typeName}", true);
        obj.transform.SetParent(this.transform);
    }

    public void Remove(SmartObject obj){
        items[obj.typeName].Remove(obj);
        if (!Contains(obj.typeName)){
            worldState.AddState($"Holding{obj.typeName}", false);
        }
    }

    public void Remove(string typeName, int numToRemove){
        if (!items.ContainsKey(typeName)){
            Debug.LogWarning($"Trying to remove {typeName} from inventory that does not exist.");
            return;
        }
        if (items[typeName].Count < numToRemove){
            Debug.LogWarning(
                $"Trying to remove {numToRemove} of {typeName} from inventory ({items[typeName].Count} available)"
            );
        }
        numToRemove = numToRemove < items[typeName].Count ? numToRemove : items[typeName].Count;
        for (int i = numToRemove-1; i >= 0; i--){
            items[typeName].RemoveAt(i);
        }
        if (!Contains(typeName)){
            worldState.AddState($"Holding{typeName}", false);
        }
    }

    public bool Contains(string typeName){
        return (items.ContainsKey(typeName) && items[typeName].Count > 0);
    }
}
