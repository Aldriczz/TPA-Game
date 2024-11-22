using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState
{
    protected Enemy enemy;
    protected EnemyStateMachine stateMachine;

    protected EnemyState(EnemyStateMachine _stateMachine, Enemy _enemy)
    {
        stateMachine = _stateMachine;
        enemy = _enemy;
    }
    
    public abstract void Enter();
    public abstract void HandleInput();
    public abstract void LogicUpdate();
    public abstract void PhysicsUpdate();

    public abstract void Exit();
}