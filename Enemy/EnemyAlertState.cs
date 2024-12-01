using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAlertState : EnemyState
{
    private Transform AlertText;
    private bool playerInSight = false;
    private float fieldOfViewAngle = 45f;

    public EnemyAlertState(EnemyStateMachine _stateMachine, Enemy _enemy) : base( _stateMachine, _enemy)
    {
        stateMachine = _stateMachine;
        enemy = _enemy;
    }
    public override void Enter()
    {
        enemy.animator.SetFloat("speed", 0f);
        AlertText = enemy.transform.Find("Canvas/Bar/Alert Text");
        AlertText.gameObject.SetActive(true);
        TurnGameManager.Instance.AlertEnemies.Add(stateMachine);
        
        AudioManager.Instance.PlayAlert(enemy.transform);
    }

    public override void HandleInput()
    {
        
    }

    public override void LogicUpdate()
    {
        RaycastHit hit;
        
        var playerPos = new Vector3(Player.Instance.transform.position.x, Player.Instance.transform.position.y + 0.5f, Player.Instance.transform.position.z);
        var enemyPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 0.5f, enemy.transform.position.z);
        var direction = (playerPos - enemyPos).normalized;
        
        var angleToPlayer = Vector3.Angle(enemy.transform.forward, direction);

        if (angleToPlayer <= fieldOfViewAngle / 2f)
        {
            playerInSight = true;
        }

        if (Vector3.Distance(playerPos, enemyPos) > 6)
        {
            stateMachine.ChangeState(stateMachine.enemyIdleState);
        }
        
        if (Physics.Raycast(enemyPos, direction, out hit, 3f, stateMachine.layerMask))
        {
            if (hit.collider.name == "Player")
            {
                stateMachine.ChangeState(stateMachine.enemyChaseState);
            }
        }
        if (Physics.Raycast(enemyPos, direction, out hit, 6f, stateMachine.layerMask))
        {
            if (hit.collider.tag == "Player")
            {

                if (playerInSight)
                {
                    stateMachine.ChangeState(stateMachine.enemyChaseState);
                }
            }
        }
        
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void Exit()
    {
        AlertText.gameObject.SetActive(false);
        TurnGameManager.Instance.AlertEnemies.Remove(stateMachine);
    }
}
