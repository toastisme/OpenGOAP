using UnityEngine;

public interface ISmartObject 
{
    void PickedUp(){}
    void PutDown(){}
    void Add(SmartObject obj){}
    void Remove(){}
    
}
