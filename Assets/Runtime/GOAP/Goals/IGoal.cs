namespace GOAP{
public interface IGoal
{
    /**
     * \interface GOAP.IGoal
     * Basic interface for all GOAPGoals
     */

    void Setup();
    float GetPriority(); // Between 0 and 1
    bool ConditionsSatisfied(WorldState worldState);
    bool PreconditionsSatisfied(WorldState worldState);
    void OnTick();
    void OnActivate();
    void OnDeactivate();
}
}