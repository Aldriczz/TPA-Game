using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    protected string Name;
    protected string Description;
    protected int LevelRequired;
    protected int Cooldown;
    protected int CurrentCooldown;
    protected bool CanBeUsed;

    public abstract void ActivateSkill();

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
    
}

public abstract class ActiveSkill : Skill
{
    public bool isToggle;
    public float DamageBoost;

    public abstract int SkillDamage(int PlayerDamage);
}   
    
public abstract class PassiveSkill : Skill
{
    public int Duration;
    
}   
