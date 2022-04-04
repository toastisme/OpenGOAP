namespace GOAP{
public interface IGoal
{
    void Setup();
    float GetPriority(); // Between 0 and 1
    bool PreconditionsSatisfied();
    void OnTick();
    void OnActivated();
    void OnDeactivated();
}
}