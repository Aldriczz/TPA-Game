using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : State
{
    private float speed;
    private Vector3 velocity;
    private bool isMoving;
    private Coroutine moveCoroutine;
    public MovingState(PlayerStateMachine _stateMachine, Player _player, Transform _cam) : base(_stateMachine, _player, _cam)
    {
        stateMachine = _stateMachine;
        player = _player;
        cam = _cam;
    }

    public override void Enter()
    {
        speed = 0f;
        StartMovingAlongPath(player.resultMap);
    }
    
    public void StartMovingAlongPath(List<Tile> path)
    {
        if (moveCoroutine != null && player.isClickedWhileMoving)
        {
            stateMachine.StopCoroutine(moveCoroutine);
            
        }

        if (TurnGameManager.Instance.agroEnemies.Count > 0)
        {
            path = path.GetRange(0, Mathf.Min(2, path.Count));
        }
        moveCoroutine = stateMachine.StartCoroutine(MoveAlongPath(path));
    }
    
    private IEnumerator MoveAlongPath(List<Tile> path)
    {
        player.isMoving = true;
        // path.Reverse();
        foreach (Tile targetTile in path)
        {
            Vector3 targetPosition = new Vector3(targetTile.x, 0.75f, targetTile.y);
            
            while (Vector3.Distance(stateMachine.transform.position, targetPosition) > 0.01f)
            {
                stateMachine.transform.position = Vector3.MoveTowards(stateMachine.transform.position, targetPosition, player.moveSpeed * Time.deltaTime);
                Vector3 dir = (targetPosition - stateMachine.transform.position).normalized;
                stateMachine.transform.forward = Vector3.Slerp(stateMachine.transform.forward, dir, 30*Time.deltaTime);
                if (player.isClickedWhileMoving)
                {
                    player.isMoving = false;
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

                    stateMachine.transform.position = targetPosition;
                    TurnGameManager.Instance.SwitchGameState();
                    yield break;
                }
                yield return null;
            }
            
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
        if (speed > 1)
        {
            speed = 1;
        }
        player.animator.SetFloat("speed", speed);
        speed += Time.deltaTime * 3;

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
        // character.animator.SetFloat("speed", 0f);
        HoverManager.Instance.path.Clear();
    }
}
