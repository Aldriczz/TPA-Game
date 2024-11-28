using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EventChannel", menuName = "Event Channels/GameObjectEventChannel")]
public class GameObjectEventChannel : ScriptableObject
{
    public event UnityAction<GameObject> OnRaiseGameObjectEvent;

    public void RaiseGameObjectEvent(GameObject gameObject)
    {
        OnRaiseGameObjectEvent?.Invoke(gameObject);
    }
}
