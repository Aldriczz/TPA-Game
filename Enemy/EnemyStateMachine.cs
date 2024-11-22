using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    [HideInInspector] public EnemyState currentState;
    [HideInInspector] public EnemyIdleState enemyIdleState;
    [HideInInspector] public EnemyChaseState enemyChaseState;
    [HideInInspector] public EnemyAlertState enemyAlertState;
    [HideInInspector] public Enemy enemy;
    [HideInInspector] public AStar astar;
    public float detectionRange = 10f; // How far the enemy can see
    public float detectionAngle = 45f; // Angle of the enemy's field of view
    public LayerMask obstacleMask;
    
    public bool isMoving = false;
     public TurnChangeEventChannel turnChangeEventChannel;

    private void Start()
    {
        enemy = new Enemy();
        astar = new AStar();
        enemy.animator = GetComponent<Animator>();
        enemyIdleState = new EnemyIdleState(this, enemy);
        enemyChaseState = new EnemyChaseState(this, enemy);
        enemyAlertState = new EnemyAlertState(this, enemy);
        
        ChangeState(enemyIdleState);
        if (PlayerInLineOfSight())
        {
            Debug.Log("PLAYYEYEYYEYE");
        }
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

    private void OnEnable()
    {
        turnChangeEventChannel.OnTurnChanged += OnTurnChanged;
    }

    private void OnDisable()
    {
        turnChangeEventChannel.OnTurnChanged -= OnTurnChanged;
    }

    private void OnTurnChanged(TurnGameManager.GameState gameState)
    {
        if (gameState == TurnGameManager.GameState.PlayerTurn)
        {
            enemy.isAgro = false;
        }
    }
    
    public void ChangeState(EnemyState newState)
    {
        Debug.Log("ChangeState: " + newState.ToString());
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
    
    private bool PlayerInLineOfSight()
    {
        Vector3 directionToPlayer = (PlayerStateMachine.Instance.transform.position - transform.position).normalized;

        // Check distance
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerStateMachine.Instance.transform.position);
        if (distanceToPlayer > detectionRange)
            return false;

        // Check angle
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer > detectionAngle / 2)
            return false;

        // Check for obstacles
        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, detectionRange, obstacleMask))
        {
            if (hit.transform == PlayerStateMachine.Instance.transform)
            {
                return true; // Player is visible
            }
        }

        return false; // Player is not visible
    }
    
    void OnDrawGizmos()
    {
        if (PlayerStateMachine.Instance.transform == null) return;

        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw field of view
        Vector3 leftBoundary = Quaternion.Euler(0, -detectionAngle / 2, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, detectionAngle / 2, 0) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * detectionRange);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * detectionRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(6, 3, 6));
    }

}
