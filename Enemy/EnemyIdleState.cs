using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private float fieldOfViewAngle = 45f;
    private bool playerInSight = false;
    public EnemyIdleState(EnemyStateMachine _stateMachine, Enemy _enemy) : base( _stateMachine, _enemy)
    {
        stateMachine = _stateMachine;
        enemy = _enemy;
    }
    public override void Enter()
    {
        enemy.animator.SetFloat("speed", 0f);
    }

    public override void HandleInput()
    {
        
    }

    public override void LogicUpdate()
    {
        RaycastHit hit;

        var playerPos = new Vector3(Player.Instance.transform.position.x, Player.Instance.transform.position.y + 0.5f, Player.Instance.transform.position.z);
        var enemyPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 0.5f, enemy.transform.position.z);
        var direction = (playerPos - enemyPos).normalized;
        
        var angleToPlayer = Vector3.Angle(enemy.transform.forward, direction);

        if (angleToPlayer <= fieldOfViewAngle / 2f)
        {
            playerInSight = true;
        }

        if (Vector3.Distance(playerPos, enemyPos) <= 6)
        {
                Player.Instance.isClickedWhileMoving = true;
                
                if (!playerInSight)
                {
                    stateMachine.ChangeState(stateMachine.enemyAlertState);
                }
                else
                {
                    stateMachine.ChangeState(stateMachine.enemyChaseState);
                }
        }
        else
        {
            stateMachine.ChangeState(stateMachine.enemyIdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void Exit()
    {
        
    }
}
