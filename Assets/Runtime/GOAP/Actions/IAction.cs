using System.Collections;
using System.Collections.Generic;

namespace GOAP{
public interface IAction
{
    /**
     * \interface GOAP.IAction
     * Basic interface for all GOAPActions
     */

    void Setup();
    float GetCost(); // Assumed between 0f and 1f
    bool SatisfiesConditions(Dictionary<string, bool> conditions);
    bool PreconditionsSatisfied(WorldState worldState); 
    bool EffectsSatisfied(WorldState worldState); 
    void OnTick();
    void OnActivate();
    void OnDeactivate();
    
}
}