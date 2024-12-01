using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSystem : MonoBehaviour
{
    
    public static SkillSystem Instance { get; private set; }

    [Header("Player Data")]
    public PlayerStatsSO PlayerStats;
    public PlayerSkillsSO PlayerSkills;
    [HideInInspector] private List<Skill> AvailableSkills = new List<Skill>();
    
    [Header("Skill Cooldown")]
    public List<Image> SkillCooldownList = new List<Image>();
    public List<GameObject> CurrentCooldownTextList = new List<GameObject>();
     
    [Header("Skill Effect")]
    public List<GameObject> SkillEffectList = new List<GameObject>();
    
    [Header("Toggle Skill Image")]
    public List<Image> SkillToggleImageList = new List<Image>();
    
    [Header("Buff Duration Skill")]
    public List<GameObject> SkillDurationTextList = new List<GameObject>();
    public List<GameObject> SkillDurationGameObjectList = new List<GameObject>();
    
    [Header("Event Channel")]
    public VoidEventChannel SkillCooldownEventChannel;

    private enum AllSkillIndex
    {
        ARCANE_STRIKE,
        RAGE,
    }
    private enum ActiveSkillIndex
    {
        ARCANE_STRIKE,
        
    }

    private enum PassiveSkillIndex
    {
        RAGE,
    }
    
    private void Start()
    {   
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        PlayerSkills.PlayerSkillsList = new List<Skill>();
        AvailableSkills.Add(new ArcaneStrike(SkillCooldownList[(int)AllSkillIndex.ARCANE_STRIKE], CurrentCooldownTextList[(int)AllSkillIndex.ARCANE_STRIKE], SkillEffectList[(int)AllSkillIndex.ARCANE_STRIKE]));
        AvailableSkills.Add(new Rage(SkillCooldownList[(int)AllSkillIndex.RAGE], CurrentCooldownTextList[(int)AllSkillIndex.RAGE], SkillEffectList[(int)AllSkillIndex.RAGE], SkillDurationGameObjectList[(int)PassiveSkillIndex.RAGE], SkillDurationTextList[(int)PassiveSkillIndex.RAGE]));
     }

    private void Update()
    {
        for (var i = 0; i < AvailableSkills.Count; i++)
        {
            if (PlayerStats.Level >= AvailableSkills[i].GetLevelRequired())
            {
                PlayerSkills.PlayerSkillsList.Add(AvailableSkills[i]);
                AvailableSkills.RemoveAt(i);
            }
        }
    }

    public void ActivateSkill1()
    {
        if (PlayerSkills.PlayerSkillsList.Count == 0) return;
        if (PlayerSkills.PlayerSkillsList.Count - 1 < (int)AllSkillIndex.ARCANE_STRIKE) return;
        if (PlayerSkills.PlayerSkillsList[(int)AllSkillIndex.ARCANE_STRIKE].GetCanBeUsed() == false) return;

        if (PlayerSkills.PlayerSkillsList[(int)AllSkillIndex.ARCANE_STRIKE] is ActiveSkill activeSkill)
        {
            if (!activeSkill.isToggle)
            {
                SkillEffectList[(int)AllSkillIndex.ARCANE_STRIKE].SetActive(true);
                SkillToggleImageList[(int)ActiveSkillIndex.ARCANE_STRIKE].gameObject.SetActive(true);
                activeSkill.isToggle = true;
                AudioManager.Instance.PlayArcaneStrikeToggle(Player.Instance.transform);
            }
            else
            {
                SkillEffectList[(int)AllSkillIndex.ARCANE_STRIKE].SetActive(false);
                SkillToggleImageList[(int)ActiveSkillIndex.ARCANE_STRIKE].gameObject.SetActive(false);
                activeSkill.isToggle = false;
            }
        }
    }
    
    public void ActivateSkill2()
    {
        if (PlayerSkills.PlayerSkillsList.Count == 0) return;
        if (PlayerSkills.PlayerSkillsList.Count - 1 < (int)AllSkillIndex.RAGE) return;
        if (PlayerSkills.PlayerSkillsList[(int)AllSkillIndex.RAGE].GetCanBeUsed() == false) return;
        
        if (PlayerSkills.PlayerSkillsList[(int)AllSkillIndex.RAGE] is PassiveSkill passiveSkill)
        {
            if (passiveSkill.GetCanBeUsed())
            {
                Player.Instance.animator.SetTrigger("buff");
                passiveSkill.SetCanBeUsed(false);
                passiveSkill.ActivateSkill();
                SkillSlotController.Instance.UpdateCooldownUI();
            }
        }
    }
}
