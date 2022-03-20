using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP{
public class WorldState 
{
    public Dictionary<string, bool> boolKeys;
    public Dictionary<string, float> floatKeys;
    public Dictionary<string, int> intKeys;
    public Dictionary<string, string> stringKeys;
    public Dictionary<string, Vector3> vector3Keys;
    public Dictionary<string, Vector2> vector2Keys;
    public Dictionary<string, GameObject> gameObjectKeys;

    public WorldState(){
        boolKeys = new Dictionary<string, bool>();
        floatKeys = new Dictionary<string, float>();
        intKeys = new Dictionary<string, int>();
        stringKeys = new Dictionary<string, string>();
        vector3Keys = new Dictionary<string, Vector3>();
        vector2Keys = new Dictionary<string, Vector2>();
        gameObjectKeys = new Dictionary<string, GameObject>();
    }

    static public WorldState GetCombinedState(WorldState a, WorldState b){
        /**
         * Returns a combined WorldState of both a and b.
         * b will overwrite any duplicates in a.
         */
        WorldState combinedState = a;
        foreach (KeyValuePair<string, bool> entry in b.boolKeys){
            a.boolKeys[entry.Key] = entry.Value;
        }
        foreach (KeyValuePair<string, float> entry in b.floatKeys){
            a.floatKeys[entry.Key] = entry.Value;
        }
        foreach (KeyValuePair<string, int> entry in b.intKeys){
            a.intKeys[entry.Key] = entry.Value;
        }
        foreach (KeyValuePair<string, string> entry in b.stringKeys){
            a.stringKeys[entry.Key] = entry.Value;
        }
        foreach (KeyValuePair<string, Vector3> entry in b.vector3Keys){
            a.vector3Keys[entry.Key] = entry.Value;
        }
        foreach (KeyValuePair<string, Vector2> entry in b.vector2Keys){
            a.vector2Keys[entry.Key] = entry.Value;
        }
        foreach (KeyValuePair<string, GameObject> entry in b.gameObjectKeys){
            a.gameObjectKeys[entry.Key] = entry.Value;
        }
        return combinedState;
    }

    public bool IsSubset(WorldState otherState){
        /**
         * Returns true if all members of this are found in otherState
         */ 

        foreach (KeyValuePair<string, bool> entry in boolKeys){
            bool val;
            if (!(otherState.boolKeys.TryGetValue(entry.Key, out val) && val == entry.Value)){
                return false;
            }
        }
        foreach (KeyValuePair<string, float> entry in floatKeys){
            if (!(otherState.floatKeys.ContainsKey(entry.Key))){
                return false;
            }
            if (Mathf.Abs(otherState.floatKeys[entry.Key]) - Mathf.Abs(entry.Value) > 1E-5){
                return false;
            }
        }
        foreach (KeyValuePair<string, int> entry in intKeys){
            if (!(otherState.intKeys.ContainsKey(entry.Key))){
                return false;
            }
            if (otherState.intKeys[entry.Key] != entry.Value){
                return false;
            }
        }
        foreach (KeyValuePair<string, string> entry in stringKeys){
            if (!(otherState.stringKeys.ContainsKey(entry.Key))){
                return false;
            }
            if (otherState.stringKeys[entry.Key] != entry.Value){
                return false;
            }
        }
        foreach (KeyValuePair<string, Vector3> entry in vector3Keys){
            if (!(otherState.vector3Keys.ContainsKey(entry.Key))){
                return false;
            }
            if (otherState.vector3Keys[entry.Key] != entry.Value){
                return false;
            }
        }
        foreach (KeyValuePair<string, Vector2> entry in vector2Keys){
            if (!(otherState.vector2Keys.ContainsKey(entry.Key))){
                return false;
            }
            if (otherState.vector2Keys[entry.Key] != entry.Value){
                return false;
            }
        }
        foreach (KeyValuePair<string, GameObject> entry in gameObjectKeys){
            if (!(otherState.gameObjectKeys.ContainsKey(entry.Key))){
                return false;
            }
            if (otherState.gameObjectKeys[entry.Key] != entry.Value){
                return false;
            }
        }
        return true;
    }
}
}