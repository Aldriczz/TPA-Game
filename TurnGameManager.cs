using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnGameManager : MonoBehaviour
{
    public static TurnGameManager Instance { get; private set; }
    public enum GameState { PlayerTurn, EnemyTurn } 
    private GameState currentGameState = GameState.PlayerTurn;
    [SerializeField] private TurnChangeEventChannel turnChangeEventChannel;
    [SerializeField] public List<EnemyStateMachine> agroEnemies;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        agroEnemies = new List<EnemyStateMachine>();
        turnChangeEventChannel.RaiseEvent(currentGameState);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchGameState();
        }
    }

    public void SwitchGameState()
    {
        currentGameState = currentGameState == GameState.PlayerTurn ? GameState.EnemyTurn : GameState.PlayerTurn;
        if (currentGameState == GameState.PlayerTurn)
        {
            Player.Instance.EnableInput();
            // turnChangeEventChannel.RaiseEvent(currentGameState);
        }
        else
        {
            Player.Instance.DisableInput();
            turnChangeEventChannel.RaiseEvent(currentGameState);
            StartCoroutine(HandleEnemyTurn());
        }
    }

    private IEnumerator HandleEnemyTurn()
    {
        List<EnemyStateMachine> AgroEnemies = new List<EnemyStateMachine>(agroEnemies);

        for(var i = 0; i < AgroEnemies.Count; i++){
            if (!AgroEnemies[i].isRealized)
            {
                AgroEnemies[i].isRealized = true;
                yield return null;
            }
            AgroEnemies[i].canDoAction = true;
            
            yield return new WaitUntil(() => AgroEnemies[i].canDoAction == false || AgroEnemies[i].GetComponent<Enemy>().isAlive == false);
        }

        SwitchGameState(); // End the enemy turn and switch back to the player
    }
}

