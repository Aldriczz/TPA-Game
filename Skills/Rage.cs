using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rage : PassiveSkill
{
    private float BuffATKnCritChance = 0.2f;
    private float DebuffDefense = 0.3f;

    private int AtkbuffTemp;
    private int CritChanceBuffTemp;
    private int DefenseDebuffTemp;
    private PlayerStatsSO PlayerStats;
    
    public Rage(Image _skillCooldown, GameObject _currentCooldownText, GameObject _skillEffect, GameObject _buffDurationImage, GameObject _currentBuffDurationText) : base(_skillCooldown, _currentCooldownText, _skillEffect, _buffDurationImage, _currentBuffDurationText)
    {
        Name = "Rage";
        Description = "Rage - THe rage of arcane, increase 20% damage and crit chance while reducing defense by 30%";
        LevelRequired = 5;
        Cooldown = 9;
        CurrentCooldown = 0;
        CanBeUsed = true;
        Duration = 3;
        CurrentDuration = 0;
        isActive = false;
        PlayerStats = SkillSlotController.Instance.PlayerStats;

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
        AtkbuffTemp = (int) Mathf.Round(PlayerStats.Damage * BuffATKnCritChance);
        CritChanceBuffTemp = (int) Mathf.Round(PlayerStats.CritChance * BuffATKnCritChance);
        DefenseDebuffTemp = (int) Mathf.Round(PlayerStats.Defense * DebuffDefense);
        Debug.Log("DEBUFF: " + DebuffDefense);
        PlayerStats.Damage += AtkbuffTemp;
        PlayerStats.CritChance += CritChanceBuffTemp;
        PlayerStats.Defense -= DefenseDebuffTemp;

        CurrentDuration = Duration;
        SkillEffect.SetActive(true);
        BuffDurationImage.SetActive(true);
    }

    public override void DeactivateSkill()
    {
        isActive = false;
        PlayerStats.Damage -= AtkbuffTemp;
        PlayerStats.CritChance -= CritChanceBuffTemp;
        PlayerStats.Defense += DefenseDebuffTemp;
        SkillEffect.SetActive(false);
        BuffDurationImage.SetActive(false);
    }
}
