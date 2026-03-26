using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Weapon,
    Trap,
    Player
}

[CreateAssetMenu(menuName = "Skill Tree/Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public string description;
    public int cost;
    public int level;
    public SkillType skillType;

}