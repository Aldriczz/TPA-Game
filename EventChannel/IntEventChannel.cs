using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "IntEventChannel", menuName = "Event Channels/IntEventChannel")]
public class IntEventChannel : ScriptableObject
{
    public event UnityAction<int> OnRaiseIntEvent;

    public void RaiseIntEvent(int value)
    {
        OnRaiseIntEvent?.Invoke(value);
    }
}
