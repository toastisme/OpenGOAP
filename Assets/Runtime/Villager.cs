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
[RequireComponent(typeof(WorldState))]
public class Villager : Actor
{
    Movement movement;
    Vision vision;
    Awareness awareness;
    Memory memory;
    Hearing hearing;
    GOAPPlanner planner;
    WorldState worldState;

    List<string> memorableObjectTypes = new List<string>{
        "FoodStore",
        "WoodStore"
    };

    protected override void Awake(){
        base.Awake();
        Setup();
    }

    protected override void Start(){
        base.Start();
    }

    void Update(){
        awareness.OnTick();
    }

    void Setup(){
        SetupWorldState();
        movement = GetComponent<Movement>();
        memory = GetComponent<Memory>();
        hearing = GetComponent<Hearing>();
        planner = GetComponent<GOAPPlanner>();
        SetupAwareness();
        SetupVision();
        SetupMemory();
    }

    void SetupWorldState(){
        worldState = GetComponent<WorldState>();
        worldState.SetGlobalState(
            GameObject.Find("TribeState").GetComponent<StateSet>()
            );
    }

    void SetupAwareness(){
        awareness = GetComponent<Awareness>();
    }

    void SetupVision(){
        vision = GetComponent<Vision>();
        vision.SetOnSeenDetectable(OnSeenDetectable);
    }

    void SetupMemory(){
        memory.Record(GameObject.Find("FoodStore").GetComponent<Detectable>());
        memory.Record(GameObject.Find("WoodStore").GetComponent<Detectable>());
    }

    public void OnSeenDetectable(Detectable detectable){
        awareness.Add(obj:detectable, memory:100f, memoryDecay:1f);
        if(Memorable(detectable)){
            memory.Record(obj:detectable);
        }
    }

    bool Memorable(Detectable detectable){
        if(memorableObjectTypes.Contains(detectable.typeName)){
            return true;
        }
        else{
            return false;
        }
    }
}
