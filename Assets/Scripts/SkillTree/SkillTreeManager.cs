using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    // Singleton pattern implementation
    private static SkillTreeManager _instance;
    public static SkillTreeManager Instance => _instance;

    private Dictionary<SkillType, int> currentLevels;
    private Dictionary<SkillType, List<Skill>> m_skills;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        currentLevels = new Dictionary<SkillType, int>() 
        {
            { SkillType.Weapon, 0 },
            { SkillType.Trap, 0 },
            { SkillType.Player, 0 }
        };

        m_skills = new Dictionary<SkillType, List<Skill>>();

        loadAllSkills();
    }

    private void loadAllSkills()
    {
        Skill[] loadedSkills = Resources.LoadAll<Skill>("Skills");

        foreach (Skill skill in loadedSkills)
        {
            if ( !m_skills.ContainsKey(skill.skillType) )
                m_skills[skill.skillType] = new List<Skill>();

            m_skills[skill.skillType].Add(skill);
        }

        foreach ( List<Skill> list in m_skills.Values )
        {
            list.Sort((a, b) => a.level.CompareTo(b.level));
        }
    }

    public Dictionary<SkillType, List<Skill>> getSkills()
    {
        return m_skills;
    }

    public bool canUnlock( Skill skill )
    {
        int currentLevel = currentLevels[skill.skillType];
        return skill.level == currentLevel + 1;
    }

    public void unlockSkill( Skill skill )
    {
        if ( !canUnlock(skill) ) return;

        currentLevels[skill.skillType]++;

        applySkill( skill );
    }

    public bool isUnlocked( Skill skill )
    {
        int currentLevel = currentLevels[skill.skillType];
        return skill.level <= currentLevel;
    }
    
    private void applySkill( Skill skill )
    {
    }
}
