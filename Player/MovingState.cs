using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : State
{
    private Vector3 velocity;
    private bool isMoving;
    public MovingState(PlayerStateMachine _stateMachine, Player _player, Transform _cam) : base(_stateMachine, _player, _cam)
    {
        stateMachine = _stateMachine;
        player = _player;
        cam = _cam;
    }

    public override void Enter()
    {
        StartMovingAlongPath(player.resultMap);
        player.animator.SetFloat("speed", 1f);
    }
    
    public void StartMovingAlongPath(List<Tile> path)
    {

        if (TurnGameManager.Instance.AlertEnemies.Count > 0 || TurnGameManager.Instance.AgroEnemies.Count > 0)
        {
            path = path.GetRange(0, Mathf.Min(1, path.Count));
        }
        stateMachine.StartCoroutine(MoveAlongPath(path));
    }
    
    private IEnumerator MoveAlongPath(List<Tile> path)
    {
        player.isMoving = true;
        // path.Reverse();
        foreach (Tile targetTile in path)
        {
            Vector3 targetPosition = new Vector3(targetTile.x, 0.75f, targetTile.y);
            AudioManager.Instance.PlayFootStep();

            while (Vector3.Distance(stateMachine.transform.position, targetPosition) > 0.01f)
            {
                stateMachine.transform.position = Vector3.MoveTowards(stateMachine.transform.position, targetPosition, player.moveSpeed * Time.deltaTime);
                Vector3 dir = (targetPosition - stateMachine.transform.position).normalized;
                stateMachine.transform.forward = Vector3.Slerp(stateMachine.transform.forward, dir, 30*Time.deltaTime);
                if (player.isClickedWhileMoving)
                {
                    player.isClickedWhileMoving = false;
                    while (Vector3.Distance(stateMachine.transform.position, targetPosition) > 0.01f)
                    {
                        stateMachine.transform.position = Vector3.MoveTowards(stateMachine.transform.position,
                            targetPosition, player.moveSpeed * Time.deltaTime);
                        Vector3 direction = (targetPosition - stateMachine.transform.position).normalized;
                        stateMachine.transform.forward =
                            Vector3.Slerp(stateMachine.transform.forward, direction, 30 * Time.deltaTime);
                        yield return null;
                    }

                    stateMachine.SkillReduceCooldownEventChannel.RaiseVoidEvent();
                    stateMachine.transform.position = targetPosition;
                    TurnGameManager.Instance.SwitchGameState();
                    player.isMoving = false;
                    yield break;
                }
                yield return null;
            }
            
            stateMachine.SkillReduceCooldownEventChannel.RaiseVoidEvent();
            stateMachine.transform.position = targetPosition;
        }

        player.isMoving = false;
        TurnGameManager.Instance.SwitchGameState();
    }

    public override void HandleInput()
    {
    }

    public override void LogicUpdate()
    {
        if (!player.isMoving)
        {
            stateMachine.ChangeState(stateMachine.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
    }

    public override void Exit()
    {
        HoverManager.Instance.path.Clear();
    }
}
