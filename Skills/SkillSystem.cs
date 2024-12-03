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
    
    [Header("Locked Image")]
    public List<GameObject> LockedSkillImageList = new List<GameObject>();
    
    [Header("Skill Description")]
    public List<Text> SkillsDescription = new List<Text>();
    
    [Header("Event Channel")]
    public VoidEventChannel SkillCooldownEventChannel;

    private int SkillCounterIndex = 0;
    private enum AllSkillIndex
    {
        ARCANE_STRIKE,
        RAGE,
        EQUINOX,
    }
    private enum ActiveSkillIndex
    {
        ARCANE_STRIKE,
        
    }

    private enum PassiveSkillIndex
    {
        RAGE,
        EQUINOX
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
        AvailableSkills.Add(new Equinox(SkillCooldownList[(int)AllSkillIndex.EQUINOX], CurrentCooldownTextList[(int)AllSkillIndex.EQUINOX], SkillEffectList[(int)AllSkillIndex.EQUINOX], SkillDurationGameObjectList[(int)PassiveSkillIndex.EQUINOX], SkillDurationTextList[(int)PassiveSkillIndex.EQUINOX]));
     }

    private void Update()
    {
        for (var i = SkillCounterIndex; i < AvailableSkills.Count; i++)
        {
            if (PlayerStats.Level >= AvailableSkills[i].GetLevelRequired())
            {
                PlayerSkills.PlayerSkillsList.Add(AvailableSkills[i]);
                SkillCooldownEventChannel.RaiseVoidEvent();
                LockedSkillImageList[i].SetActive(false);
                SkillsDescription[i].text = AvailableSkills[i].GetDescription();
                SkillCounterIndex++;
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
    
    public void ActivateSkill3()
    {
        if (PlayerSkills.PlayerSkillsList.Count == 0) return;
        if (PlayerSkills.PlayerSkillsList.Count - 1 < (int)AllSkillIndex.EQUINOX) return;
        if (PlayerSkills.PlayerSkillsList[(int)AllSkillIndex.EQUINOX].GetCanBeUsed() == false) return;
        
        if (PlayerSkills.PlayerSkillsList[(int)AllSkillIndex.EQUINOX] is PassiveSkill passiveSkill)
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
