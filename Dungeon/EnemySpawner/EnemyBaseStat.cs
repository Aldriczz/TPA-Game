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
    
    public GameObject Prefab { get; set;}
    
}

public class CommonEnemy : EnemyBaseStat
{
    public CommonEnemy(int multiplier)
    {
        Type ="Common";
        MaxHealth = 10 * multiplier + Random.Range(0, multiplier) * 2;
        CurrentHealth = MaxHealth;
        Damage = 2 * multiplier;
        CritChance = 1 + multiplier / 2;
        CritDamage = 5 + multiplier;
        Defense = 5 * multiplier;
        ExpDrop = Random.Range(1, 5);
        ZenDrop = Random.Range(3, 9) + 2 * multiplier;
    }
}

public class MediumEnemy : EnemyBaseStat
{
    public MediumEnemy(int multiplier)
    {
        Type ="Medium";
        MaxHealth = 15 * multiplier + Random.Range(0, multiplier) * 2;
        CurrentHealth = MaxHealth;
        Damage = 3 * multiplier;
        CritChance = 3 + multiplier / 2;
        CritDamage = 10 + multiplier;
        Defense = 10 * multiplier;
        ExpDrop = Random.Range(6, 12);
        ZenDrop = Random.Range(9, 18) + 2 * multiplier;
    }
}

public class EliteEnemy : EnemyBaseStat
{
    public EliteEnemy(int multiplier)
    {
        Type ="Elite";
        MaxHealth = 25 * multiplier + Random.Range(0, multiplier) * 2;
        CurrentHealth = MaxHealth;
        Damage = 5 * multiplier;
        CritChance = 5 + multiplier / 2;
        CritDamage = 20 + multiplier;
        Defense = 20 * multiplier;
        ExpDrop = Random.Range(12, 18);
        ZenDrop = Random.Range(18, 31) + 2 * multiplier;
    }
}

