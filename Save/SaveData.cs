using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int MaxHealth;
    public int CurrentHealth;
    public int Damage;
    public int CritChance;
    public int CritDamage;
    public int Defense;
    public int CurrentExperience;
    public int MaxExperience;
    public int Level;
    public int Zen;
    public int UnlockLevel;
    public int CurrentLevel;
    
    public int HealthUP;
    public int AttackUP;
    public int DefenseUP;
    public int LuckUP;
    public int CritDmgUp;

    public int HealthUPCost;
    public int AttackUPCost;
    public int DefenseUPCost;
    public int LuckUPCost;
    public int CritDmgUPCost;
}
