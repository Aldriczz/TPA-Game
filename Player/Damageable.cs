using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damageable : MonoBehaviour
{
    private FloatingText damageText; 
        
    public PlayerStatsSO PlayerStats;
    public IntEventChannel SetupHealthBarEvent;
    public IntEventChannel UpdateHealthBarEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        SetupHealthBarEvent.RaiseIntEvent(PlayerStats.MaxHealth);
        UpdateHealthBarEvent.RaiseIntEvent(PlayerStats.CurrentHealth);
        damageText = transform.Find("PlayerTextCanvas/Bar/Floating Text").GetComponent<FloatingText>();
    }

    public void TakeDamage(int damage, Color color)
    {
        foreach  (var skill in SkillSlotController.Instance.PlayerSkills.PlayerSkillsList)
        {
            if(skill.GetName() == "Equinox" && skill is PassiveSkill passiveSkill)
                if (passiveSkill.isActive)
                {
                    AudioManager.Instance.PlayPunch();
                    damageText.ActivateDamageText(0, color);
                    passiveSkill.DeactivateSkill();
                    return;
                }
        }
       
        AudioManager.Instance.PlayGetHit();
        damageText.ActivateDamageText(damage, color);
        Player.Instance.animator.SetTrigger("gethit");
        PlayerStats.CurrentHealth -= damage;
        UpdateHealthBarEvent.RaiseIntEvent(PlayerStats.CurrentHealth);
        
        if (PlayerStats.CurrentHealth <= 0)
        {
            AudioManager.Instance.PlayDied();
            Destroy(GetComponent<PlayerStateMachine>());
            GetComponent<Player>().animator.SetTrigger("die");
        }
    }
    
}
