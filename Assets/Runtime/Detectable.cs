using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sensors{
public class Detectable : MonoBehaviour
{
    void Start()
    {
        DetectableManager.Instance?.Add(this);
    }

    void OnDestroy(){
        DetectableManager.Instance?.Remove(this);
    }
}
}