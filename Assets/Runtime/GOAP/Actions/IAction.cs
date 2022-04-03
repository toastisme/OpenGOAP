using System.Collections;
using System.Collections.Generic;

namespace GOAP{
public interface IAction
{
    float GetCost(); // Between 0 and 1
    void OnActivated();
    void OnDeactivated();
    void OnTick();
    void Setup(ref WorldState worldState, ref Inventory inventory);
    bool SatisfiesConditions(Dictionary<string, bool> conditions);
    bool CanRun(); // true if currentState satisfies preconditions
    bool EffectsSatisfied(); // true if WorldState contains all effects of action
    
}
}