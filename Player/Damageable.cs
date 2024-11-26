using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public HealthSO PlayerHealth;
    public IntEventChannel SetupHealthBarEvent;
    public IntEventChannel UpdateHealthBarEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        SetupHealthBarEvent.RaiseIntEvent(PlayerHealth.MaxHealth);
        UpdateHealthBarEvent.RaiseIntEvent(PlayerHealth.CurrentHealth);
    }

    public void TakeDamage(int damage)
    {
        Player.Instance.animator.SetTrigger("gethit");
        PlayerHealth.CurrentHealth -= damage;
        UpdateHealthBarEvent.RaiseIntEvent(PlayerHealth.CurrentHealth);
    }
    
}
