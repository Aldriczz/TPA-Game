using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public IdleState(PlayerStateMachine _stateMachine, Player _player, Transform _cam) : base(_stateMachine, _player, _cam)
    {
        stateMachine = _stateMachine;
        player = _player;
        cam = _cam;
    }

    public override void Enter()
    {
        player.animator.SetFloat("speed", 0f);
    }

    public override void HandleInput()
    {
    }

    public override void LogicUpdate()
    {
        if (player.isMoving)
        {
            stateMachine.ChangeState(stateMachine.movingState);
        }
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void Exit()
    {
        
    }
}
