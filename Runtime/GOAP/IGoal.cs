namespace GOAP{
public interface IGoal
{
    /**
     * \interface GOAP.IGoal
     * Basic interface for all GOAPGoals
     */

    float GetPriority(); // Assumed between 0f and 1f
    bool ConditionsSatisfied(WorldState worldState);
    bool PreconditionsSatisfied(WorldState worldState);
    void OnTick();
    void OnActivate();
    void OnDeactivate();
}
}