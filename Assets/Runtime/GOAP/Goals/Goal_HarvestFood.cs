using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using Sensors;

[RequireComponent(typeof(Memory))]
public class Goal_HarvestFood : GOAPGoal
{
    Memory memory;
    public override void Setup(){
        base.Setup();
        memory = GetComponent<Memory>();
        conditions["FoodHarvested"] = true;
        actionLayer = "Food";
    }

    public override float GetPriority()
    {
        float demand = worldState.GetFloatState("People");
        demand *= worldState.GetFloatState("FoodExtractValue");
        return 1/(1+(worldState.GetFloatState("g_Food")/demand));
    }

    public override bool PreconditionsSatisfied(WorldState worldState){
        return memory.InMemory("FoodStore");
    }
}