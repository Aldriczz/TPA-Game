using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSystem : MonoBehaviour
{
    
    public static SkillSystem Instance { get; private set; }

    public PlayerStatsSO PlayerStats;
    public PlayerSkillsSO PlayerSkills;
     
    public GameObject SkillEffect1;
    
    [Header("Toggle Skill Image")]
    public Image Skill1ToggleImage;
    
    [HideInInspector] public List<Skill> Skills;

    
    // Start is called before the first frame update
    void Start()
    {   
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        
        Skills = new List<Skill>();
        Skills.Add(new ArcaneStrike());
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < Skills.Count; i++)
        {
            if (PlayerStats.Level >= Skills[i].GetLevelRequired())
            {
                PlayerSkills.PlayerSkillsList.Add(Skills[i]);
                Skills.RemoveAt(i);
            }
        }
    }

    public void ActivateSkill1()
    {
        if (PlayerSkills.PlayerSkillsList.Count == 0 || PlayerSkills.PlayerSkillsList[0].GetCanBeUsed() == false)
        {
            return;
        }

        if (PlayerSkills.PlayerSkillsList[0] is ActiveSkill activeSkill)
        {
            if (!activeSkill.isToggle)
            {
                SkillEffect1.SetActive(true);
                Skill1ToggleImage.gameObject.SetActive(true);
                activeSkill.isToggle = true;
            }
            else
            {
                SkillEffect1.SetActive(false);
                Skill1ToggleImage.gameObject.SetActive(false);
                activeSkill.isToggle = false;
            }
        }
    }
}
