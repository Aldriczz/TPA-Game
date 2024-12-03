using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStatSO", menuName = "Character/Stat")]
public class PlayerStatsSO : ScriptableObject
{
    public int MaxHealth;
    public int CurrentHealth;
    public int Damage;
    public int CritChance;
    public int CritDamage;
    public int Defense;
    public int DefenseImpact;
    public int CurrentExperience;
    public int MaxExperience;
    public int Level;
    public int Zen;

    public int UnlockLevel;
    public int CurrentLevel;

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
    }

    public void Reset()
    {
        MaxHealth = 20;
        CurrentHealth = MaxHealth;
        Damage = 4;
        CritChance = 5;
        CritDamage = 150;
        Defense = 5;
        DefenseImpact = 100;
        CurrentExperience = 0;
        MaxExperience = 0;
        Level = 1;
        UnlockLevel = 1;
        CurrentLevel = 1;
        Zen = 0;
    }

    public void LevelUp()
    {
        MaxHealth = MaxHealth + MaxHealth * 5 / 100;
        Damage = Damage + Damage * 5 / 100;
        Defense = Defense + Defense * 5 / 100;
    }
}
