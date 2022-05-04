using System.Collections;
using System.Collections.Generic;

namespace GOAP{
public interface IAction
{
    float GetCost(); // Between 0 and 1
    void OnActivate();
    void OnDeactivate();
    void OnTick();
    void Setup();
    bool SatisfiesConditions(Dictionary<string, bool> conditions);
    bool PreconditionsSatisfied(WorldState worldState); 
    bool EffectsSatisfied(WorldState worldState); 
    
}
}