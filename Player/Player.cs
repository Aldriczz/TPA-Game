using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
 
    public static Player Instance { get; private set; } 
    
    [HideInInspector] public Animator animator;
    [HideInInspector] public List<Tile> resultMap;
    [HideInInspector] public LayerMask ClickableLayerMask;
    private InputControl inputControl;
    
    public bool isMoving = false; 
    public bool isClickedWhileMoving = false;
    public float moveSpeed = 4f;
    
    
    private float clipDuration;
    private float clipSpeed;

    public PlayerStatsSO stats;

    public GameObject Sword;
    
    private RaycastHit hit;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
            
        inputControl = new InputControl();
        AssignInput();
        
        ClickableLayerMask = LayerMask.GetMask("Enemy", "Ground");
        animator = GetComponent<Animator>();
        
    }

    private void Update()
    {
        
    }

    private void AssignInput()
    {
        inputControl.Player.Move.performed += ctx => OnClickMove();
        inputControl.Player.Skill1.performed += ctx => SkillSystem.Instance.ActivateSkill1();
        inputControl.Player.Skill2.performed += ctx => SkillSystem.Instance.ActivateSkill2();
        EnableInput();
    }
    
    public void EnableInput()
    {
        inputControl.Enable();
    }

    public void DisableInput()
    {
        inputControl.Disable();
    }

    private void OnClickMove()
    {
        if (isMoving)
        {
            isClickedWhileMoving = true;
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, ClickableLayerMask))
        {
            if (hit.transform.tag == "Enemy" && Vector3.Distance(transform.position, hit.transform.position) <= 1)
            {
                DisableInput();
                StartAttackAnimation();
            }
            
            else if (hit.collider.tag == "Tile")
            {
                resultMap = HoverManager.Instance.path;
                if (resultMap != null && resultMap.Count > 0 && resultMap.Count <= 15)
                {
                    isMoving = true;
                }
                else
                {
                    Debug.Log("No valid path found.");
                }
            }
        }
    }

    private void StartAttackAnimation()
    {
        var target = (hit.transform.position - transform.position).normalized;
        transform.forward = target;

        var random = Random.Range(1, 4);
        if (SkillSlotController.Instance.PlayerSkills.PlayerSkillsList.Count == 0)
        {
            animator.SetTrigger($"attack{random}");
        }
        else
        {
            if (SkillSlotController.Instance.PlayerSkills.PlayerSkillsList[0] != null &&
                SkillSlotController.Instance.PlayerSkills.PlayerSkillsList[0] is ActiveSkill activeSkill)
            {
                if (activeSkill.isToggle)
                {
                    animator.SetTrigger($"arcanestrike");
                }
                else
                {
                    animator.SetTrigger($"attack{random}");
                }
            }
        }
    }

    private void StartAttack()
    {
        var randomCritNumber = Random.Range(0, 100);
        GetComponent<PlayerStateMachine>().SkillReduceCooldownEventChannel.RaiseVoidEvent();
        GetComponent<PlayerStateMachine>().SkillUseEventChannel.RaiseGameObjectEvent(hit.transform.gameObject);

        if (randomCritNumber < stats.CritChance)
        {
            hit.transform.GetComponent<Enemy>().Gethit(stats.Damage * stats.CritDamage / 100, Color.red);
        }
        else
        {
            hit.transform.GetComponent<Enemy>().Gethit(stats.Damage, Color.white);
        }
    }

    private void EndAttack()
    {
        TurnGameManager.Instance.SwitchGameState();
    }

    private void ShowWeapon()
    {
        Sword.SetActive(true);
    }
    
    private void HideWeapon()
    {
        Sword.SetActive(false);
    }

    private void Died()
    {
        DisableInput();
    }
    
}
