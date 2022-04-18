using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using Sensors;

[RequireComponent(typeof(Memory))]
public class Goal_HarvestFood : Goal
{
    Memory memory;
    public override void Setup(){
        base.Setup();
        memory = GetComponent<Memory>();
        conditions["FoodHarvested"] = true;
    }

    public override float GetPriority()
    {
        return .3f;
    }

    public override bool PreconditionsSatisfied(){
        return memory.InMemory("FoodStore");
    }
}