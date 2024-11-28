using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotController : MonoBehaviour
{
    public PlayerSkillsSO PlayerSkills;
    public PlayerStatsSO PlayerStats;
    
    [Header("Skill Cooldown")]
    public Image SkillCooldown1;
    public GameObject CurrentCooldownText1;

    [Header("Skill Event Channel")] 
    public IntEventChannel SkillCooldownEventChannel;
    public GameObjectEventChannel SkillUsedEventChannel;

    private void OnEnable()
    {
        SkillCooldownEventChannel.OnRaiseIntEvent += ReduceCooldown;
        SkillCooldownEventChannel.OnRaiseIntEvent += UpdateCooldownUI;
        SkillUsedEventChannel.OnRaiseGameObjectEvent += UseSkill;
    }

    private void OnDisable()
    {
        SkillCooldownEventChannel.OnRaiseIntEvent -= ReduceCooldown;
        SkillCooldownEventChannel.OnRaiseIntEvent -= UpdateCooldownUI;
        SkillUsedEventChannel.OnRaiseGameObjectEvent -= UseSkill;
    }

    private void ReduceCooldown(int turn)
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
        }
    }

    private void UpdateCooldownUI(int turn)
    {
        if (PlayerSkills.PlayerSkillsList.Count == 0) return;
        
        SkillCooldown1.fillAmount = (float) PlayerSkills.PlayerSkillsList[0].GetCurrentCooldown() / (float) PlayerSkills.PlayerSkillsList[0].GetCooldown();

        if (PlayerSkills.PlayerSkillsList[0].GetCurrentCooldown() == 0)
        {
            CurrentCooldownText1.GetComponent<Text>().text = "";
        }
        else
        {
            CurrentCooldownText1.GetComponent<Text>().text = PlayerSkills.PlayerSkillsList[0].GetCurrentCooldown().ToString();
        }
    }

    private void UseSkill(GameObject enemy)
    {
        if (PlayerSkills.PlayerSkillsList.Count == 0) return;
        foreach (var skill in PlayerSkills.PlayerSkillsList)
        {
            if (skill is ActiveSkill activeSkill && activeSkill.GetCanBeUsed() == true && activeSkill.isToggle == true)
            {
                enemy.transform.Find($"Canvas/Bar/{skill.GetName() + " Damage Text"}").GetComponent<DamageText>().Activate(activeSkill.SkillDamage(PlayerStats.Damage), Color.blue);
                enemy.transform.GetComponent<Enemy>().Stat.CurrentHealth -= activeSkill.SkillDamage(PlayerStats.Damage);
                activeSkill.isToggle = false;
                SkillSystem.Instance.Skill1ToggleImage.gameObject.SetActive(false);
                SkillSystem.Instance.SkillEffect1.SetActive(false);
                activeSkill.SetCanBeUsed(false);
                UpdateCooldownUI(0);
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
