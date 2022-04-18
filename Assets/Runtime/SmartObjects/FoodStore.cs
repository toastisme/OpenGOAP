
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class FoodStore : SmartObject
{
    StateSet tribeState;
    [SerializeField]
    GameObject berriesPrefab;
    
    protected override void Start()
    {
        base.Start();
        value = 0f;
        typeName = "FoodStore";
        tribeState = GameObject.Find("TribeState").GetComponent<StateSet>();
        tribeState.UpdateState("g_Food", value);
    }

    public override void Add(SmartObject obj){
        if (obj.typeName == "Food"){
            value += obj.value;
            tribeState.UpdateState("g_Food", obj.value);
            tribeState.AddState("g_FoodAvailable", true);
            obj.Remove();
        }
    }

    public override GameObject Extract(float extractValue){
        GameObject berries = Instantiate(
            berriesPrefab, 
            transform.position, 
            Quaternion.identity
        );
        extractValue = extractValue > value ? value : extractValue;
        berries.GetComponent<Berries>().SetValue(extractValue);
        value -= extractValue;
        tribeState.UpdateState("g_Food", -extractValue);
        if (tribeState.GetFloatState("g_Food") < 1e-7){
            tribeState.AddState("g_FoodAvailable", false);
        }
        return berries;
    }
}
