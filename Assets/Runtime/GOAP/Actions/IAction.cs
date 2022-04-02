using System.Collections;
using System.Collections.Generic;

namespace GOAP{
public interface IAction
{
    float GetCost(); // Between 0 and 1
    void OnActivated();
    void OnDeactivated();
    void OnTick();
    void Setup(ref WorldState worldState);
    bool SatisfiesCondition(string condition);
    bool SatisfiesState(WorldState state);
    bool CanRun(); // true if currentState satisfies preconditions
    
}
}