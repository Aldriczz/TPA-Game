using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseStat
{
    public string Type {get; set;}
    public int CurrentHealth { get; set;}
    public int MaxHealth { get; set;}
    public int Damage { get; set;}
    public int CritChance { get; set;}
    public int CritDamage { get; set;} 
    public int Defense { get; set;}
    public int ExpDrop { get; set;}
    public int ZenDrop { get; set;}
    
    public int DefenseImpact { get; set;}
    
    public GameObject Prefab { get; set;}
    
}

public class CommonEnemy : EnemyBaseStat
{
    public CommonEnemy(int multiplier)
    {
        Type ="Common";
        MaxHealth = 15 * multiplier + Random.Range(0, multiplier) * 2;
        CurrentHealth = MaxHealth;
        Damage = 2 * multiplier;
        CritChance = 1 + multiplier / 2;
        CritDamage = 120 + multiplier;
        Defense = 5 * multiplier;
        ExpDrop = Random.Range(5, 10);
        ZenDrop = Random.Range(5, 14) + 3 * multiplier;
        DefenseImpact = 150;
    }
}

public class MediumEnemy : EnemyBaseStat
{
    public MediumEnemy(int multiplier)
    {
        Type ="Medium";
        MaxHealth = 20 * multiplier + Random.Range(0, multiplier) * 2;
        CurrentHealth = MaxHealth;
        Damage = 3 * multiplier;
        CritChance = 3 + multiplier / 2;
        CritDamage = 130 + multiplier;
        Defense = 10 * multiplier;
        ExpDrop = Random.Range(10, 30);
        ZenDrop = Random.Range(15, 30) + 3 * multiplier;
        DefenseImpact = 100;
    }
}

public class EliteEnemy : EnemyBaseStat
{
    public EliteEnemy(int multiplier)
    {
        Type ="Elite";
        MaxHealth = 30 * multiplier + Random.Range(0, multiplier) * 2;
        CurrentHealth = MaxHealth;
        Damage = 5 * multiplier;
        CritChance = 5 + multiplier / 2;
        CritDamage = 150 + multiplier;
        Defense = 20 * multiplier;
        ExpDrop = Random.Range(30, 60);
        ZenDrop = Random.Range(31, 50) + 3 * multiplier;
        DefenseImpact = 150;
    }
}

public class BossEnemy : EnemyBaseStat
{
    public BossEnemy()
    {
        Type ="Boss";
        MaxHealth = 10000;
        CurrentHealth = MaxHealth;
        Damage = 1000;
        CritChance = 50;
        CritDamage = 150;
        Defense = 200;
        ExpDrop = 2000;
        ZenDrop = 10000;
        DefenseImpact = 50;
    }
}

