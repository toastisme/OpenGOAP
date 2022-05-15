using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GOAP;
using Sensors;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Memory))]
public class Action_TakeWoodToStore : GOAPAction
{

    Inventory inventory;
    Movement movement;
    Memory memory;
    
    [SerializeField]
    SmartObject woodStore;

    public override float GetCost(){
        return 0.1f * worldState.GetFloatState("Fatigue");
    }
    protected override void SetupDerived(){
        movement = GetComponent<Movement>();
        inventory = GetComponent<Inventory>();
        memory = GetComponent<Memory>();
        woodStore = (SmartObject)memory.RememberNearest("WoodStore");
    }

    protected override void OnActivateDerived(){
        woodStore = (SmartObject)memory.RememberNearest("WoodStore");
        if (woodStore == null){
            StopAction();
            return;
        }
        movement.GoTo(woodStore);
    }

    protected override void OnDeactivateDerived(){
        worldState.RemoveBoolState("WoodHarvested");
        StopAction();
    }

    public override void OnTick()
    {
        movement.GoTo(woodStore);
        if (movement.AtTarget()){
            for(int i = inventory.items["Wood"].Count -1; i >= 0; i--){
                woodStore.Add(inventory.items["Wood"][i]);
                inventory.Remove(inventory.items["Wood"][i]);
            }
            worldState.AddState("WoodHarvested", true);
        }
    }

    public override bool PreconditionsSatisfied(WorldState worldState)
    {
        bool result = base.PreconditionsSatisfied(worldState);
        if (!result){
            return result;
        }
        else{
            return (memory.RememberNearest("WoodStore") != null);
        }
    } 

    protected override void SetupActionLayers(){
        actionLayers.Add("Wood");
    }
    protected override void SetupEffects(){
        effects["WoodHarvested"] = true;
        effects["g_WoodAvailable"] = true;
    }
    protected override void SetupConditions(){
        preconditions["HoldingWood"] = true;
    }
}