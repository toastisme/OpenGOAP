using System.Collections;
using System.Collections.Generic;

namespace GOAP{
public interface IAction
{
    List<System.Type> GetSupportedGoals();
    float GetCost(); // Between 0 and 1
    void OnActivated(Goal linkedGoal);
    void OnDeactivated();
    void OnTick();
    void Setup();
}
}