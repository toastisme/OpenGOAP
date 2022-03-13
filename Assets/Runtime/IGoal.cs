namespace GOAP{
public interface IGoal
{
    void Setup();
    float GetPriority(); // Between 0 and 1
    bool CanRun();
    void OnTick();
    void OnActivated(GOAPAction linkedAction);
    void OnDeactivated();
}
}