using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill
{
    protected string Name;
    protected string Description;
    protected int LevelRequired;
    protected int Cooldown;
    protected int CurrentCooldown;
    protected bool CanBeUsed;
    public Image SkillCooldown;
    public GameObject CurrentCooldownText;
    public GameObject SkillEffect;
    
    public string GetName()
    {
        return Name;
    }
    public string GetDescription()
    {
        return Description;
    }
    public int GetLevelRequired()
    {
        return LevelRequired;
    }
    
    public int GetCooldown()
    {
        return Cooldown;
    }
    
    public int GetCurrentCooldown()
    {
        return CurrentCooldown;
    }
    
    public void SetCurrentCooldown(int _currentCooldown)
    {
        CurrentCooldown = _currentCooldown;
    }

    public void ReduceCurrentCooldown()
    {
        CurrentCooldown--;
    }

    public bool GetCanBeUsed()
    {
        return CanBeUsed;
    }

    public void SetCanBeUsed(bool _canBeUsed)
    {
        if (!_canBeUsed)
        {
            CurrentCooldown = Cooldown;
        }
        else
        {
            CurrentCooldown = 0;
        }
        CanBeUsed = _canBeUsed;
    }

    protected Skill(Image _skillImage, GameObject _currentCooldownText, GameObject _skillEffect)
    {
        SkillCooldown = _skillImage;
        CurrentCooldownText = _currentCooldownText;
        SkillEffect = _skillEffect;
    }
    
}

public abstract class ActiveSkill : Skill
{
    public bool isToggle;
    public float DamageBoost;

    protected ActiveSkill(Image _skillImage, GameObject _currentCooldownText, GameObject _skillEffect) : base(_skillImage, _currentCooldownText, _skillEffect)
    {
        SkillCooldown = _skillImage;
        CurrentCooldownText = _currentCooldownText;
    }
    
    public abstract int SkillDamage(int PlayerDamage);
}   
    
public abstract class PassiveSkill : Skill
{
    public int Duration;
    public int CurrentDuration;
    public bool isActive;
    public GameObject BuffDurationImage;
    public GameObject CurrentBuffDurationText;

    public abstract void ReduceCurrentDuration();
    public abstract void ActivateSkill();
    public abstract void DeactivateSkill();
    protected PassiveSkill(Image _skillImage, GameObject _currentCooldownText, GameObject _skillEffect, GameObject _buffDurationImage, GameObject _currentBuffDurationText) : base(_skillImage, _currentCooldownText, _skillEffect)
    {
        SkillCooldown = _skillImage;
        CurrentCooldownText = _currentCooldownText;
        SkillEffect = _skillEffect;
    }
    
}   
