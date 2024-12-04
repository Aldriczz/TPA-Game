using System.Collections;
using System.Collections.Generic;
using RPGCharacterAnims.Actions;
using UnityEngine;
using UnityEngine.UI;

public class EnemyChaseState : EnemyState
{
    public List<Tile> path;
    private PlayerStateMachine playerStateMachine;
    private Tile lastPlayerPosition;
    private Vector3 playerPosition;
    private Vector3 enemyPosition;

    public EnemyChaseState(EnemyStateMachine _stateMachine, Enemy _enemy) : base(_stateMachine, _enemy)
    {
        stateMachine = _stateMachine;
        enemy = _enemy;
    }

    public override void Enter()
    {
        stateMachine.isRealized = false;
        stateMachine.isMoving = false;
        playerStateMachine = PlayerStateMachine.Instance;
        TurnGameManager.Instance.AgroEnemies.Add(stateMachine);
        TurnGameManager.Instance.AlertEnemies.Remove(stateMachine);
        List<Tile> path = new List<Tile>();
        
        var AgroText = enemy.transform.Find("Canvas/Bar/Agro Text");
        AgroText.gameObject.SetActive(true);
        
        AudioManager.Instance.PlayAgro();
        var target = (playerStateMachine.transform.position - enemy.transform.position).normalized;
        enemy.transform.forward = target;
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
                DungeonGenerator.Instance.map[(int)stateMachine.transform.position.x, (int)stateMachine.transform.position.z] = ' ';

                if(path == null) SearchAlternatePath();
                
                stateMachine.StartCoroutine(MoveAlongPath(path));
                
            }
            else if (!stateMachine.isMoving && Vector3.Distance(playerPosition, enemyPosition) <= 1)
            {
                enemy.animator.SetFloat("speed", 0f);
                Attack();
            }
            stateMachine.UpdateRangeSkillPositionEventChannel.RaiseVoidEvent();
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
        
        path = stateMachine.astar.Trace(start, end, DungeonGenerator.Instance.map);
        
        if(path != null)  path = path.GetRange(0, Mathf.Min(1, path.Count));
    }

    private IEnumerator MoveAlongPath(List<Tile> path)
    {
        foreach (var targetTile in path)
        {
            AudioManager.Instance.PlayFootStep();
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

    private void SearchAlternatePath()
    {
        Vector3 TempPath;
        Vector3 AlternatePath = new Vector3(999999, 999999, 999999);
        int[] moveX = { 0, 1,  0, -1, 0 };
        int[] moveY = { 1, 0, -1,  0, 0 };

        for (var i = 0; i < moveX.Length; i++)
        {
            TempPath = new Vector3(enemy.transform.position.x + moveX[i],enemy.transform.position.y, enemy.transform.position.z + moveY[i]);
            
            if (DungeonGenerator.Instance.map[(int)TempPath.x, (int)TempPath.z] == '#' || DungeonGenerator.Instance.map[(int)TempPath.x, (int)TempPath.z] == '.') continue;
            
            if (Vector3.Distance(TempPath, Player.Instance.transform.position) < Vector3.Distance(AlternatePath, Player.Instance.transform.position))
            {
                AlternatePath = TempPath;
            }
            Debug.Log("Iterate: " + i);
        }
        path = new List<Tile>();
        path.Add(new Tile((int) AlternatePath.x,(int) AlternatePath.z));
        Debug.Log(AlternatePath);
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
        path.Clear();
        stateMachine.isMoving = false;
    }
}