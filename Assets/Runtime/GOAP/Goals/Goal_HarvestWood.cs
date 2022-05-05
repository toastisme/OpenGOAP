using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using Sensors;

[RequireComponent(typeof(Memory))]
public class Goal_HarvestWood : GOAPGoal
{
    Memory memory;

    public override void Setup(){
        base.Setup();
        memory = GetComponent<Memory>();
        conditions["WoodHarvested"] = true;
        actionLayer = "Wood";
    }

    public override float GetPriority()
    {
        float demand = worldState.GetFloatState("People");
        demand *= worldState.GetFloatState("WoodExtractValue");
        return 1/(1+(worldState.GetFloatState("g_Wood")/demand));
    }

    public override bool PreconditionsSatisfied(WorldState worldState){
        return memory.InMemory("WoodStore");
    }

    public override void OnDeactivate()
    {
        worldState.RemoveBoolState("WoodHarvested");
    }
}