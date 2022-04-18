using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using Sensors;

[RequireComponent(typeof(Memory))]
public class Goal_HarvestWood : Goal
{
    Memory memory;

    public override void Setup(){
        base.Setup();
        memory = GetComponent<Memory>();
        conditions["WoodHarvested"] = true;
    }

    public override float GetPriority()
    {
        return .2f;
    }

    public override bool PreconditionsSatisfied(){
        return memory.InMemory("WoodStore");
    }
}