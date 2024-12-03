using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
 
    public static Player Instance { get; private set; } 
     
    [SerializeField] private Transform cam;
    
    [HideInInspector] public Animator animator;
    [HideInInspector] public List<Tile> resultMap;
    [HideInInspector] public LayerMask ClickableLayerMask;
    [HideInInspector] public CameraShake cameraShake;
    
    [SerializeField] private PlayerSkillsSO playerSkills;

    private InputControl inputControl;
        
    public bool isMoving = false; 
    public bool isClickedWhileMoving = false;
    public float moveSpeed = 4f;
    
    private float clipDuration;
    private float clipSpeed;

    public PlayerStatsSO stats;

    public GameObject Sword;
    
    private RaycastHit hit;

    private void Awake()
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
        cameraShake = cam.GetComponent<CameraShake>();
        AssignInput();
        
        ClickableLayerMask = LayerMask.GetMask("Enemy", "Ground");
        animator = GetComponent<Animator>();
        
        stats.CurrentHealth = stats.MaxHealth;
    }
    private void AssignInput()
    {
        inputControl.Player.Move.performed += ctx => OnClickAction();
        inputControl.Player.Skill1.performed += ctx => SkillSystem.Instance.ActivateSkill1();
        inputControl.Player.Skill2.performed += ctx => SkillSystem.Instance.ActivateSkill2();
        inputControl.Player.Skill3.performed += ctx => SkillSystem.Instance.ActivateSkill3();
        inputControl.Player.Skill4.performed += ctx => SkillSystem.Instance.ActivateSkill4();
    }
    
    public void EnableInput()
    {
        inputControl.Enable();
    }
    public void DisableInput()
    {
        inputControl.Disable();
    }
    private void OnEnable()
    {
        inputControl.Enable();
    }
    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void OnClickAction()
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
            }else if(hit.transform.tag == "Enemy" && Vector3.Distance(transform.position, hit.transform.position) > 1)
            {
                foreach(var skill in playerSkills.PlayerSkillsList)
                {
                    if (skill is ActiveSkill activeSkill)
                        if (activeSkill.GetName() == "Divine Arcane")
                        {
                            GetComponent<PlayerStateMachine>().RangeSkillUseEventChannel.RaiseGameObjectEvent(hit.transform.gameObject);
                        }
                }
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
        int Dmg;
        var DefenseImpact = hit.transform.GetComponent<Enemy>().Stat.DefenseImpact;
        var EnemyDefense = hit.transform.GetComponent<Enemy>().Stat.Defense;
        var RandomFactorDmg = Mathf.RoundToInt(Random.Range(-10f, 10f) * stats.Damage / 100f);
        
        var randomCritNumber = Random.Range(0, 100);
        GetComponent<PlayerStateMachine>().SkillReduceCooldownEventChannel.RaiseVoidEvent();
        GetComponent<PlayerStateMachine>().MeleeSkillUseEventChannel.RaiseGameObjectEvent(hit.transform.gameObject);

        if (randomCritNumber < stats.CritChance)
        {
            AudioManager.Instance.PlaySwordCriticalSlash(transform);    
            Dmg = Mathf.RoundToInt((float)((stats.Damage + RandomFactorDmg) * stats.CritDamage / 100f) * (1f - (float)EnemyDefense / (float)(EnemyDefense + DefenseImpact)));
            hit.transform.GetComponent<Enemy>().Gethit(Dmg, Color.red);
            StartCoroutine(cameraShake.Shake(0.1f, 0.07f));
        }
        else
        {
            AudioManager.Instance.PlaySwordSlash(transform);
            Dmg = Mathf.RoundToInt((float)(stats.Damage + RandomFactorDmg) * (1f - (float)EnemyDefense / (float)(EnemyDefense + DefenseImpact)));
            hit.transform.GetComponent<Enemy>().Gethit(Dmg, Color.white);
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
        PopUpUI.Instance.ShowGameOverPopUp();
    }
    
}
