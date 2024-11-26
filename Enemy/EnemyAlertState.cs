using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAlertState : EnemyState
{
    private Transform AlertText;
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
        
        if (Physics.Raycast(enemyPos, direction, out hit, 2f, stateMachine.layerMask))
        {
            if (hit.collider.name == "Player")
            {
                Player.Instance.isClickedWhileMoving = true;
                stateMachine.ChangeState(stateMachine.enemyChaseState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void Exit()
    {
        AlertText.gameObject.SetActive(false);
    }
}
