using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
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
        if (Vector3.Distance(PlayerStateMachine.Instance.transform.position, stateMachine.transform.position) <= 4 && !enemy.isAgro)
        {
            stateMachine.ChangeState(stateMachine.enemyAlertState);
        }
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void Exit()
    {
        
    }
}
