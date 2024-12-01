using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    private Enemy enemy;
    
    
    [HideInInspector] public EnemyState currentState;
    [HideInInspector] public EnemyIdleState enemyIdleState;
    [HideInInspector] public EnemyChaseState enemyChaseState;
    [HideInInspector] public EnemyAlertState enemyAlertState;
    [HideInInspector] public AStar astar;
    [HideInInspector] public bool canDoAction;
    [HideInInspector] public int layerMask;
    
    public bool isMoving = false;
    public bool isAttacking = false;
    public bool isRealized = false;
    public TurnChangeEventChannel turnChangeEventChannel;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        astar = new AStar();
        enemy.animator = GetComponent<Animator>();
        enemyIdleState = new EnemyIdleState(this, enemy);
        enemyChaseState = new EnemyChaseState(this, enemy);
        enemyAlertState = new EnemyAlertState(this, enemy);
        
        layerMask = LayerMask.GetMask("Player", "Obstacles");
        ChangeState(enemyIdleState);
        
    }

    private void Update()
    {
        currentState?.HandleInput();
        currentState?.LogicUpdate();
    }

    private void FixedUpdate()
    {
        currentState?.PhysicsUpdate();
    }

    
    public void ChangeState(EnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
    
    void OnDrawGizmos()
    {
     
            // Gizmos.color = Color.green;
            // Gizmos.DrawWireSphere(enemy.transform.position, 5);
            //
            // Gizmos.color = Color.yellow;
            // Vector3 leftBoundary = Quaternion.Euler(0, -45 / 2f, 0) * enemy.transform.forward * 45;
            // Vector3 rightBoundary = Quaternion.Euler(0, 45 / 2f, 0) * enemy.transform.forward * 45;
            //
            // Gizmos.DrawRay(enemy.transform.position, leftBoundary);
            // Gizmos.DrawRay(enemy.transform.position, rightBoundary);
            
    var playerpos = new Vector3(Player.Instance.transform.position.x, Player.Instance.transform.position.y + 0.5f, Player.Instance.transform.position.z);
    var enemypos = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 0.5f, enemy.transform.position.z);
    Vector3 direction = (playerpos - enemypos).normalized;
    Debug.DrawRay(enemypos, direction * 6f, Color.red);
    }

}
