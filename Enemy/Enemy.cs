using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public EnemyBaseStat Stat { get; set; }
    [HideInInspector] public bool isAlive = true;
    
    [SerializeField] public bool isAgro;
    [SerializeField] public float moveSpeed;

    [SerializeField] private Slider HPBar;
    [SerializeField] private Slider EaseHPBar;

    private DamageText _damageText;
    

    private float timePassed = 0f;
    private void Start()
    {
        animator = GetComponent<Animator>();
        _damageText = transform.Find("Canvas/Bar/Damage Text").GetComponent<DamageText>();
        moveSpeed = 3f;
        isAgro = false;
        HPBar.maxValue = Stat.MaxHealth;
        HPBar.value = Stat.CurrentHealth;
        EaseHPBar.maxValue = Stat.MaxHealth;
        EaseHPBar.value = Stat.CurrentHealth;
    }

    private void Update()
    {
        timePassed += Time.deltaTime;
        
        if (HPBar.value != EaseHPBar.value)
        {
            EaseHPBar.value = Mathf.Lerp(EaseHPBar.value, Stat.CurrentHealth,  2f * Time.deltaTime);
        }
        
        
    }
    
    private void StartAttack()
    {
        var randomCritChance = Random.Range(0, 100);
        if (randomCritChance < Stat.CritChance)
        {
            Player.Instance.GetComponent<Damageable>().TakeDamage(Stat.Damage * Stat.CritDamage / 100, Color.red);
        }
        else
        {
            Player.Instance.GetComponent<Damageable>().TakeDamage(Stat.Damage, Color.white);
        }
    }

    private void StopAttack()
    {
        GetComponent<EnemyStateMachine>().canDoAction = false;
        GetComponent<EnemyStateMachine>().isAttacking = false;
    }

    public void Gethit(int damage, Color color)
    {
        _damageText.Activate(damage, color);
        
        Stat.CurrentHealth -= damage;
        HPBar.value = Stat.CurrentHealth;
        transform.forward = (Player.Instance.transform.position - transform.position).normalized; 
        animator.SetTrigger("gethit");

        if (Stat.CurrentHealth <= 0)
        {
            isAlive = false;
            animator.SetTrigger("die");
        }
    }

    private void Died()
    {
        DungeonGenerator.Instance.map[(int)transform.position.x, (int)transform.position.z] = ' ';
        TurnGameManager.Instance.agroEnemies.Remove(GetComponent<EnemyStateMachine>());
        ExpManager.Instance.AddExp(Stat.ExpDrop);
        gameObject.SetActive(false);
    }
}
