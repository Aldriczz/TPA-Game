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
    [SerializeField] public List<EnemyStateMachine> AgroEnemies;
    [SerializeField] public List<EnemyStateMachine> AlertEnemies;

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
        AgroEnemies = new List<EnemyStateMachine>();
        AlertEnemies = new List<EnemyStateMachine>();
        turnChangeEventChannel.RaiseEvent(currentGameState);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerStateMachine.Instance.SkillReduceCooldownEventChannel.RaiseVoidEvent();
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
        List<EnemyStateMachine> NewAgroEnemies = new List<EnemyStateMachine>(AgroEnemies);

        for(var i = 0; i < NewAgroEnemies.Count; i++){
            if (NewAgroEnemies[i].isRealized)
            {
                NewAgroEnemies[i].canDoAction = true;
            }
            else
            {
                NewAgroEnemies[i].isRealized = true;
            }
            
            yield return new WaitUntil(() => NewAgroEnemies[i].canDoAction == false || NewAgroEnemies[i].GetComponent<Enemy>().isAlive == false);
        }

        SwitchGameState(); // End the enemy turn and switch back to the player
    }
}

