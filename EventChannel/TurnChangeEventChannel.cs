using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event Channels/Turn Change Event")]
public class TurnChangeEventChannel : ScriptableObject
{
    public UnityAction<TurnGameManager.GameState> OnTurnChanged;

    public void RaiseEvent(TurnGameManager.GameState newTurnState)
    {
        OnTurnChanged?.Invoke(newTurnState);
    }
}