using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcaneStrike : ActiveSkill
{
    public ArcaneStrike(Image _skillCooldown, GameObject _currentCooldownText, GameObject _skillEffect) : base(_skillCooldown, _currentCooldownText,  _skillEffect)
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

    public override int SkillDamage(int PlayerDamage)
    {
        float cal = (float)PlayerDamage * DamageBoost;
        return (int)cal;
    }
}
