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
    
    SmartObject woodStore;

    public override float GetCost(){
        return 0.1f * worldState.GetFloatState("Fatigue");
    }
    public override void Setup(){
        base.Setup();
        movement = GetComponent<Movement>();
        inventory = GetComponent<Inventory>();
        memory = GetComponent<Memory>();
        worldState.AddState("WoodHarvested", false);
        woodStore = (SmartObject)memory.RememberNearest("WoodStore");
    }

    public override void OnActivated(){
        woodStore = (SmartObject)memory.RememberNearest("WoodStore");
        worldState.AddState("WoodHarvested", false);
        if (woodStore == null){
            StopAction();
            return;
        }
        movement.GoTo(woodStore);
    }

    public override void OnDeactivated(){
        worldState.AddState("WoodHarvested", false);
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

    public override bool PreconditionsSatisfied()
    {
        bool result = base.PreconditionsSatisfied();
        if (!result){
            return result;
        }
        else{
            return (woodStore != null);
        }
    } 

    protected override void SetupActionLayers(){
        base.SetupActionLayers();
        actionLayers.Add("Wood");
    }
    protected override void SetupEffects(){
        base.SetupEffects();
        effects["WoodHarvested"] = true;
        effects["g_WoodAvailable"] = true;
    }
    protected override void SetupConditions(){
        base.SetupConditions();
        preconditions["HoldingWood"] = true;
    }
}