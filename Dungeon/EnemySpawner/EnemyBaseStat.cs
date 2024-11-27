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
    public GameObject Prefab { get; set;}
    
}

public class CommonEnemy : EnemyBaseStat
{
    public string Type => "Common";
    public CommonEnemy(int multiplier)
    {
        // multiplier /= 2;
        MaxHealth = 10 * multiplier;
        CurrentHealth = MaxHealth;
        Damage = 2 * multiplier;
        CritChance = 1 * multiplier;
        CritDamage = 5 * multiplier;
        Defense = 5 * multiplier;
    }
}

public class MediumEnemy : EnemyBaseStat
{
    public string Type => "Medium";
    public MediumEnemy(int multiplier)
    {
        multiplier /= 2;
        MaxHealth = 15 * multiplier;
        CurrentHealth = MaxHealth;
        Damage = 3 * multiplier;
        CritChance = 2 * multiplier;
        CritDamage = 10 * multiplier;
        Defense = 10 * multiplier;
    }
}

public class EliteEnemy : EnemyBaseStat
{
    public string Type => "Medium";
    public EliteEnemy(int multiplier)
    {
        multiplier /= 2;
        MaxHealth = 25 * multiplier;
        CurrentHealth = MaxHealth;
        Damage = 5 * multiplier;
        CritChance = 5 * multiplier;
        CritDamage = 20 * multiplier;
        Defense = 20 * multiplier;
    }
}

