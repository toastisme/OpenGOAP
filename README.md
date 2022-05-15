# OpenGOAP

OpenGOAP is an open source tool to design and monitor goal orientated action planning in Unity.
![OpenGOAP](https://github.com/toastisme/OpenGOAP/blob/main/Assets/Runtime/GOAP/Screenshots/HarvestFoodPlan.PNG?raw=true)

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

- A `GameObject` using GOAP has a `GOAPPlanner` and `WorldState` components, along with a series of `GOAPGoal` and `GOAPAction` components. 
- The `GOAPPlanner` finds the `GOAPGoal` with the highest priority that has a viable action plan executes that plan.
- An action plan is a list of `GOAPActions`, and is viable if the final `GOAPAction` satisfies the conditions of the `GOAPGoal`, and each preceeding `GOAPACtion` satisfies the conditions of the action that follows it, where the first `GOAPAction`'s conditions are satisfied by the current `WorldState`.
- To find the optimum viable action plan the `GOAPPlanner` uses the [A* search algorithm](https://en.wikipedia.org/wiki/A*_search_algorithm) to find 
the series of actions which have the minimum cost.

#### GOAPGoal

A `GOAPGoal` has a dictionary of boolean `conditions` that need to be met to satisfy the goal, and optionally a dictionary of boolean `preconditions` that must be met before it can be considered (beyond having a viable plan). This component has the following interface:
- `SetupDerived()` called when the `GameObject` is first initialised
- `OnActivate()` called the when goal is first selected by the `GOAPPlanner`
- `OnDeactivate()` called when the goal is deselected by the `GOAPPlanner` (either due to completing the goal or finding a better one)
- `GetPriority()` value between 0 and 1 
- `PreconditionsSatisfied(WorldState)` can be used to avoid searching for action plans due to some known requirement
- `ConditionsSatisfied(WorldState)` true if the aim of the goal is satisfied (i.e the current `WorldState` satisfies the `GOAPGoal` `conditions`)
- `OnTick()` called every frame by the `GOAPPlanner` 

#### GOAPAction

A `GOAPAction` has a dictionary of `preconditions` that need to be met before it can run, and a dictionary of boolean `effects` that will occur as a result of running to completion. This component has the following interface:
- `SetupDerived()` called when the `GameObject` is first initialised
- `OnActivate()` called the when action is first selected by the `GOAPPlanner`
- `OnDeactivate()` called when the action is deselected by the `GOAPPlanner` (either due to completing the action or changing plan)
- `GetCost()` value between 0 and 1
- `PreconditionsSatisfied(WorldState)` Can this action run based on `WorldState`
- `EffectsSatisfied(WorldState)` Are the action's effects all present in `WorldState`
- `SatisfiesConditions(Dictionary<string, bool>)` Does this action satisfy all boolean conditions in the dictionary
- `OnTick()` called every frame by the `GOAPPlanner`

#### WorldState

A `WorldState` is composed of two `StateSets`, one global and one local. The global `StateSet` is common to multiple `GameObjects`, whereas the local
`StateSet` is specific to the `GameObject` the `WorldState` is attached to. A `StateSet` is simply several dictionaries of strings mapped to values. 
The `WorldState` differentiates between local and global states by assuming a `g_` prefix for all global states. 
I.e, if you call `WorldState.AddState("InDanger", true)`, this would be added to the local `StateSet`, whereas `WorldState.AddState("g_InDanger", true)` 
would be added to the global `StateSet` and apply to all other `GameObjects` sharing the same `Stateset`.
By default, an absent boolean key is assumed to be equivalent to the key being present with a false value. This can be turned off for each `StateSet` with the `defaultFalse` parameter (visible in the inspector).

### HarvestWood Example

Consider an agent that has a goal of harvesting wood. The goal script could look something like this:

```
using UnityEngine;
using GOAP;

public class Goal_HarvestWood : GOAPGoal
{

    public override void SetupDerived(){
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
We now need a series of `GOAPActions`, atleast one of which has `"WoodHarvested"` in their `effects` dictionary. One of these could be taking wood to the wood store:

```
using UnityEngine;
using GOAP;

public class Action_TakeWoodToStore : GOAPAction
{
 
    protected override void SetupActionLayers(){
        actionLayers.Add("Wood"); // This action is in the same layer as Goal_HarvestWood
    }
    protected override void SetupEffects(){
        effects["WoodHarvested"] = true;
        effects["g_WoodAvailableAtStore"] = true; // Lets other GameObjects know wood is at the store
    }
    protected override void SetupConditions(){
        preconditions["HoldingWood"] = true;
    }
    
   public override float GetCost(){
        return worldState.GetFloatState("Fatigue"); // action cost increases with fatigue
    }

    public override void OnActivateDerived(){
        /* Identify wood store position */
    }

    public override void OnDeactivateDerived(){
        if (worldState.InBoolStates("WoodHarvested")){
            worldState.RemoveBooleanState("WoodHarvested"); // state no longer needed
        }
    }

    public override void OnTick()
    {
        /*
         * Move towards wood store
         * If at wood store deposite wood and call 
         * worldState.AddTemporaryState("WoodHarvested, true")
         */
    }
}
```
This action has a precondition of `"HoldingWood" being true`, and so we could have another action `Action_PickUpWood`, which picks up the nearest wood, given the precondition `"WoodNearby"` is true. This preconditon in turn could be in the `effects` dictionary of both `Action_ChopDownTree` and `Action_LookAround`. The latter could have a higher cost than the former, and so would only be selected by the `GOAPPlanner` if, say, the `GameObject` did not have an axe. 

To have a `GameObject` utilise these behaviours simply add the goal and action scripts, along with a `GOAPPlanner` and `WorldState` script to the `GameObject` as components. The global `StateSet` can be kept on a separate `GameObject` and added in the inspector on the `WorldState` component, or added in code via `WorldState.SetGlobalState(StateSet)`. 

### Visualisation and Debugging

The `GOAPPlanner` has a boolean `Display Planner` in the inspector. If this is set to true, when clicking on `GameObject` in the Hierarchy navigation bar, a `GUIPlanner` window will be displayed of the current active plan, and the priorities of all goals. For a given `GOAPGoal`, if `PreconditionsSatisfied() == false`, the goal will be greyed out.

Additional debugging can be done by adding a `GOAPLogger` to the scene. This contains a logger for the active plan and one for the planner, which can be assigned to a `GameObject`'s `GOAPPlanner` in the inspector. These can be turned on or off individually on the loggers themselves, in the inspector, by selecting `Show Logs`. The Planner logger will print log statements for each step through the A* process when finding the optimum plan. The ActivePlan logger will essentially print log statements with the same information as the `GUIPlanner` window, but this can still be useful (for example for identifying if plans are being selected/completing instantly, repeatedly, due to a condition in `WorldState` not being reset correctly).





