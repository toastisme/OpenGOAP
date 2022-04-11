using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class WoodStore : SmartObject
{
    WorldState tribeState;
    [SerializeField]
    GameObject woodPrefab;
    
    protected override void Start()
    {
        base.Start();
        value = 0f;
        typeName = "WoodStore";
        tribeState = GameObject.Find("TribeState").GetComponent<WorldState>();
    }

    public override void Add(SmartObject obj){
        if (obj.typeName == "Wood"){
            value += obj.value;
            obj.Remove();
        }
        tribeState.UpdateFloatValue("Wood", obj.value);
    }

    public override GameObject Extract(float value){
        GameObject wood = Instantiate(
            woodPrefab, 
            transform.position, 
            Quaternion.identity
        );
        wood.GetComponent<Wood>().SetValue(value);
        return wood;
    }
}
