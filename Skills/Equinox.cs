using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equinox : PassiveSkill
{
  public Equinox(Image _skillCooldown, GameObject _currentCooldownText, GameObject _skillEffect, GameObject _buffDurationImage, GameObject _currentBuffDurationText) : base(_skillCooldown, _currentCooldownText, _skillEffect, _buffDurationImage, _currentBuffDurationText)
    {
        Name = "Equinox";
        Description = "Equinox - Immune from one enemy attack";
        LevelRequired = 7;
        Cooldown = 14;
        CurrentCooldown = 0;
        CanBeUsed = true;
        Duration = 5;
        CurrentDuration = 0;
        isActive = false;

        SkillCooldown = _skillCooldown;
        CurrentCooldownText = _currentCooldownText;
        SkillEffect = _skillEffect;
        BuffDurationImage = _buffDurationImage;
        CurrentBuffDurationText = _currentBuffDurationText;
    }

    public override void ReduceCurrentDuration()
    {
        CurrentDuration--;
        if (CurrentDuration == 0)
        {
            DeactivateSkill();
        }
    }
    public override void ActivateSkill()
    {
        isActive = true;

        CurrentDuration = Duration;
        SkillEffect.SetActive(true);
        BuffDurationImage.SetActive(true);
    }

    public override void DeactivateSkill()
    {
        isActive = false;
        SkillEffect.SetActive(false);
        BuffDurationImage.SetActive(false);
    }
}
