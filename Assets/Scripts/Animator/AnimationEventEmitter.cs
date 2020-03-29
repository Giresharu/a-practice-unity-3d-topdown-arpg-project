using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventEmitter : MonoBehaviour
{
    public void EventEmitter(string eventName) {
        SendMessageUpwards(eventName);
    }
}
