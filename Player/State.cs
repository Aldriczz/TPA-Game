using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
   protected Player player;
   protected Transform cam;
   protected PlayerStateMachine stateMachine;
   
   protected State(PlayerStateMachine _stateMachine, Player _player, Transform _cam)
   {
      stateMachine = _stateMachine;
      player = _player;
      cam = _cam;
   }

   public abstract void Enter();
   public abstract void HandleInput();
   public abstract void LogicUpdate();
   public abstract void PhysicsUpdate();

   public abstract void Exit();
}
