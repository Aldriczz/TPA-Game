using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New VoidEventChannel", menuName = "Event Channels/VoidEventChannel")]
public class VoidEventChannel : ScriptableObject
{
    public event UnityAction OnRaiseVoidEvent;

    public void RaiseVoidEvent()
    {
        OnRaiseVoidEvent?.Invoke();
    }
}