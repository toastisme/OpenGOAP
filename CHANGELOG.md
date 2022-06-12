# Changelog

## [0.1.2]

### Fixed
- Bug in `GOAPPlanner.GetNextNode` where a null `ActionNode` could override a valid next node, resulting in a valid path not being found by the planner.

## [0.1.1]

### Added
- `ComponentGrouper` class to better organise `GOAPActions` and `GOAPGoals` in the inspector.

### Changed
- Made `WorldState` a required component of `GOAPPlanner`
