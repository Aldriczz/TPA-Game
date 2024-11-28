using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneStrike : ActiveSkill
{
    public ArcaneStrike()
    {
        Name = "Arcane Strike";
        Description = "A Powerful True Damage Slash";
        LevelRequired = 3;
        Cooldown = 4;
        CurrentCooldown = 0;
        CanBeUsed = true;
        isToggle = false;

        DamageBoost = 0.3f;
    }    
    
    public override void ActivateSkill()
    {
        
    }

    public override int SkillDamage(int PlayerDamage)
    {
        float cal = (float)PlayerDamage * DamageBoost;
        return (int)cal;
    }
}
