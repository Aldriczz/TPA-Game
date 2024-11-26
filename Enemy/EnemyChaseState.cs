using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims.Actions;
using UnityEngine;
using UnityEngine.UI;

public class EnemyChaseState : EnemyState
{
    public List<Tile> path = new List<Tile>();
    private PlayerStateMachine playerStateMachine;
    private Tile lastPlayerPosition;
    private Vector3 playerPosition;
    private Vector3 enemyPosition;
    private Coroutine movementCoroutine;

    public EnemyChaseState(EnemyStateMachine _stateMachine, Enemy _enemy) : base(_stateMachine, _enemy)
    {
        stateMachine = _stateMachine;
        enemy = _enemy;
    }

    public override void Enter()
    {
        stateMachine.isMoving = false;
        playerStateMachine = PlayerStateMachine.Instance;
        TurnGameManager.Instance.agroEnemies.Add(stateMachine);
        
        var AgroText = enemy.transform.Find("Canvas/Bar/Agro Text");
        AgroText.gameObject.SetActive(true);
    }

    public override void HandleInput() { }

    public override void LogicUpdate()
    {
        if (stateMachine.canDoAction && stateMachine.isRealized)
        {
            playerPosition = playerStateMachine.transform.position;
            enemyPosition = stateMachine.transform.position;

            if (Vector3.Distance(playerPosition, enemyPosition) > 1 && !stateMachine.isMoving)
            {
                enemy.animator.SetFloat("speed", 1f);
                stateMachine.isMoving = true;

                UpdatePathToPlayer();

                if (movementCoroutine != null)
                {
                    stateMachine.StopCoroutine(movementCoroutine);
                }

                movementCoroutine = stateMachine.StartCoroutine(MoveAlongPath(path));
            }
            else if (!stateMachine.isMoving && Vector3.Distance(playerPosition, enemyPosition) <= 1)
            {
                enemy.animator.SetFloat("speed", 0f); // Ensure idle animation
                Attack();
            }
        }
    }

    public override void PhysicsUpdate()
    {
        if (!stateMachine.isMoving && !stateMachine.isAttacking)
        {
            enemy.animator.SetFloat("speed", 0f);
        }
    }

    private void UpdatePathToPlayer()
    {
        var start = new Tile((int)stateMachine.transform.position.x, (int)stateMachine.transform.position.z);
        var end = new Tile((int)playerPosition.x, (int)playerPosition.z);

        if (lastPlayerPosition == null || lastPlayerPosition.x != end.x || lastPlayerPosition.y != end.y)
        {
            Debug.Log($"Recalculating path: Start ({start.x}, {start.y}), End ({end.x}, {end.y})");
            path = stateMachine.astar.Trace(start, end, DungeonGenerator.Instance.map);

            path = path.GetRange(0, Mathf.Min(1, path.Count));
            lastPlayerPosition = end;
        }
    }

    private IEnumerator MoveAlongPath(List<Tile> path)
    {
        DungeonGenerator.Instance.map[(int)stateMachine.transform.position.x, (int)stateMachine.transform.position.z] = ' ';
        foreach (var targetTile in path)
        {
            var targetPosition = new Vector3(targetTile.x, 0.75f, targetTile.y);
            while (Vector3.Distance(stateMachine.transform.position, targetPosition) > 0.01f)
            {
                stateMachine.transform.position = Vector3.MoveTowards(stateMachine.transform.position, targetPosition,
                    enemy.moveSpeed * Time.deltaTime);
                stateMachine.transform.forward = Vector3.Slerp(stateMachine.transform.forward,
                    (targetPosition - stateMachine.transform.position).normalized, 30 * Time.deltaTime);
                yield return null;
            }

            stateMachine.transform.position = targetPosition;
        }

        DungeonGenerator.Instance.map[(int)stateMachine.transform.position.x, (int)stateMachine.transform.position.z] = '#';
        path.Clear();
        stateMachine.isMoving = false;
        EndAction();
    }

    private void Attack()
    {
        if (!stateMachine.isAttacking)
        {
            stateMachine.isAttacking = true;
            var target = (playerStateMachine.transform.position - enemy.transform.position).normalized;
            enemy.transform.forward = target;

            float randomIndexAttack = Random.Range(1, 4);
            enemy.animator.SetTrigger($"attack{randomIndexAttack}");
        }
    }

    private void EndAction()
    {
        path.Clear();
        stateMachine.canDoAction = false;
    }

    public override void Exit()
    {
        if (movementCoroutine != null)
        {
            stateMachine.StopCoroutine(movementCoroutine);
            movementCoroutine = null;
        }

        path.Clear();
        stateMachine.isMoving = false;
    }
}

