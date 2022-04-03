using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GOAP{
public class WorldState 
{
    public Dictionary<string, Func<bool> > boolKeys;
    public Dictionary<string, Func<float> > floatKeys;
    public Dictionary<string, Func<int> > intKeys;
    public Dictionary<string, Func<string> > stringKeys;
    public Dictionary<string, Func<Vector3> > vector3Keys;
    public Dictionary<string, Func<Vector2> > vector2Keys;
    public Dictionary<string, Func<GameObject> > gameObjectKeys;

    public WorldState(){
        boolKeys = new Dictionary<string, Func<bool> >();
        floatKeys = new Dictionary<string, Func<float> >();
        intKeys = new Dictionary<string, Func<int> >();
        stringKeys = new Dictionary<string, Func<string> >();
        vector3Keys = new Dictionary<string, Func<Vector3> >();
        vector2Keys = new Dictionary<string, Func<Vector2> >();
        gameObjectKeys = new Dictionary<string, Func<GameObject> >();
    }

    public bool IsSubset(Dictionary<string, bool> state){
        foreach(var i in state){
            if (!boolKeys.ContainsKey(i.Key)){
                return false;
            }
            if (boolKeys[i.Key]() != i.Value){
                return false;
            }
        }
        return true;
    }
}
}