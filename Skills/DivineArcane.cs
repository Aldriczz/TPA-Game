using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DivineArcane : ActiveSkill
{
    public DivineArcane(Image _skillCooldown, GameObject _currentCooldownText, GameObject _skillEffect, GameObject _projectileFX) : base(_skillCooldown, _currentCooldownText,  _skillEffect, _projectileFX)
    {
        Name = "Divine Arcane";
        Description = "Divine Arcane - Six Powerful divine strike from the air dealing 40% damage";
        LevelRequired = 9;
        Cooldown = 15;
        CurrentCooldown = 0;
        CanBeUsed = true;
        isToggle = false;
        ActiveSkillType = "Range";

        DamageBoost = 0.4f;
    }    

    public override int SkillDamage(int PlayerDamage)
    {
        float cal = (float)PlayerDamage * DamageBoost;
        return (int)cal;
    }
}
