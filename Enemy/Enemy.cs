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

    [SerializeField] private GameObject Weapon;
    
    public IntEventChannel ZenUpdateEventChannel;
    public VoidEventChannel EnemyNumberUpdateEventChannel;

    private FloatingText _damageText;
    

    private float timePassed = 0f;
    private void Start()
    {
        animator = GetComponent<Animator>();
        _damageText = transform.Find("Canvas/Bar/Damage Text").GetComponent<FloatingText>();
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
        var DefenseImpact = Player.Instance.stats.DefenseImpact;
        var PlayerDefense = Player.Instance.stats.Defense;
        var RandomFactorDmg = Mathf.RoundToInt(Random.Range(-10f, 10f) * Stat.Damage / 100f);
        int Dmg;
        if (randomCritChance < Stat.CritChance)
        {
            AudioManager.Instance.PlaySwordCriticalSlash(transform);
            Dmg = Mathf.RoundToInt((float)((Stat.Damage + RandomFactorDmg) * Stat.CritDamage / 100f) *
                                   (1f - (float)PlayerDefense / (float)(PlayerDefense + DefenseImpact)));
            Player.Instance.GetComponent<Damageable>().TakeDamage(Dmg, Color.red);
            StartCoroutine(Player.Instance.cameraShake.Shake(0.1f, 0.07f));
        }
        else
        {
            switch (Stat.Type)
            {
                case "Common":
                    AudioManager.Instance.PlayPunch(transform);
                    break;
                default:
                    AudioManager.Instance.PlaySwordSlash(transform);
                    break;
            }
            Dmg = Mathf.RoundToInt((float)(Stat.Damage + RandomFactorDmg) * (1f - (float)PlayerDefense / (float)(PlayerDefense + DefenseImpact)));
            Player.Instance.GetComponent<Damageable>().TakeDamage(Dmg, Color.white);
        }
    }

    private void StopAttack()
    {
        GetComponent<EnemyStateMachine>().canDoAction = false;
        GetComponent<EnemyStateMachine>().isAttacking = false;
    }

    public void Gethit(int damage, Color color)
    {
        _damageText.ActivateDamageText(damage, color);
        
        Stat.CurrentHealth -= damage;
        HPBar.value = Stat.CurrentHealth;
        transform.forward = (Player.Instance.transform.position - transform.position).normalized;
        if (Stat.Type == "Elite" || Stat.Type == "Boss")
        {
            if(color == Color.red) animator.SetTrigger($"gethit{1}");
            else animator.SetTrigger($"gethit{2}");
        }
        else
            animator.SetTrigger("gethit");
        
        AudioManager.Instance.PlayGetHit(transform);

        if (Stat.CurrentHealth <= 0)
        {
            AudioManager.Instance.PlayDied(transform);
            
            isAlive = false;
            Destroy(GetComponent<BoxCollider>());
            DungeonGenerator.Instance.map[(int)transform.position.x, (int)transform.position.z] = ' ';
            animator.SetTrigger("die");
            
            EntitySpawnerManager.Instance.enemyList.Remove(GetComponent<EnemyStateMachine>());
            TurnGameManager.Instance.AgroEnemies.Remove(GetComponent<EnemyStateMachine>());
            
            if(TurnGameManager.Instance.AgroEnemies.Count == 0) AudioManager.Instance.PlayDungeonBackgroundMusic();
        }
    }

    private void Died()
    {
        ExpManager.Instance.AddExp(Stat.ExpDrop);
        ZenUpdateEventChannel.RaiseIntEvent(Stat.ZenDrop);
        EnemyNumberUpdateEventChannel.RaiseVoidEvent();
        
        if (EntitySpawnerManager.Instance.enemyList.Count == 0)
        {
            PopUpUI.Instance.ShowFloorClearedPopUp();
        }
        Destroy(gameObject);
    }

    private void ShowWeapon()
    {
        if (Weapon != null)
        {
            Weapon.SetActive(true);
        }
    }

    private void HideWeapon()
    {
        if (Weapon != null)
        {
            Weapon.SetActive(false);
        }
    }
}
