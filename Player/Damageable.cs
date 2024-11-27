using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damageable : MonoBehaviour
{
    private DamageText damageText; 
        
    public PlayerStatsSO PlayerStats;
    public IntEventChannel SetupHealthBarEvent;
    public IntEventChannel UpdateHealthBarEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        SetupHealthBarEvent.RaiseIntEvent(PlayerStats.MaxHealth);
        UpdateHealthBarEvent.RaiseIntEvent(PlayerStats.CurrentHealth);
        damageText = transform.Find("PlayerTextCanvas/Bar/Damage Text").GetComponent<DamageText>();
    }

    public void TakeDamage(int damage, Color color)
    {
        damageText.Activate(damage, color);
        Player.Instance.animator.SetTrigger("gethit");
        PlayerStats.CurrentHealth -= damage;
        UpdateHealthBarEvent.RaiseIntEvent(PlayerStats.CurrentHealth);
        
        if (PlayerStats.CurrentHealth <= 0)
        {
            GetComponent<Player>().animator.SetTrigger("die");
        }
    }
    
}
