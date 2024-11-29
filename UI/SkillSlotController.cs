using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotController : MonoBehaviour
{
    public static SkillSlotController Instance { get; private set; }
    
    public PlayerSkillsSO PlayerSkills;
    public PlayerStatsSO PlayerStats;

    [Header("Skill Event Channel")]     
    public VoidEventChannel SkillCooldownEventChannel;
    public GameObjectEventChannel SkillUsedEventChannel;

    private void Start()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
    }

    private void OnEnable()
    {
        SkillCooldownEventChannel.OnRaiseVoidEvent += ReduceCooldown;
        SkillCooldownEventChannel.OnRaiseVoidEvent += UpdateCooldownUI;
        SkillUsedEventChannel.OnRaiseGameObjectEvent += UseSkill;
    }

    private void OnDisable()
    {
        SkillCooldownEventChannel.OnRaiseVoidEvent -= ReduceCooldown;
        SkillCooldownEventChannel.OnRaiseVoidEvent -= UpdateCooldownUI;
        SkillUsedEventChannel.OnRaiseGameObjectEvent -= UseSkill;
    }

    private void ReduceCooldown()
    {
        if (PlayerSkills.PlayerSkillsList.Count == 0) return;
        foreach (var skill in PlayerSkills.PlayerSkillsList)
        {
            if (skill.GetCanBeUsed() == false)
            {
                skill.ReduceCurrentCooldown();
                if (skill.GetCurrentCooldown() == 0)
                {
                    skill.SetCanBeUsed(true);
                }
            }
            if (skill is PassiveSkill passiveSkill)
            {
                passiveSkill.ReduceCurrentDuration();   
            }
        }
    }

    public void UpdateCooldownUI()
    {
        if (PlayerSkills.PlayerSkillsList.Count == 0) return;
        foreach (var skill in PlayerSkills.PlayerSkillsList)
        {
            skill.SkillCooldown.fillAmount = (float) skill.GetCurrentCooldown() / (float) skill.GetCooldown();
            
            skill.CurrentCooldownText.GetComponent<Text>().text = (skill.GetCurrentCooldown() == 0) ?  "" : skill.GetCurrentCooldown().ToString();

            if (skill is PassiveSkill passiveSkill)
            {
                passiveSkill.BuffDurationImage.GetComponentInChildren<Image>().fillAmount = (float) passiveSkill.CurrentDuration / (float) passiveSkill.Duration;
                passiveSkill.CurrentBuffDurationText.GetComponent<Text>().text = (passiveSkill.CurrentDuration <= 0) ? "" : passiveSkill.CurrentDuration.ToString();
            }
        }
        
    }

    private void UseSkill(GameObject enemy)
    {
        if (PlayerSkills.PlayerSkillsList.Count == 0) return;
        for (var i = 0; i < PlayerSkills.PlayerSkillsList.Count; i++)
        {
            var skill = PlayerSkills.PlayerSkillsList[i];
            if (skill is ActiveSkill activeSkill && activeSkill.GetCanBeUsed() == true && activeSkill.isToggle == true)
            {
                enemy.transform.Find($"Canvas/Bar/{skill.GetName() + " Damage Text"}").GetComponent<DamageText>().Activate(activeSkill.SkillDamage(PlayerStats.Damage), Color.blue);
                enemy.transform.GetComponent<Enemy>().Stat.CurrentHealth -= activeSkill.SkillDamage(PlayerStats.Damage);
                activeSkill.isToggle = false;
                SkillSystem.Instance.SkillToggleImageList[i].gameObject.SetActive(false);
                SkillSystem.Instance.SkillEffectList[i].SetActive(false);
                activeSkill.SetCanBeUsed(false);
                UpdateCooldownUI();
            }
        }
    }   
    
}
