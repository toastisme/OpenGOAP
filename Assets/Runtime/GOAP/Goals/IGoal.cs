namespace GOAP{
public interface IGoal
{
    void Setup();
    float GetPriority(); // Between 0 and 1
    bool ConditionsSatisfied(WorldState worldState);
    bool PreconditionsSatisfied(WorldState worldState);
    void OnTick();
    void OnActivate();
    void OnDeactivate();
}
}