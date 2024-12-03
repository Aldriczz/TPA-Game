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

    [Header("Skill Event Channel")] public VoidEventChannel SkillCooldownEventChannel;
    public GameObjectEventChannel MeleeSkillUsedEventChannel;
    public GameObjectEventChannel RangeSkillUsedEventChannel;
    public VoidEventChannel UpdateRangeSkillPositionEventChannel;

    private GameObject CurrentEnemyTargetSkillRange;

    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        SkillCooldownEventChannel.OnRaiseVoidEvent += ReduceCooldown;
        SkillCooldownEventChannel.OnRaiseVoidEvent += UpdateCooldownUI;
        MeleeSkillUsedEventChannel.OnRaiseGameObjectEvent += UseActiveMeleeSkill;
        RangeSkillUsedEventChannel.OnRaiseGameObjectEvent += UseActiveRangeSkill;
        UpdateRangeSkillPositionEventChannel.OnRaiseVoidEvent += UpdateRangeSkillPosition;
    }

    private void OnDisable()
    {
        SkillCooldownEventChannel.OnRaiseVoidEvent -= ReduceCooldown;
        SkillCooldownEventChannel.OnRaiseVoidEvent -= UpdateCooldownUI;
        MeleeSkillUsedEventChannel.OnRaiseGameObjectEvent -= UseActiveMeleeSkill;
        RangeSkillUsedEventChannel.OnRaiseGameObjectEvent -= UseActiveRangeSkill;
        UpdateRangeSkillPositionEventChannel.OnRaiseVoidEvent -= UpdateRangeSkillPosition;
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
            skill.SkillCooldown.fillAmount = (float)skill.GetCurrentCooldown() / (float)skill.GetCooldown();

            skill.CurrentCooldownText.GetComponent<Text>().text =
                (skill.GetCurrentCooldown() == 0) ? "" : skill.GetCurrentCooldown().ToString();

            if (skill is PassiveSkill passiveSkill)
            {
                passiveSkill.BuffDurationImage.GetComponentInChildren<Image>().fillAmount = (float)passiveSkill.CurrentDuration / (float)passiveSkill.Duration;
                passiveSkill.CurrentBuffDurationText.GetComponent<Text>().text = (passiveSkill.CurrentDuration <= 0) ? "" : passiveSkill.CurrentDuration.ToString();
            }
        }

    }

    private void UseActiveMeleeSkill(GameObject enemy)
    {
        if (PlayerSkills.PlayerSkillsList.Count == 0) return;
        var ActiveSkillCount = 0;
        for (var i = 0; i < PlayerSkills.PlayerSkillsList.Count; i++)
        {
            var skill = PlayerSkills.PlayerSkillsList[i];
            if (skill is ActiveSkill activeSkill)
            {
                if (activeSkill.GetCanBeUsed() == true && activeSkill.isToggle == true &&
                    activeSkill.ActiveSkillType == "Melee")
                {
                    AudioManager.Instance.PlayArcaneStrikeImpact(Player.Instance.transform);
                    enemy.transform.Find("Canvas/Bar/Skill Damage Text").GetComponent<FloatingText>()
                        .ActivateDamageText(activeSkill.SkillDamage(PlayerStats.Damage), Color.blue);
                    enemy.transform.GetComponent<Enemy>().Stat.CurrentHealth -=
                        activeSkill.SkillDamage(PlayerStats.Damage);
                    activeSkill.isToggle = false;
                    SkillSystem.Instance.SkillToggleImageList[ActiveSkillCount].gameObject.SetActive(false);
                    SkillSystem.Instance.SkillEffectList[i].SetActive(false);
                    activeSkill.SetCanBeUsed(false);
                    UpdateCooldownUI();
                    StartCoroutine(Player.Instance.cameraShake.Shake(0.3f, 0.01f));
                }
                ActiveSkillCount++;
            }
        }
    }

    private void UseActiveRangeSkill(GameObject enemy)
    {
        if (PlayerSkills.PlayerSkillsList.Count == 0) return;
        var ActiveSkillCount = 0;
        for (var i = 0; i < PlayerSkills.PlayerSkillsList.Count; i++)
        {
            var skill = PlayerSkills.PlayerSkillsList[i];
            if (skill is ActiveSkill activeSkill)
            {
                if (activeSkill.GetCanBeUsed() == true && activeSkill.isToggle == true &&
                    activeSkill.ActiveSkillType == "Range")
                {
                    CurrentEnemyTargetSkillRange = enemy;
                    // AudioManager.Instance.PlayArcaneStrikeImpact(Player.Instance.transform);
                    ShowRangeSkillFX(enemy);
                    StartCoroutine(DivineStrikeDamageOutput(enemy, activeSkill.SkillDamage(PlayerStats.Damage)));
                    activeSkill.isToggle = false;
                    SkillSystem.Instance.SkillToggleImageList[ActiveSkillCount].gameObject.SetActive(false);
                    SkillSystem.Instance.SkillEffectList[i].SetActive(false);
                    activeSkill.SetCanBeUsed(false);
                    UpdateCooldownUI();
                    StartCoroutine(Player.Instance.cameraShake.Shake(0.3f, 0.01f));
                }
                ActiveSkillCount++;
            }
        }
        TurnGameManager.Instance.SwitchGameState();
    }

    private void ShowRangeSkillFX(GameObject enemy)
    {
        for (var i = 0; i < PlayerSkills.PlayerSkillsList.Count; i++)
        {
            var skill = PlayerSkills.PlayerSkillsList[i];
            if (skill is ActiveSkill activeSkill)
            {
                if (activeSkill.GetCanBeUsed() == true && activeSkill.isToggle == true &&
                    activeSkill.ActiveSkillType == "Range")
                {
                    activeSkill.ProjectileFX.transform.position = enemy.transform.position;
                    activeSkill.ProjectileFX.SetActive(true);
                }
            }
        }
    }
    
    private void HideRangeSkillFX()
    {
        for (var i = 0; i < PlayerSkills.PlayerSkillsList.Count; i++)
        {
            var skill = PlayerSkills.PlayerSkillsList[i];
            if (skill is ActiveSkill activeSkill)
            {
                if (activeSkill.GetCanBeUsed() == false && activeSkill.isToggle == false &&
                    activeSkill.ActiveSkillType == "Range" && activeSkill.ProjectileFX.activeSelf)
                {
                    activeSkill.ProjectileFX.SetActive(false);
                }
            }
        }
    }

    private void UpdateRangeSkillPosition()
    {
        foreach (var skill in PlayerSkills.PlayerSkillsList)
        {
            if (skill is ActiveSkill activeSkill && skill.GetName() == "Divine Arcane" &&
                activeSkill.ProjectileFX.activeSelf)
            {
                activeSkill.ProjectileFX.transform.position = CurrentEnemyTargetSkillRange.transform.position;
            }
        }
    }

    private IEnumerator DivineStrikeDamageOutput(GameObject enemy, int damage)
    {
        Player.Instance.animator.SetTrigger("divinearcane");
        for (var j = 0; j < 6; j++)
        {
            enemy.GetComponent<Enemy>().Gethit(damage, Color.yellow);
            yield return new WaitForSeconds(0.6f);
        }
        HideRangeSkillFX();
    }
}

    
