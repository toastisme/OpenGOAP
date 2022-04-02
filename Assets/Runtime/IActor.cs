using UnityEngine;

public interface IActor
{
    void PickUp(SmartObject obj);
    void PutDown(SmartObject obj);
    void AddTo(SmartObject obj, SmartObject objToAdd);

}
