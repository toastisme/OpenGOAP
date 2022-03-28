using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sensors{
public class Hearing : MonoBehaviour
{
    void Start()
    {
        HearingManager.Instance.Add(this);
    }

    void OnDestroy(){
        HearingManager.Instance?.Remove(this);
    }

    public virtual void OnHeardSound(
        GameObject source,
        Vector3 location,
        SoundPacket data,
        float intensity
    ){}
}
}