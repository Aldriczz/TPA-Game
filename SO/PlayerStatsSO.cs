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
    public int CurrentExperience;
    public int MaxExperience;
    public int Level;
    public int Zen;
    
    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
    }
}
