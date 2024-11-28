using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skills", menuName = "SO/Skills")]
public class PlayerSkillsSO : ScriptableObject
{
   public List<Skill> PlayerSkillsList = new List<Skill>();
}
