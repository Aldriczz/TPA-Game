using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseStat
{
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
    public string Type => "Common";
    public CommonEnemy(int multiplier)
    {
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
    public string Type => "Medium";
    public MediumEnemy(int multiplier)
    {
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
    public string Type => "Elit";
    public EliteEnemy(int multiplier)
    {
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

