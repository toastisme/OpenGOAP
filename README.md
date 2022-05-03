# OpenGOAP

OpenGOAP is an open source tool to design and monitor goal orientated action planning in Unity.

## Features
- World state consisting of local (`GameObject` specific) states and global states
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

- A `GameObject` using GOAP has a `GOAPPlanner` and `WorldState` components, along with a series of `Goal` and `GOAPAction` components. 
- The `GOAPPlanner` finds the `Goal` with the highest priority that has a viable action plan (i.e a list of `GOAPActions`) and calls the active `GOAPAction` 
(calls its `OnTick` method). 
- An action plan is viable if the final action satisfies the conditions of the `Goal`, and each preceeding action satisfies the conditions of the action 
that follows it, where the first `GOAPAction`'s conditions are satisfied by the current `WorldState` 
- To find the optimum viable action plan the `GOAPPlanner` uses the [A* search algorithm](https://en.wikipedia.org/wiki/A*_search_algorithm) to find 
the series of actions which have the minimum cost.

### Harvest Wood Example

All `Goals` have the following interface
- `Setup()` called when the `GameObject` is first initialised
- `GetPriority()` value between 0 and 1 
- `PreconditionsSatisfied()` can be used to avoid searching for action plans due to some known requirement
- `ConditionsSatisfied()` true is the current `WorldState` satisfies the `Goal` conditions
- `OnTick()` called every frame by the `GOAPPlanner` for updating any values
- `OnComplete()` called by the `GOAPPlanner` when `ConditionsSatisfied()`
- `OnDeactivate()` by the `GOAPPlanner` when a plan is cancelled

