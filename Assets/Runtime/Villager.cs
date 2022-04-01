using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sensors;
using GOAP;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Vision))]
[RequireComponent(typeof(Awareness))]
[RequireComponent(typeof(Memory))]
[RequireComponent(typeof(Hearing))]
[RequireComponent(typeof(GOAPPlanner))]
public class Villager : Detectable
{
    Movement movement;
    Vision vision;
    Awareness awareness;
    Memory memory;
    Hearing hearing;
    GOAPPlanner planner;
    WorldState worldState;

    void Setup(){
        movement = GetComponent<Movement>();
        memory = GetComponent<Memory>();
        hearing = GetComponent<Hearing>();
        planner = GetComponent<GOAPPlanner>();
        SetupAwareness();
        SetupVision();
        worldState = new WorldState();
    }

    void SetupAwareness(){
        awareness = GetComponent<Awareness>();
        awareness.SetWorldState(ref worldState);
    }

    void SetupVision(){
        vision = GetComponent<Vision>();
        vision.SetOnSeenDetectable(OnSeenDetectable);
    }

    public void OnSeenDetectable(Detectable detectable){
        awareness.Add(obj:detectable, memory:100f, memoryDecay:1f);
        memory.Record(obj:detectable);
    }



}
