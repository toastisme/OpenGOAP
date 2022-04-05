using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Actor : Detectable
{
    /**
     * Class that can use SmartObjects.
     * Has an Inventory.
     */
    
    protected Inventory inventory;

    protected override void Start(){
        base.Start();
        inventory = GetComponent<Inventory>();
    }

     public virtual void PickUp(SmartObject obj){
         inventory.Add(obj);
         obj.PickedUp();
     }
     public virtual void PutDown(SmartObject obj){
         inventory.Remove(obj);
         obj.PutDown();
     }
     public virtual void AddTo(SmartObject obj, SmartObject objToAdd){
         inventory.Remove(objToAdd);
         obj.Add(objToAdd);
     }

}
