
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class BerryStore : SmartObject
{
    WorldState tribeState;
    [SerializeField]
    GameObject berriesPrefab;
    
    protected override void Start()
    {
        base.Start();
        value = 0f;
        typeName = "BerryStore";
        tribeState = GameObject.Find("TribeState").GetComponent<WorldState>();
    }

    public override void Add(SmartObject obj){
        if (obj.typeName == "Berries"){
            value += obj.value;
            obj.Remove();
        }
        tribeState.UpdateFloatValue("Berries", obj.value);
    }

    public override GameObject Extract(float value){
        GameObject berries = Instantiate(
            berriesPrefab, 
            transform.position, 
            Quaternion.identity
        );
        berries.GetComponent<Berries>().SetValue(value);
        return berries;
    }
}
