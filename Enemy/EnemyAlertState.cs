using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlertState : EnemyState
{
    public EnemyAlertState(EnemyStateMachine _stateMachine, Enemy _enemy) : base( _stateMachine, _enemy)
    {
        stateMachine = _stateMachine;
        enemy = _enemy;
    }
    public override void Enter()
    {
        enemy.animator.SetFloat("speed", 0f);
        Debug.Log("ENEMY IN ALERT STATE");
    }

    public override void HandleInput()
    {
        
    }

    public override void LogicUpdate()
    {
        if (Vector3.Distance(PlayerStateMachine.Instance.transform.position, stateMachine.transform.position) <= 2)
        {
            stateMachine.ChangeState(stateMachine.enemyChaseState);
        }
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void Exit()
    {
        
    }
}
