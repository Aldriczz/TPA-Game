using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHealthSO", menuName = "Character/Health")]
public class HealthSO : ScriptableObject
{
    public int MaxHealth;
    public int CurrentHealth;

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
    }
}
