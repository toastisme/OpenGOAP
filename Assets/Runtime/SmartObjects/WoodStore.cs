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
        tribeState.UpdateState("g_Wood", value);
    }

    public override void Add(SmartObject obj){
        if (obj.typeName == "Wood"){
            value += obj.value;
            tribeState.UpdateState("g_Wood", obj.value);
            tribeState.AddState("g_WoodAvailable", true);
            obj.Remove();
        }
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
        if (tribeState.GetFloatState("g_Wood") < 1e-7){
            tribeState.AddState("g_WoodAvailable", false);
        }
        return wood;
    }
}
