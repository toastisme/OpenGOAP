# OpenGOAP

OpenGOAP is an open source tool to design and monitor goal orientated action planning in Unity.
![OpenGOAP](https://github.com/toastisme/OpenGOAP/blob/main/Runtime/Screenshots/HarvestFoodPlan.PNG?raw=true)

## Features
- World state consisting of local (`GameObject` specific) and global states
- A* planner for selecting the current optimum plan
- Action layers to improve performance (e.g. avoid checking if sleeping is a viable action when running from danger)
- GUI to visualise the current active plan and goals in priority order
- Optional logger for additional debugging

## Installation

Tested on Windows 10 using Unity 2021.2.11f1
- Install Git (e.g. for Windows https://gitforwindows.org/)
- In Unity open the package manager (`Window` --> `Package Manager`)
- Use the plus button to add a new package, and choose `Add package from git URL`
- Add the URL `https://github.com/toastisme/OpenGOAP.git`

## Getting Started

### How OpenGOAP works

- A `GameObject` using GOAP has a `GOAPPlanner`, `WorldState`, and a series of `GOAPGoal` and `GOAPAction` components. 
- The `GOAPPlanner` finds the `GOAPGoal` with the highest priority that has a viable action plan and executes that plan.
- An action plan is a list of `GOAPActions`, and is viable if the final `GOAPAction` satisfies the conditions of the `GOAPGoal`, and each preceeding `GOAPAction` satisfies the conditions of the action that follows it, where the first `GOAPAction`'s conditions are satisfied by the current `WorldState`.
- To find the optimum viable action plan the `GOAPPlanner` uses the [A* search algorithm](https://en.wikipedia.org/wiki/A*_search_algorithm) to find 
the series of actions which have the minimum cost.

#### GOAPGoal

A `GOAPGoal` has a dictionary of boolean `conditions` that need to be met to satisfy the goal, and optionally a dictionary of boolean `preconditions` that must be met before it can be considered (beyond having a viable plan). Each `GOAPGoal` belongs to an `actionLayer` (string), which tells the planner only consider `GOAPActions` for this goal that have the same `actionLayer` (by default this is set to All, where all `GOAPActions` are considered). For this component the main interface is the following:
- `SetupDerived()` called when the `GameObject` is first initialised
- `OnActivate()` called the when goal is first selected by the `GOAPPlanner`
- `OnDeactivate()` called when the goal is deselected by the `GOAPPlanner` (either due to completing the goal or finding a better one)
- `GetPriority()` value between 0 and 1 
- `PreconditionsSatisfied(WorldState)` can be used to avoid searching for action plans due to some known requirement
- `ConditionsSatisfied(WorldState)` true if the aim of the goal is satisfied (i.e the current `WorldState` satisfies the `GOAPGoal` `conditions`)
- `OnTick()` called every frame by the `GOAPPlanner` 

#### GOAPAction

A `GOAPAction` has a dictionary of boolean `preconditions` that need to be met before it can run, and a dictionary of boolean `effects` that will occur as a result of running to completion. For this component the main interface is the following:
- `SetupDerived()` called when the `GameObject` is first initialised. Used for e.g. getting components required for the action
- `SetupEffects()` called when the `GameObject` is first initialised. Used to populate the effects boolean dictionary
- `SetupActionLayers()` called when the `GameObject` is first initalised. Used to populate which `actionLayers` the action belongs to
- `OnActivateDerived()` called the when action is first selected by the `GOAPPlanner`
- `OnDeactivateDerived()` called when the action is deselected by the `GOAPPlanner` (either due to completing the action or changing plan)
- `GetCost()` value between 0 and 1
- `PreconditionsSatisfied(WorldState)` Can this action run based on `WorldState`
- `EffectsSatisfied(WorldState)` Are the action's effects all present in `WorldState`
- `SatisfiesConditions(Dictionary<string, bool>)` Does this action satisfy all boolean conditions in the dictionary
- `OnTick()` called every frame by the `GOAPPlanner`

#### WorldState

A `WorldState` is composed of two `StateSets`, one global and one local. The global `StateSet` is common to multiple `GameObjects`, whereas the local
`StateSet` is specific to the `GameObject` the `WorldState` is attached to. A `StateSet` is simply several dictionaries of strings mapped to values (analogous to a blackboard for behaviour trees). 
The `WorldState` differentiates between local and global states by assuming a `g_` prefix for all global states. 
I.e, if you call `WorldState.AddState("InDanger", true)`, this would be added to the local `StateSet`, whereas `WorldState.AddState("g_InDanger", true)` 
would be added to the global `StateSet` and apply to all other `GameObjects` sharing the same `Stateset`.
By default, an absent boolean key is assumed to be equivalent to the key being present with a false value. This can be turned off using `SetGlobalDefaultFalse` and `SetLocalDefaultFalse` for the global ahd local `StateSets`, respectively. (The motivation for this is to avoid the need of requiring many boolean states to properly define a particular `WorldState`. For example, if the goal is to harvest wood, a viable plan could be to take wood from the wood store and put it back in. To avoid this it's simpler to have a `woodExtractedFromStore = true` effect added to a `Action_TakeWoodFromStore`, rather than having `woodExtractedFromStore = false` on all other approaches.)

### HarvestWood Example

Consider an agent that has a goal of harvesting wood. The goal script could look something like this:

```
using UnityEngine;
using GOAP;

public class Goal_HarvestWood : GOAPGoal
{

    protected override void SetupDerived(){
        conditions["WoodHarvested"] = true; // GOAPPlanner will consider the goal complete when this condition is in the WorldState
        actionLayer = "Wood"; // Only actions in this layer will be considered by the GOAPPlanner for this goal
    }

    public override float GetPriority()
    {
        /*
         * Priority depends on the number of known people, 
         * the current global wood level (g_wood), and the 
         * amount of wood obtained from a single harvest (WoodExtractValue)
         */
    
        float demand = worldState.GetFloatState("People");
        demand *= worldState.GetFloatState("WoodExtractValue");
        return 1/(1+(worldState.GetFloatState("g_Wood")/demand));
    }
}
```
We now need a series of `GOAPActions`, atleast one of which has `"WoodHarvested" == true` in their `effects` dictionary. One of these could be taking wood to the wood store:

```
using UnityEngine;
using GOAP;

public class Action_TakeWoodToStore : GOAPAction
{
 
    protected override void SetupActionLayers(){
        actionLayers.Add("Wood"); // This action is in the same layer as Goal_HarvestWood. GOAPActions can belong to many actionLayers.
    }
    
    protected override void SetupEffects(){
        effects["WoodHarvested"] = true; // This action satisfies the conditions of Goal_HarvestWood
        effects["g_WoodAvailableAtStore"] = true; // Lets other GameObjects know wood is at the store
    }
    
    protected override void SetupConditions(){
        preconditions["HoldingWood"] = true; // Cannot perform this action unless holding wood
    }
    
    public override float GetCost(){
        return worldState.GetFloatState("Fatigue"); // action cost increases with fatigue
    }

    protected override void OnActivateDerived(){
        /* Called when the action is first selected by the GOAPPlanner.
           Some code here could identify the wood store position */
    }

    protected override void OnDeactivateDerived(){
        if (worldState.InBoolStates("WoodHarvested")){
            worldState.RemoveBooleanState("WoodHarvested"); // state no longer needed
        }
    }

    public override void OnTick()
    {
        /*
         * Move towards wood store
         * If at wood store deposit wood and call 
         * AddTemporaryState("WoodHarvested, true") 
         * TemporaryStates are automatically removed when the action completes
         * This is useful for states that are no longer relevant after the action completes, and saves you needing to 
         * remember to remove it manually.
         */
    }
}
```
This action has a precondition of `"HoldingWood" == true`, and so we could have another action `Action_PickUpWood`, which picks up the nearest wood, given the precondition `"WoodNearby"` is true. This preconditon in turn could be in the `effects` dictionary of both `Action_ChopDownTree` and `Action_LookAround`. The latter could have a higher cost than the former, and so would only be selected by the `GOAPPlanner` if, say, the `GameObject` did not have an axe. `Action_LookAround` could have no `preconditions`, and so would always be viable from the current `WorldState`. 

To have a `GameObject` utilise these behaviours simply add the goal and action scripts, along with a `GOAPPlanner` and `WorldState` script to the `GameObject` as components. The global `StateSet` is kept on a separate (empty) `GameObject`. This can be added in the inspector on the `WorldState` component, or added in code via `WorldState.SetGlobalState(StateSet)`. 

### Visualisation and Debugging

The `GOAPPlanner` has a boolean `Display Planner` in the inspector. If this is set to true, when clicking on the `GameObject` in the Hierarchy navigation bar, a `GUIPlanner` window will be displayed showing the current active plan, and the priorities of all goals. For a given `GOAPGoal`, if `PreconditionsSatisfied() == false`, the goal will be greyed out.

Additional debugging can be done by adding a `GOAPLogger` to the scene (`Packages/OpenGOAP/Runtime/Prefabs/GOAPLogger`). This contains a logger for the active plan and another for the planner, which can be assigned to a `GameObject`'s `GOAPPlanner` in the inspector. These can be turned on or off individually on the loggers themselves, in the inspector, by selecting `Show Logs`. The Planner logger will print log statements for each step through the A* process when finding the optimum plan. The ActivePlan logger will essentially print log statements with the same information as the `GUIPlanner` window, but this can still be useful (for example for identifying if plans are being selected/completing instantly, repeatedly, due to a condition in `WorldState` not being reset correctly).

![OpenGOAP](https://github.com/toastisme/OpenGOAP/blob/main/Runtime/Screenshots/GOAPLoggers.PNG?raw=true)

## Running Tests
- In your project packages folder, open the manifest file and add `"com.davidmcdonagh-opengoap"` to `"testables"` (see the Enabling tests for a package section [here](https://docs.unity3d.com/Manual/cus-tests.html#tests)).
- In the Editor open `Window -> General -> Test Runner
- Under `PlayMode` you should now see tests for OpenGOAP

# TODO
- Partial plans
- Fixed sequences of actions

## Further Info
- [Full documentation](https://toastisme.github.io/OpenGOAP/)
- See also [OpenBehaviourTree](https://github.com/toastisme/OpenBehaviourTree)



