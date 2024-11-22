using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnGameManager : MonoBehaviour
{
    public static TurnGameManager Instance { get; private set; }
    public enum GameState {PlayerTurn, EnemyTurn} 
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

    // Start is called before the first frame update
    void Start()
    {
        agroEnemies = new List<EnemyStateMachine>();
        turnChangeEventChannel.RaiseEvent(currentGameState);
    }
    
    // Update is called once per frame
    void Update()
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
            // Debug.Log("Player Turn");
            PlayerStateMachine.Instance.player.EnableInput();
            turnChangeEventChannel.RaiseEvent(currentGameState);
        }
        else
        {
            // Debug.Log("Enemy Turn");
            PlayerStateMachine.Instance.player.DisableInput();
            StartCoroutine(HandleEnemyTurn());
        }
    }

    private IEnumerator HandleEnemyTurn()
    {
        var enemiesToProcess = new List<EnemyStateMachine>(agroEnemies); 
        foreach (var enemy in enemiesToProcess)
        {
            enemy.ChangeState(enemy.enemyChaseState);

            while (enemy.isMoving)
            {
                yield return null;
            }
        }
        SwitchGameState();
    }

}
