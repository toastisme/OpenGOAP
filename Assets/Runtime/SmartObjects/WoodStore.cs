using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class WoodStore : SmartObject
{
    StateSet tribeState;
    [SerializeField]
    GameObject woodPrefab;
    
    protected override void Start()
    {
        base.Start();
        value = 0f;
        typeName = "WoodStore";
        tribeState = GameObject.Find("TribeState").GetComponent<StateSet>();
    }

    public override void Add(SmartObject obj){
        if (obj.typeName == "Wood"){
            value += obj.value;
            obj.Remove();
        }
        tribeState.UpdateState("g_Wood", obj.value);
    }

    public override GameObject Extract(float extractValue){
        GameObject wood = Instantiate(
            woodPrefab, 
            transform.position, 
            Quaternion.identity
        );
        extractValue = extractValue > value ? value : extractValue;
        wood.GetComponent<Wood>().SetValue(extractValue);
        value -= extractValue;
        tribeState.UpdateState("g_Wood", -extractValue);
        return wood;
    }
}
