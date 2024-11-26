using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public bool isAlive = true;
    
    [SerializeField] public bool isAgro;
    [SerializeField] public int MaxHealth;
    [SerializeField] public int damage;
    [SerializeField] public int CurrentHealth;
    [SerializeField] public float moveSpeed;

    [SerializeField] private Slider HPBar;
    [SerializeField] private Slider EaseHPBar;
    [SerializeField] private GameObject Ragdoll;

    private float timePassed = 0f;
    private void Start()
    {
        animator = GetComponent<Animator>();
        moveSpeed = 3f;
        MaxHealth = 100;
        CurrentHealth = MaxHealth;
        isAgro = false;
        damage = 5;
        HPBar.maxValue = MaxHealth;
        HPBar.value = CurrentHealth;
        EaseHPBar.maxValue = MaxHealth;
        EaseHPBar.value = CurrentHealth;
    }

    private void Update()
    {
        timePassed += Time.deltaTime;
        
        if (HPBar.value != EaseHPBar.value)
        {
            EaseHPBar.value = Mathf.Lerp(EaseHPBar.value, CurrentHealth,  2f * Time.deltaTime);
        }
        
        
    }
    
    private void StartAttack()
    {
        Player.Instance.GetComponent<Damageable>().TakeDamage(damage);
    }

    private void StopAttack()
    {
        GetComponent<EnemyStateMachine>().canDoAction = false;
        GetComponent<EnemyStateMachine>().isAttacking = false;
    }

    public void Gethit(int damage)
    {
        CurrentHealth -= Player.Instance.damage;
        HPBar.value = CurrentHealth;
        transform.forward = (Player.Instance.transform.position - transform.position).normalized; 
        animator.SetTrigger("gethit");

        if (CurrentHealth <= 0)
        {
            isAlive = false;
            animator.SetTrigger("die");
        }
    }

    private void Died()
    {
        DungeonGenerator.Instance.map[(int)transform.position.x, (int)transform.position.z] = ' ';
        TurnGameManager.Instance.agroEnemies.Remove(GetComponent<EnemyStateMachine>());
        gameObject.SetActive(false);
    }
}
