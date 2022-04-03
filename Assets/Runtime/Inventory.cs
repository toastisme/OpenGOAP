using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<string, List<SmartObject> > items;

    public Inventory(){
        items = new Dictionary<string, List<SmartObject>>();
    }

    public void Add(SmartObject obj){
        if (!items.ContainsKey(obj.typeName)){
            items[obj.typeName] = new List<SmartObject>();
        }
        items[obj.typeName].Add(obj);
        obj.PickedUp();
    }

    public void Remove(SmartObject obj){
        items[obj.typeName].Remove(obj);
    }

    public bool Contains(string typeName){
        return (items.ContainsKey(typeName) && items[typeName].Count > 0);
    }
}
