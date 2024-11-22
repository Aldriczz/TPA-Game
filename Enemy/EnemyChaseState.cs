using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    private float timePassed;
    private List<Tile> path = new List<Tile>();
    private PlayerStateMachine playerStateMachine;
    private Tile lastPlayerPosition;

    public EnemyChaseState(EnemyStateMachine _stateMachine, Enemy _enemy) : base(_stateMachine, _enemy)
    {
        stateMachine = _stateMachine;
        enemy = _enemy;
    }

    public override void Enter()
    {
        if (!enemy.isAgro)
        {
            TurnGameManager.Instance.agroEnemies.Add(stateMachine);
            enemy.animator.SetFloat("speed", 1f);
            timePassed = 0;
            playerStateMachine = PlayerStateMachine.Instance;
            stateMachine.isMoving = true;
            enemy.isAgro = true;

            UpdatePathToPlayer();
        }

        if (path.Count > 0)
        {
            stateMachine.StartCoroutine(MoveAlongPath(path));
        }
        else
        {
            stateMachine.isMoving = false;
        }
    }

    public override void HandleInput()
    {
    }

    public override void LogicUpdate()
    {
    }

    public override void PhysicsUpdate()
    {
        // If the enemy has lost sight of the player or reached the target, go idle
        if (enemy.isAgro == false)
        {
            stateMachine.ChangeState(stateMachine.enemyIdleState);
        }

        if (!stateMachine.isMoving)
        {
            enemy.animator.SetFloat("speed", 0f);

        }
    }

    private void UpdatePathToPlayer()
    {
        Tile start = new Tile((int)stateMachine.transform.position.x, (int)stateMachine.transform.position.z);
        Tile end = new Tile((int)playerStateMachine.transform.position.x, (int)playerStateMachine.transform.position.z);

        if (lastPlayerPosition == null || lastPlayerPosition.x != end.x || lastPlayerPosition.y != end.y)
        {
            path = stateMachine.astar.Trace(start, end, DungeonGenerator.Instance.map);
            path = path.GetRange(0, Mathf.Min(2, path.Count));
            lastPlayerPosition = end;
            Debug.Log("Path recalculated. Path length: " + path.Count);
        }
    }

    private IEnumerator MoveAlongPath(List<Tile> path)
    {
        foreach (Tile targetTile in path)
        {
            DungeonGenerator.Instance.map[(int)stateMachine.transform.position.x, (int)stateMachine.transform.position.z] = ' ';
            Vector3 targetPosition = new Vector3(targetTile.x, 0.75f, targetTile.y);

            while (Vector3.Distance(stateMachine.transform.position, targetPosition) > 0.01f)
            {
                stateMachine.transform.position = Vector3.MoveTowards(stateMachine.transform.position, targetPosition, enemy.moveSpeed * Time.deltaTime);
                Vector3 dir = (targetPosition - stateMachine.transform.position).normalized;
                stateMachine.transform.forward = Vector3.Slerp(stateMachine.transform.forward, dir, 30 * Time.deltaTime);

                yield return null;
            }

            stateMachine.transform.position = targetPosition;
            DungeonGenerator.Instance.map[(int)stateMachine.transform.position.x, (int)stateMachine.transform.position.z] = '#';
        }

        path.Clear();
        stateMachine.isMoving = false;
        enemy.isAgro = false;
    }

    public override void Exit()
    {
        if (stateMachine.isMoving)
        {
            stateMachine.StopAllCoroutines();
            path.Clear();
            stateMachine.isMoving = false;
        }
    }
}
