using System.Collections;
using System.Collections.Generic;

namespace GOAP{
public interface IAction
{
    float GetCost(); // Between 0 and 1
    void OnActivated();
    void OnDeactivated();
    void OnTick();
    void Setup();
    bool SatisfiesConditions(Dictionary<string, bool> conditions);
    bool PreconditionsSatisfied(); // true if WorldState satisfies preconditions
    bool EffectsSatisfied(); // true if WorldState contains all effects of action
    
}
}