namespace GOAP{
public interface IGoal
{
    void Setup(WorldState worldState);
    float GetPriority(); // Between 0 and 1
    bool PreconditionsSatisfied();
    void OnTick();
    void OnActivated();
    void OnDeactivated();
}
}