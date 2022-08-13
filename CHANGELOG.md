# Changelog

## [0.1.6]

### Fixed
- Erroneous default check of `GOAPAction.stopAction_==false` in `GOAPAction.PreconditionsSatisfied()`

## [0.1.5]

### Fixed
- `StateSet.GetBoolState` no longer throws a key error if `StateSet.defaultFalse==true` and the state is not present.

## [0.1.4]

### Changed
- `GOAPPlanner` now allows for actions in a plan to be skipped if the preconditions of a subsequent action are met. 

## [0.1.3]

### Fixed
- Bug in `GOAPPlanner.OnCompleteActivePlan` and `GOAPPlanner.OnFailActivePlan` where `GOAPAction.OnDeactivate` was not called for the active action.

## [0.1.2]

### Fixed
- Bug in `GOAPPlanner.GetNextNode` where a null `ActionNode` could override a valid next node, resulting in a valid path not being found by the planner.

## [0.1.1]

### Added
- `ComponentGrouper` class to better organise `GOAPActions` and `GOAPGoals` in the inspector.

### Changed
- Made `WorldState` a required component of `GOAPPlanner`
