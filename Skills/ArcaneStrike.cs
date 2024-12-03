using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcaneStrike : ActiveSkill
{
    public ArcaneStrike(Image _skillCooldown, GameObject _currentCooldownText, GameObject _skillEffect, GameObject _projectileFX) : base(_skillCooldown, _currentCooldownText,  _skillEffect, _projectileFX)
    {
        Name = "Arcane Strike";
        Description = "Arcane Strike - A Powerful lightning strike ignoring enemy's defense dealing 30% attack";
        LevelRequired = 3;
        Cooldown = 4;
        CurrentCooldown = 0;
        CanBeUsed = true;
        isToggle = false;
        ActiveSkillType = "Melee";

        DamageBoost = 0.3f;
    }    

    public override int SkillDamage(int PlayerDamage)
    {
        float cal = (float)PlayerDamage * DamageBoost;
        return (int)cal;
    }
}
